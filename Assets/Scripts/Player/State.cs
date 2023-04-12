public abstract class State
{
	protected MainPlayer player;

	public State(MainPlayer player)
	{
		this.player = player;
	}
	
	public abstract void StartState();
	public abstract void UpdateState();
	public abstract void EndState();
}