using UnityEngine;

public class RandomCristalGenerator : ICrystalGenerator
{
	private int lastGeneratedTile = -1;

	public bool CanGenerateCrystal(int countTilesGroup, int totalIndexTile)
	{
		int lastGeneratedGroup = lastGeneratedTile / countTilesGroup;
		int group = totalIndexTile / countTilesGroup;
		bool canGenerate;
		if (lastGeneratedTile == -1)
		{
			canGenerate = CanGenerateCrystalRandom(countTilesGroup, totalIndexTile);
			if (canGenerate)
			{
				lastGeneratedTile = totalIndexTile;
			}
			return canGenerate;
		}
		if (lastGeneratedGroup == group - 1)
		{
			canGenerate = CanGenerateCrystalRandom(countTilesGroup, totalIndexTile);
			if (canGenerate)
			{
				lastGeneratedTile = totalIndexTile;
			}
			return canGenerate;
		}
		else
		{
			return false;
		}
	}

	private bool CanGenerateCrystalRandom(int countTilesGroup, int totalIndexTile)
	{
		int maxRandom = countTilesGroup - totalIndexTile % countTilesGroup;
		int random = Random.Range(0, maxRandom);
		if (random == 0)
		{
			return true;
		}
		return false;

	}
}
