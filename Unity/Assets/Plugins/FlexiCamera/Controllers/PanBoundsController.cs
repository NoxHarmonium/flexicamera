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
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Modifiers;
	using InputAdapters;

	public class PanBoundsController : IController
	{
		protected Camera _targetCamera;
		protected RaycastFromCameraCenter _raycast;
		protected float _maxRadius = 1.25f;
		protected float _bounceTime = 1f;
		protected Vector3 _center = Vector3.zero;
		protected float _maxAngle = -7.5f;
		protected bool _enabled = true;

		public PanBoundsController(CameraProcessor parent, float maxRadius)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera);
			this._maxRadius = maxRadius;

		}

		#region IController implementation

		public void ProcessMessage(InputMessage message)
		{
			_enabled &= !(message.InputType == InputMessage.InputTypes.OneFingerDrag && message.MessageType == InputMessage.MessageTypes.Update);
		
		}
		
		public List<IModifier> GetModifiers()
		{
			if (!_enabled)
			{
				_enabled = true;
				return new List<IModifier>();
			}
			
			_raycast.Invalidate();

			// TODO: Less ugly way to determine this
			Vector3 angles = Vector3.Scale(Quaternion.LookRotation(_targetCamera.transform.forward).eulerAngles - new Vector3(0f, 180f, 0f), new Vector3(-1f, 1f, 1f));

			float pitchDegrees = angles.x;


			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (_raycast.DidHit  && pitchDegrees < _maxAngle) {
				bool active = false;
				Vector3 worldDelta = Vector3.zero;
				float distance = Vector3.Distance(_center, _raycast.HitPoint);

				if (distance > _maxRadius) {
					float overshoot = distance - _maxRadius;

					Vector3 direction = (-_raycast.HitPoint).normalized;
					worldDelta = direction * Mathf.Pow(overshoot,2.0f) * Time.deltaTime * (1f / _bounceTime);

					active = true;
				}

				if (!active) {
					_enabled = true;
					return new List<IModifier>();
				}
	
				_enabled = true;
				return new List<IModifier>() {
					new PositionModifier(worldDelta),
				};
			}


			return new List<IModifier>();
		}



		#endregion
	}
}

