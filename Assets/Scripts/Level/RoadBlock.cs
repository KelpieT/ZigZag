using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RoadBlock
{
	public enum Side
	{
		None,
		Left,
		Right
	}
	private const float tileSpace = 0;

	private Level level;
	private RoadTile roadTilePrefab;
	private CrystalFactory crystalFactory;
	private ICrystalGenerator crystalGenerator;
	private Vector3 centerPos;
	private Vector3 size;
	private Quaternion rotation;

	private Vector3 p1;
	private Vector3 p2;
	private Vector3 p3;
	private Vector3 p4;
	private Vector3 ps;
	private Vector3 pe;

	private List<RoadTile> roadTiles = new List<RoadTile>();
	private List<Crystal> crystals = new List<Crystal>();

	public Vector3 CenterPos { get => centerPos; }
	public IEnumerable<Crystal> Crystals { get => crystals; }

	public RoadBlock(RoadTile roadTile, Level level, CrystalFactory crystalFactory)
	{
		this.level = level;
		this.roadTilePrefab = roadTile;
		this.crystalFactory = crystalFactory;
	}

	public void Set(int length, int width, Vector3 centerPos, Quaternion rotation, ref int lastIndexTile, ICrystalGenerator crystalGenerator)
	{
		this.centerPos = centerPos;
		this.rotation = rotation;
		this.crystalGenerator = crystalGenerator;

		size.x = (width * roadTilePrefab.Size.x + (width - 1) * tileSpace).ClampMin(0);
		size.z = (length * roadTilePrefab.Size.z + (length - 1) * tileSpace).ClampMin(0);
		Vector3 halfSize = size / 2;

		p1 = centerPos + rotation * new Vector3(halfSize.x, 0, -halfSize.z);

		p2 = centerPos + rotation * new Vector3(-halfSize.x, 0, -halfSize.z);

		p3 = centerPos + rotation * new Vector3(-halfSize.x, 0, halfSize.z);

		p4 = centerPos + rotation * new Vector3(halfSize.x, 0, halfSize.z);

		ps = centerPos + rotation * new Vector3(0, 0, -halfSize.z);
		pe = centerPos + rotation * new Vector3(0, 0, halfSize.z);


		for (int z = 0; z < length; z++)
		{
			for (int x = 0; x < width; x++)
			{
				Vector3 tileOffset = roadTilePrefab.Size / 2;
				Vector3 spaceOffset = new Vector3(tileSpace * (x - 1).ClampMin(0), 0, tileSpace * (length - 1).ClampMin(0));
				Vector3 tilesSize = new Vector3(roadTilePrefab.Size.x * x, 0, roadTilePrefab.Size.z * z);
				Vector3 pos = centerPos + rotation * (tilesSize + spaceOffset + tileOffset - halfSize);
				RoadTile roadTile = GameObject.Instantiate<RoadTile>(roadTilePrefab, pos, rotation);
				lastIndexTile++;
				if (crystalGenerator.CanGenerateCrystal(level.CountTilesGroupForCrystal, lastIndexTile))
				{
					Crystal crystal = crystalFactory.Create();
					crystal.transform.SetPositionAndRotation(pos, Quaternion.identity);
					crystals.Add(crystal);
				}
				roadTiles.Add(roadTile);
			}
		}
	}

	public Vector3 GetPointEnd(Side side)
	{
		switch (side)
		{
			case Side.Left:
				return p3;
			case Side.Right:
				return p4;
			default:
				throw new System.ArgumentException();
		}
	}

	public static Vector3 CenterPosByEndPos(int length, int width, Vector3 endPoint, Side sideEndPoint, Side sideToRotate, Vector3 tileSize)
	{
		Vector3 centerPosition;
		Vector3 size = Vector3.zero;
		Quaternion rot = sideToRotate == Side.Left ? Quaternion.identity : Quaternion.Euler(0, 90, 0);

		size.x = (width * tileSize.x + (width - 1) * tileSpace).ClampMin(0);
		size.z = (length * tileSize.z + (length - 1) * tileSpace).ClampMin(0);
		Vector3 halfSize = size / 2;
		if (sideEndPoint == Side.Left)
		{
			centerPosition = endPoint + rot * new Vector3(halfSize.x, 0, halfSize.z);
		}
		else
		{
			centerPosition = endPoint + rot * new Vector3(-halfSize.x, 0, halfSize.z);
		}
		return centerPosition;
	}

	public bool PointInBlock(Vector3 point)
	{
		return (GeometryUtils.PointInTriangle(p1, p2, p3, point))
			|| (GeometryUtils.PointInTriangle(p3, p4, p1, point));
	}

	public void DestroyRoadBehind(Vector3 direction, Vector3 position)
	{
		List<RoadTile> roadTilesToDestroy = new List<RoadTile>();
		List<Crystal> crystalsToDestroy = new List<Crystal>();
		foreach (var item in roadTiles)
		{
			bool behind = Behind(direction, position, item.transform.position);
			if (behind)
			{
				roadTilesToDestroy.Add(item);
			}
		}
		foreach (var item in crystals)
		{
			if (item == null)
			{
				continue;
			}
			bool behind = Behind(direction, position, item.transform.position);
			if (behind)
			{
				crystalsToDestroy.Add(item);
			}
		}
		foreach (var item in roadTilesToDestroy)
		{
			roadTiles.Remove(item);
			item.Destroy();
		}
		foreach (var item in crystalsToDestroy)
		{
			crystals.Remove(item);
			GameObject.Destroy(item.gameObject);
		}

	}

	private bool Behind(Vector3 direction, Vector3 position, Vector3 checkedPosition)
	{
		Vector3 dif = checkedPosition - position;
		Vector3 projection = Vector3.Project(dif, direction);
		bool behind = (projection.magnitude > level.DistanceDestroyTiles)
			&& (Vector3.Dot(direction.normalized, dif.normalized) < 0);
		return behind;
	}

	public void ClearRoadBlock(bool immediately)
	{
		crystals.ForEach(x =>
		{
			if (x != null)
			{
				GameObject.Destroy(x.gameObject);
			}
		});
		crystals.Clear();
		if (immediately)
		{
			roadTiles.ForEach(x => GameObject.Destroy(x.gameObject));
		}
		else
		{
			roadTiles.ForEach(x => x.Destroy());
		}
		roadTiles.Clear();

	}
}
