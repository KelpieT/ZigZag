using UnityEngine;
using Zenject;

public class Crystal : MonoBehaviour
{
	private Counter counter;

	[Inject]
	private void Construct(Counter counter)
	{
		this.counter = counter;
	}

	public void Pickup()
	{
		counter.Add(1);
		Destroy(gameObject);
	}
}
