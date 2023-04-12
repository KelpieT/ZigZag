using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class Level : MonoBehaviour
{
	[SerializeField] private Vector3 levelZone;
	[SerializeField] private float distanceDestroyTiles;
	[SerializeField] private int countTilesGroupForCrystal;
	[SerializeField] private bool randomGenerateCrystals = false;
	
	private RoadTile roadTile;
	private LevelDifficulty levelDifficulty;
	private RoadBlockFactory roadBlockFactory;
	private Vector3 levelZoneOffset;
	private ICrystalGenerator crystalGenerator;
	private List<RoadBlock> roadBlocks = new List<RoadBlock>();
	private RoadBlock currentRoadBlock;
	private RoadBlock lastRoadBlock;
	private int lastTileIndex;
	private RoadBlock.Side side = RoadBlock.Side.Right;

	public float DistanceDestroyTiles { get => distanceDestroyTiles; }
	public int CountTilesGroupForCrystal { get => countTilesGroupForCrystal; }

	[Inject]
	private void Constuct(RoadTile roadTile, LevelDifficulty levelDifficulty, RoadBlockFactory testFactory)
	{
		this.roadTile = roadTile;
		this.levelDifficulty = levelDifficulty;
		this.roadBlockFactory = testFactory;
	}

	public void Init()
	{
		if (randomGenerateCrystals)
		{
			crystalGenerator = new RandomCristalGenerator();
		}
		else
		{
			crystalGenerator = new SimpleCristalGenerator();
		}
		Generate();
	}

	public void UpdateLevel(Transform player)
	{

		levelZoneOffset = player.position;
		var onRoadResult = OnRoad(player.position);
		if (onRoadResult.onRoad)
		{
			if (!object.ReferenceEquals(currentRoadBlock, onRoadResult.roadBlock))
			{
				lastRoadBlock = currentRoadBlock;
			}
			currentRoadBlock = onRoadResult.roadBlock;
		}
		currentRoadBlock?.DestroyRoadBehind(player.forward, player.position);
		lastRoadBlock?.DestroyRoadBehind(player.forward, player.position);
		GenerateLevel();
		ClearBlocks();
	}

	public void OnDrawGizmos()
	{
#if UNITY_EDITOR
		Gizmos.color = new Color(0, 0.2f, 1, 0.5f);
		Gizmos.DrawCube(transform.position + levelZoneOffset, levelZone);
#endif
	}

	public (bool onRoad, RoadBlock roadBlock) OnRoad(Vector3 position)
	{
		foreach (var item in roadBlocks)
		{
			if (item.PointInBlock(position))
			{
				return (true, item);
			}
		}
		return (false, null);
	}

	private void Generate()
	{
		GenerateFirstBlock();
		while (GenerateLevel()) ;

	}

	private void GenerateFirstBlock()
	{
		RoadBlock firstRoadBlock = roadBlockFactory.Create();// new RoadBlock(roadTile, this, crystalPrefab, );
		firstRoadBlock.Set(3, 3, Vector3.zero, Quaternion.identity, ref lastTileIndex, new EmptyCrystalGenerator());
		roadBlocks.Add(firstRoadBlock);
	}

	private bool GenerateLevel()
	{
		if (roadBlocks == null || roadBlocks.Count == 0)
		{
			Debug.LogError("Empty roadBlocks");
			return false;
		}
		Vector3 blockPosition = Vector3.zero;
		LevelDifficultyBlock levelDifficultyBlock = levelDifficulty.GetNextLevelDifficultyBlock();
		int width = levelDifficultyBlock.GetWidhtBlock();
		int length = levelDifficultyBlock.GetLenghtBlock();
		Vector3 endPoint = roadBlocks.Last().GetPointEnd(side);
		RoadBlock.Side sideEndPoint = side == RoadBlock.Side.Left ? RoadBlock.Side.Right : RoadBlock.Side.Left;
		blockPosition = RoadBlock.CenterPosByEndPos(length, width, endPoint, sideEndPoint, side, roadTile.Size);
		if (!InLevelArea(blockPosition))
		{
			return false;
		}
		RoadBlock roadBlock = roadBlockFactory.Create();
		Quaternion rotation = side == RoadBlock.Side.Left ? Quaternion.identity : Quaternion.Euler(0, 90, 0);
		roadBlock.Set(length, width, blockPosition, rotation, ref lastTileIndex, crystalGenerator);
		roadBlocks.Add(roadBlock);
		side = side == RoadBlock.Side.Left ? RoadBlock.Side.Right : RoadBlock.Side.Left;
		return true;
	}

	public bool InLevelArea(Vector3 position)
	{
		Vector3 min = levelZoneOffset - levelZone / 2;
		Vector3 max = levelZoneOffset + levelZone / 2;
		return !((position.x < min.x) || (position.z < min.z) || (position.x > max.x) || (position.z > max.z));
	}

	public List<Crystal> GetCrystalInPoint(Vector3 position, float radius)
	{
		List<Crystal> crystals = new List<Crystal>();
		foreach (var item in roadBlocks)
		{
			var inRadius = item.Crystals.Where(x => (x != null) && ((x.transform.position - position).magnitude < radius));
			crystals.AddRange(inRadius);
		}
		return crystals;
	}

	private void ClearBlocks()
	{
		List<RoadBlock> toDestroy = new List<RoadBlock>();
		foreach (var item in roadBlocks)
		{
			if (!InLevelArea(item.CenterPos))
			{
				toDestroy.Add(item);
			}
		}
		foreach (var item in toDestroy)
		{
			roadBlocks.Remove(item);
			item.ClearRoadBlock(false);
		}
	}

	public void Reset()
	{
		roadBlocks.ForEach(x => x.ClearRoadBlock(true));
		roadBlocks.Clear();
		currentRoadBlock = null;
		lastRoadBlock = null;
		lastTileIndex = 0;
		side = RoadBlock.Side.Right;
		Init();
	}
}
