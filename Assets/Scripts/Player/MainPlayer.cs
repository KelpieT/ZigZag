using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainPlayer : MonoBehaviour
{
	public event Action OnDead;
	public event Action OnAlive;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float pickupRadius;
	[SerializeField] private float deadHeight = -100f;
	[SerializeField] private float fallSpeed = 5f;
	private StateMachine stateMachine;
	private State currentState;
	private Level level;

	public float MoveSpeed { get => moveSpeed; }
	public float PickupRadius { get => pickupRadius; }
	public float DeadHeight { get => deadHeight; }
	public float FallSpeed { get => fallSpeed; }

	[Inject]
	private void Constuct(Level level)
	{
		this.level = level;
	}

	public void Init()
	{
		Dictionary<StateMachine.StateEnum, State> states = new Dictionary<StateMachine.StateEnum, State>();
		states.Add(StateMachine.StateEnum.Idle, new IdlePlayerState(this));
		states.Add(StateMachine.StateEnum.Move, new MovePlayerState(this, level));
		states.Add(StateMachine.StateEnum.Dead, new DeadPlayerState(this));

		stateMachine = new StateMachine(states);
		ChangeState(StateMachine.Command.Idle);
	}

	private void Update()
	{
		currentState?.UpdateState();
	}

	public void ChangeState(StateMachine.Command command)
	{
		currentState?.EndState();
		currentState = stateMachine.GetNextStateByCommand(command);
		currentState?.StartState();
	}

	public void Dead()
	{
		OnDead?.Invoke();
	}

	public void Alive()
	{
		OnAlive?.Invoke();
	}

	public void Reset()
	{
		ChangeState(StateMachine.Command.Idle);
	}
}
