using System;
using UnityEngine.InputSystem;

public class Controller
{
	public event Action OnTouch;
	private bool isTuched = false;
	private SimpleMouseControl simpleMouseControl;

	public void Init()
	{
		simpleMouseControl = new SimpleMouseControl();
		simpleMouseControl.Enable();
		simpleMouseControl.Game.Down.performed += StartDrag;
		simpleMouseControl.Game.Down.canceled += EndDrag;
	}

	private void StartDrag(InputAction.CallbackContext contex)
	{
		isTuched = true;
		OnTouch?.Invoke();
	}

	private void EndDrag(InputAction.CallbackContext contex)
	{
		isTuched = false;
	}

	public void Disable()
	{
		simpleMouseControl.Game.Down.performed -= StartDrag;
		simpleMouseControl.Game.Down.canceled -= EndDrag;
	}
}
