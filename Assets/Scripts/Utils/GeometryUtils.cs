using UnityEngine;

public static class GeometryUtils
{
	private const float OFFSET = 0.0001f;

	public static bool PointInTriangle(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
	{
		/*
		   B
		D / \
		 A___C
		   E
		*/
		Vector3 D = B - A;
		
		if (Mathf.Approximately(A.z, C.z))
		{
			C.z += OFFSET;
		}

		Vector3 E = C - A;

		//Weight of D
		float w1 = 0;

		//Weight of E  
		float w2 = 0;

		w1 = (E.x * (A.z - P.z) + E.z * (P.x - A.x)) / (D.x * E.z - D.z * E.x);
		w2 = (P.z - A.z - w1 * D.z) / E.z;

		bool pointInTriangle = (w1 >= 0) && (w2 >= 0) && ((w1 + w2) <= (1 + OFFSET));

		return pointInTriangle;
	}
}
