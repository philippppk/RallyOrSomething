using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Base class game controller.
/// </summary>
public class GameController :MonoBehaviour
{
	[SerializeField] GameObject CountdownObject;
	[SerializeField] float CountdownTime = 3;
	[SerializeField] float DellayCountdownShowHide = 1;
	[SerializeField] List<Transform> CarPositions = new List<Transform>();
	[SerializeField] PositioningSystem m_PositioningSystem;

	[Space(10)]
	[SerializeField] GameBalance.RegimeSettings RegimeForDebug;

	public static GameController Instance;
	public static BaseRaceEntity RaceEntity;
	public static bool RaceIsStarted { get {  return Instance.m_RaceIsStarted; } }
	public static CarController PlayerCar { get { return Instance.m_PlayerCar; } }
	public static List<CarController> AllCars { get { return Instance.m_AllCars; } }
	public static bool InGameScene { get { return Instance != null; } }
    public static bool InMainMenuScene { get { return SceneManager.GetActiveScene ().name == B.GameSettings.MainMenuSceneName; } }
    public static bool InPause { get { return Mathf.Approximately (Time.timeScale, 0); } }

	public static bool RaceIsEnded { get { return Instance.m_GameIsEnded; } }

	public PositioningSystem PositioningSystem { get { return m_PositioningSystem; } }

	bool m_GameIsEnded;
	bool m_RaceIsStarted;

	public Action RatingOfPlayersChanged;
	public Action OnEndGameAction;
	public Action OnStartRaceAction;

	public Action FixedUpdateAction;

	List<CarController> m_AllCars = new List<CarController>();
	CarController m_PlayerCar;

	void Awake ()
	{
		if (!WorldLoading.HasLoadingParams)
		{
			WorldLoading.RegimeForDebug = RegimeForDebug;
			LoadingScreenUI.LoadScene (RegimeForDebug.RegimeSceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
		}
		Instance = this;
		OnEndGameAction += () => m_GameIsEnded = true;

		StartCoroutine (StartRaceCoroutine ());

		//Find all cars in current game.
		foreach (var car in GameObject.FindObjectsOfType<CarController> ())
		{
			if (car.GetComponent<UserControl> () != null)
			{
				if (m_PlayerCar != null)
				{
					Debug.LogErrorFormat ("CarControllers with UserControl script count > 1");
				}
				else
				{
					m_PlayerCar = car;
				}
			}
			m_AllCars.Add (car);
		}

		CarPositions.ForEach (p => p.SetActive (false));

		//Destroy All AudioListeners
		foreach (var car in m_AllCars)
		{
			var audioListener = car.GetComponent<AudioListener> ();
			if (audioListener != null)
			{
				Destroy (audioListener);
			}
		}

		//For debug load scene
		if (!WorldLoading.HasLoadingParams)
		{
			if (AllCars.All (c => c.GetComponent<UserControl> () == null))
			{
				var car = AllCars.First ();
				var userControl = car.gameObject.AddComponent<UserControl> ();
				m_PlayerCar = car;

				if (car.GetComponent<DriftAIControl>() != null)
				{
					userControl.enabled = false;
				}
			}

			if (m_PlayerCar != null)
			{
                m_PlayerCar.gameObject.AddComponent<AudioListener> ();
			}
			else
			{
				Debug.LogErrorFormat ("[Debug Scene] PlayerCar not found ");
			}

            AllCars.ForEach(c => SetDriftConfigForCar (c));

            InitRaceEntity ();
			return;
		}

		//Destroy all cars in scene if load from world loading.
		m_AllCars.ForEach (c => Destroy (c.gameObject));
		m_AllCars.Clear ();

		//Initialize player car
		m_PlayerCar = GameObject.Instantiate (WorldLoading.PlayerCar.CarPrefab);
        SetDriftConfigForCar (m_PlayerCar);

        if (m_PlayerCar.GetComponent<UserControl> () == null)
		{
			m_PlayerCar.gameObject.AddComponent<UserControl> ();
		}

		m_PlayerCar.SetColor (WorldLoading.SelectedColor);
		m_AllCars.Add (m_PlayerCar);

		//Initialize AI cars
		for (int i = 0; i < WorldLoading.AIsCount; i++)
		{
			var carPreset = WorldLoading.AvailableCars.RandomChoice();
			var car = GameObject.Instantiate (carPreset.CarPrefab);
            SetDriftConfigForCar (car);
            car.gameObject.AddComponent<DriftAIControl> ();

			var userControl = car.GetComponent<UserControl> ();
			if (userControl != null)
			{
				Destroy (userControl);
			}

			car.SetColor (carPreset.GetRandomColor ());

			m_AllCars.Add (car);
		}

		//Set random start positions.
		for (int i = 0; i < m_AllCars.Count; i++)
		{
			int j = UnityEngine.Random.Range (0, m_AllCars.Count);
			var temp = m_AllCars[i];
			m_AllCars[i] = m_AllCars[j];
			m_AllCars[j] = temp;
		}

		m_PlayerCar.gameObject.AddComponent<AudioListener> ();

		if (m_AllCars.Count > CarPositions.Count)
		{
			Debug.LogErrorFormat ("CarPositions less loaded cars count: CarPositions: {0}, Loaded cars: {1}", CarPositions.Count, m_AllCars.Count);
			return;
		}

		for (int i = 0; i < m_AllCars.Count; i++)
		{
			m_AllCars[i].transform.position = CarPositions[i].position;
			m_AllCars[i].transform.rotation = CarPositions[i].rotation;
            SetDriftConfigForCar (m_AllCars[i]);
        }

		InitRaceEntity ();
	}

    void SetDriftConfigForCar (CarController car)
    {
        car.SetCarDriftConfig (WorldLoading.RegimeSettings.CarDriftConfig);
        car.GetFrontLeftWheel.UpdateFrictionConfig (WorldLoading.RegimeSettings.FrontWheelsConfig);
        car.GetFrontRightWheel.UpdateFrictionConfig (WorldLoading.RegimeSettings.FrontWheelsConfig);
        car.GetRearLeftWheel.UpdateFrictionConfig (WorldLoading.RegimeSettings.RearWheelsConfig);
        car.GetRearRightWheel.UpdateFrictionConfig (WorldLoading.RegimeSettings.RearWheelsConfig);
    }

    void InitRaceEntity ()
	{
		if (WorldLoading.RegimeSettings is GameBalance.DriftRegimeSettings)
		{
			RaceEntity = new DriftRaceEntity (this);
		}
		else
		{
			RaceEntity = new RaceEntity (this);
		}
	}

	/// <summary>
	/// Delay start the race and the inclusion of the countdown.
	/// </summary>
	IEnumerator StartRaceCoroutine ()
	{
		while (!LoadingScreenUI.IsLoaded)
		{
			yield return null;
		}

		var countdownObject = Instantiate(CountdownObject);

		yield return new WaitForSeconds (CountdownTime);

		OnStartRaceAction.SafeInvoke ();
		m_RaceIsStarted = true;

		yield return new WaitForSeconds (DellayCountdownShowHide);

		countdownObject.SetActive (false);
	}

	void Update () { }
	void FixedUpdate () 
	{
		FixedUpdateAction.SafeInvoke ();
	}

}
