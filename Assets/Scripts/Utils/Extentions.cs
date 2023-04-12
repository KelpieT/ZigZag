using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
	public static float ClampMin(this float value, float minValue = 0)
	{
		if (value < minValue)
		{
			return minValue;
		}
		else
		{
			return value;
		}
	}

	public static int ClampMin(this int value, int minValue = 0)
	{
		if (value < minValue)
		{
			return minValue;
		}
		else
		{
			return value;
		}
	}

	public static T GetRandomFromList<T>(this IList<T> list)
	{
		return list[Random.Range(0, list.Count)];
	}
}
