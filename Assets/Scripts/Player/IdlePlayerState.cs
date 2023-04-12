using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdlePlayerState : State
{
	public IdlePlayerState(MainPlayer player) : base(player)
	{
	}

	public override void StartState()
	{
		player.transform.position = Vector3.zero;
		player.Alive();
	}

	public override void UpdateState()
	{
	}
	
	public override void EndState()
	{
	}
}
