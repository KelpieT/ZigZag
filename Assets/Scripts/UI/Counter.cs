using System;

public class Counter
{
	public Action<int> OnCountChanged;
	private int count;

	public int Count { get => count; }

	public void Reset()
	{
		count = 0;
		OnCountChanged?.Invoke(count);
	}

	public void Add(int add)
	{
		count += add;
		OnCountChanged?.Invoke(count);
	}
}
