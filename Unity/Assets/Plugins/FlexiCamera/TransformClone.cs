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
	public struct TransformClone
	{
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 Scale;

		private TransformClone(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.Position = position;
			this.Rotation = rotation;
			this.Scale = scale;
		}

		public static TransformClone FromTransform(Transform t)
		{
			return new TransformClone(t.position, t.rotation, t.localScale);;
		}

		public void ApplyToTransform(Transform t){
			t.position = this.Position;
			t.rotation = this.Rotation;
			t.localScale = this.Scale;
		}

	}
}

