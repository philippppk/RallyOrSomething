using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main menu window.
/// </summary>
public class MainMenuUI :WindowWithShowHideAnimators
{
	[SerializeField] Button StartGameButton;
	[SerializeField] Button SettingsButton;
	[SerializeField] Button ResultsButton;
	[SerializeField] Button QuitButton;
	[SerializeField] Window SelectTrackWindow;
	[SerializeField] Window SettingsWindow;
	[SerializeField] Window ResultsWindow;

	protected override void Awake ()
	{
		StartGameButton.onClick.AddListener (StartGame);
		SettingsButton.onClick.AddListener (Settings);
		ResultsButton.onClick.AddListener (Results);
		QuitButton.onClick.AddListener (Quit);
		base.Awake ();
	}

	private void StartGame ()
	{
		WindowsController.Instance.OpenWindow (SelectTrackWindow);
	}

	private void Settings ()
	{
		WindowsController.Instance.OpenWindow (SettingsWindow);
	}

	private void Results ()
	{
		WindowsController.Instance.OpenWindow (ResultsWindow);
	}

	private void Quit ()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

}
