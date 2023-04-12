using System;
using UnityEngine;

[Serializable]
public class LevelDifficultyBlock
{
	[SerializeField] private int minWidthBlock;
	[SerializeField] private int maxWidthBlock;
	[SerializeField] private int minLengthBlock;
	[SerializeField] private int maxLengthBlock;

	public int GetWidhtBlock()
	{
		return UnityEngine.Random.Range(minWidthBlock, maxWidthBlock);
	}
	
	public int GetLenghtBlock()
	{
		return UnityEngine.Random.Range(minLengthBlock, maxLengthBlock);
	}
}
