using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/LevelDifficulty")]
public class LevelDifficulty : ScriptableObject
{
	[SerializeField] private List<LevelDifficultyBlock> blocks;

	public LevelDifficultyBlock GetNextLevelDifficultyBlock()
	{
		return blocks.GetRandomFromList();
	}

	private void OnValidate()
	{
		if (blocks.Count == 0)
		{
			Debug.LogError("empty list blocks");
		}
	}
}

