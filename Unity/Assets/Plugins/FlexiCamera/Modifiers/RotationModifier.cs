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

using System;
using UnityEngine;

namespace FlexiCamera.Modifiers
{
	public class RotationModifier : IModifier
	{
		Quaternion _delta;

		public RotationModifier(Quaternion delta)
		{
			_delta = delta;
		}

		#region IModifier implementation

		public TransformClone Modify(TransformClone input)
		{
			input.Rotation = _delta * input.Rotation;

			return input;
		}

		#endregion
	}
}

