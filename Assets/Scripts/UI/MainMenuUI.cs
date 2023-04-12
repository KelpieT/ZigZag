using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuUI : MonoBehaviour
{
	[SerializeField] private Button restartButton;
	private MainMenu mainMenu;
	private MainPlayer mainPlayer;

	[Inject]
	private void Construct(MainMenu mainMenu, MainPlayer mainPlayer)
	{
		this.mainMenu = mainMenu;
		this.mainPlayer = mainPlayer;
		mainPlayer.OnDead += ShowRestart;
	}

	public void ShowRestart()
	{
		restartButton.onClick.AddListener(mainMenu.Restart);
		restartButton.gameObject.SetActive(true);
	}

	public void HideAll()
	{
		restartButton.onClick.RemoveListener(mainMenu.Restart);
		restartButton.gameObject.SetActive(false);

	}

	private void OnDestroy()
	{
		mainPlayer.OnDead -= ShowRestart;
	}
}
