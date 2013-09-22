/* ========================================================================
 * FlexiCamera for Unity3D: 
 * https://github.com/NoxHarmonium/flexicamera 
 * <!!filename!!>
 * ========================================================================
 * Copyright Sean Dawson 2013 
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ======================================================================== */

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

