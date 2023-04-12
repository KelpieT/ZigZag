public class MainMenu
{
	private MainEnterPoint mainEnterPoint;

	public MainMenu(MainEnterPoint mainEnterPoint)
	{
		this.mainEnterPoint = mainEnterPoint;
	}

	public void Restart()
	{
		mainEnterPoint.RestartGame();
	}
}
