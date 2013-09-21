using UnityEngine;

namespace FlexiCamera
{
	public class FlexiUtils
	{
		public static Vector3 EulerDifference(Vector3 x, Vector3 y)
		{
			return new Vector3(
				Mathf.DeltaAngle(x.x, y.x),
				Mathf.DeltaAngle(x.y, y.y),
				Mathf.DeltaAngle(x.z, y.z)
			);
		}
		public static Vector3 EulerSubtract(Vector3 x, Vector3 y)
		{
			return x - y;
		}

		public static float ConstrainAngle(float x){
			x = x % 360;
			//if (x < 0)
			//	x += 360;
			return Mathf.Abs(x);
		}
		
		public static Vector3 SwapYZ(Vector2 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

	}
}

