using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CounterUI : MonoBehaviour
{
	private Counter counter;
	[SerializeField] private Text text;

	[Inject]
	private void Construct(Counter counter)
	{
		this.counter = counter;
		counter.OnCountChanged += ShowCount;
	}

	private void ShowCount(int count)
	{
		text.text = count.ToString();
	}

	private void OnDestroy()
	{
		counter.OnCountChanged -= ShowCount;
	}
}
