using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Entity for drift regime.
/// </summary>
public class DriftRaceEntity :BaseRaceEntity
{
	public CarStatisticsDriftRegime PlayerDriftStatistics { get; private set; }
	public List<CarStatisticsDriftRegime> DriftStatistics { get; private set; }

	public DriftRaceEntity (GameController controller): base(controller)
	{
		DriftStatistics = new List<CarStatisticsDriftRegime> ();
		foreach (var car in AllCars)
		{
			CarStatisticsDriftRegime statistic;

			if (car == PlayerCar)
			{
				statistic = new CarStatisticsDriftRegime (car, WorldLoading.PlayerName);
				PlayerDriftStatistics = statistic;
			}
			else
			{
				statistic = new CarStatisticsDriftRegime (car, GetNameForBot());
			}

			statistic.TotalScoreChanged += CheckRatingOfPlayers;

			DriftStatistics.Add (statistic);
		}
	}

	/// <summary>
	/// Sort players by total score, if there have been changes.
	/// </summary>
	public override void CheckRatingOfPlayers ()
	{
		bool ratingChanged = false;
		for (int i = 1; i < DriftStatistics.Count; i++)
		{
			if (DriftStatistics[i].TotalScore > DriftStatistics[i - 1].TotalScore)
			{
				ratingChanged = true;
				break;
			}
		}

		if (ratingChanged)
		{
			DriftStatistics = DriftStatistics.OrderBy (s => -s.TotalScore).ToList ();
		}
		Controller.RatingOfPlayersChanged.SafeInvoke ();
	}
}
