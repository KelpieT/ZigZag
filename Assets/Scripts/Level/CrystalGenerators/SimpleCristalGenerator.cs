public class SimpleCristalGenerator : ICrystalGenerator
{
	private int lastGeneratedTile = -1;
	
	public bool CanGenerateCrystal(int countTilesGroup, int totalIndexTile)
	{
		int lastGeneratedGroup = lastGeneratedTile / countTilesGroup;
		int group = totalIndexTile / countTilesGroup;
		if (lastGeneratedTile == -1)
		{
			lastGeneratedTile = totalIndexTile;
			return true;
		}
		if (lastGeneratedGroup == group - 1)
		{
			lastGeneratedTile = totalIndexTile;
			return true;
		}
		else
		{
			return false;
		}
	}
}
