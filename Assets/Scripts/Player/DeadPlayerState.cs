using UnityEngine;

public class DeadPlayerState : State
{
	public DeadPlayerState(MainPlayer player) : base(player)
	{
	}

	public override void StartState()
	{
	}

	public override void UpdateState()
	{
		if (player.transform.position.y <= player.DeadHeight)
		{
			return;
		}
		player.transform.position += Vector3.down * player.FallSpeed * Time.deltaTime;


	}
	
	public override void EndState()
	{
	}
}
