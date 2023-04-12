using System;
using UnityEngine;
using Zenject;

public class MainEnterPoint : MonoBehaviour
{
	private MainPlayer mainPlayer;
	private Level level;
	private Controller controller;
	private Counter counter;
	private MainMenuUI mainMenuUI;

	[Inject]
	private void Construct(MainPlayer mainPlayer, Level level, Counter counter, MainMenuUI mainMenuUI)
	{
		this.mainPlayer = mainPlayer;
		this.level = level;
		this.counter = counter;
		this.mainMenuUI = mainMenuUI;
	}

	private void Start()
	{
		level.Init();
		mainPlayer.Init();
		controller = new Controller();
		controller.Init();
		controller.OnTouch += StartGame;
		counter.Reset();
	}

	private void StartGame()
	{
		mainPlayer.ChangeState(StateMachine.Command.Move);
		controller.OnTouch -= StartGame;
		controller.Disable();
		controller = null;
	}

	public void RestartGame()
	{
		mainMenuUI.HideAll();
		counter.Reset();
		level.Reset();
		mainPlayer.Reset();
		controller = new Controller();
		controller.Init();
		controller.OnTouch += StartGame;

	}
}
