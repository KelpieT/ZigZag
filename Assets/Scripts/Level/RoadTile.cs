using System.Collections;
using UnityEngine;

public class RoadTile : MonoBehaviour
{
	[SerializeField] private Vector3 size;
	[SerializeField] private float destroyHeight = -10;
	[SerializeField] private float failSpeed = 5;

	public Vector3 Size { get => size; }

	public void Destroy()
	{
		StartCoroutine(DestroyEffect());
	}

	private IEnumerator DestroyEffect()
	{
		while (transform.position.y > destroyHeight)
		{
			transform.position += Vector3.down * failSpeed * Time.deltaTime;
			yield return null;
		}
		Destroy(gameObject);

	}
}
