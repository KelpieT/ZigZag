using System.Collections.Generic;
using System.Linq;

public class StateMachine
{

	public enum StateEnum
	{
		Idle,
		Move,
		Dead,
	}
	public enum Command
	{
		Move,
		Dead,
		Idle,
	}

	private StateEnum currentState = StateEnum.Idle;
	private List<Transition> transitions = new List<Transition>()
	{
		// Default Transitions
		new Transition(StateEnum.Idle,StateEnum.Dead,Command.Dead),
		new Transition(StateEnum.Move,StateEnum.Dead,Command.Dead),

		new Transition(StateEnum.Idle,StateEnum.Move,Command.Move),

		new Transition(StateEnum.Move,StateEnum.Idle,Command.Idle),

	};
	private Dictionary<StateEnum, State> states = new Dictionary<StateEnum, State>();

	public StateMachine(Dictionary<StateEnum, State> states)
	{
		this.states = states;
	}

	public StateMachine(Dictionary<StateEnum, State> states, List<Transition> transitions)
	{
		this.states = states;
		this.transitions = transitions;
	}

	public StateEnum CurrentState { get => currentState; }

	StateEnum GetNextState(Command command)
	{
		var v = transitions.FirstOrDefault(x => x.command == command && x.currentState == currentState);
		if (v == null)
		{
			return StateEnum.Idle;
		}
		return v.nextState;
	}

	protected virtual State GetState(StateEnum state)
	{
		return states[state];
	}

	public State GetNextStateByCommand(Command command)
	{
		StateEnum stateEnum = GetNextState(command);
		State nextState = GetState(stateEnum);
		currentState = stateEnum;
		return nextState;
	}

	public void SetStates(Dictionary<StateEnum, State> states)
	{
		this.states = states;
	}

}

public class Transition
{
	public StateMachine.StateEnum currentState;
	public StateMachine.StateEnum nextState;
	public StateMachine.Command command;

	public Transition(StateMachine.StateEnum currentState, StateMachine.StateEnum nextState, StateMachine.Command command)
	{
		this.currentState = currentState;
		this.nextState = nextState;
		this.command = command;
	}
}

