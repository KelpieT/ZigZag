using System.Collections.Generic;
using UnityEngine;

public class MovePlayerState : State
{
	private Controller controller;
	private Vector3 moveSpeed;
	private Level level;

	public MovePlayerState(MainPlayer player, Level level) : base(player)
	{
		this.level = level;
	}

	public override void StartState()
	{
		controller = new Controller();
		controller.Init();
		moveSpeed = new Vector3(0, 0, player.MoveSpeed);
		controller.OnTouch += ChangeDirection;
	}

	public override void UpdateState()
	{
		Vector3 position = player.transform.position + moveSpeed * Time.deltaTime;
		Quaternion rotation = Quaternion.LookRotation(moveSpeed);
		player.transform.SetPositionAndRotation(position, rotation);
		
		List<Crystal> crystals = level.GetCrystalInPoint(player.transform.position, player.PickupRadius);
		if (crystals != null && crystals.Count > 0)
		{
			crystals.ForEach(x => x.Pickup());
		}
		level.UpdateLevel(player.transform);
		bool onRoad = level.OnRoad(player.transform.position).onRoad;
		if (!onRoad)
		{
			player.ChangeState(StateMachine.Command.Dead);
			player.Dead();
		}
	}

	public override void EndState()
	{
		controller.OnTouch -= ChangeDirection;
		controller.Disable();
	}

	private void ChangeDirection()
	{
		float x = moveSpeed.x;
		float z = moveSpeed.z;
		moveSpeed.x = z;
		moveSpeed.z = x;
	}
}
