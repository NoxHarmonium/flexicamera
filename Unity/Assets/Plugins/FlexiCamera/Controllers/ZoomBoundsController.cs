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

	public class ZoomBoundsController : IController
	{
		protected Camera _targetCamera;
		protected RaycastFromCameraCenter _raycast;
		protected float _minDistance = 20f;
		protected float _maxDistance = 100f;
		protected float _bounceTime = 0.5f;
		protected bool _enabled;

		public ZoomBoundsController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera);

		}

		#region IController implementation

		public void ProcessMessage(InputMessage message)
		{
			_enabled &= !(message.InputType == InputMessage.InputTypes.TwoFingerPinch && message.MessageType == InputMessage.MessageTypes.Update);
		}
		
		public List<IModifier> GetModifiers()
		{
			_raycast.Invalidate();

			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (_raycast.DidHit && _enabled) {
				Vector3 worldDelta = default(Vector3);
				Quaternion deltaRot = default(Quaternion);
				TransformClone t = TransformClone.FromTransform(_targetCamera.transform);
				bool active = false;

				if (_raycast.Distance < _minDistance || _raycast.Distance > _maxDistance) {
					float overshoot = 0f;
					if (_raycast.Distance < _minDistance) {
						overshoot = _raycast.Distance - _minDistance;
					} else if (_raycast.Distance > _maxDistance) {
						overshoot = _raycast.Distance - _maxDistance;
					}

					Vector3 direction = (_raycast.HitPoint - t.Position).normalized;
					worldDelta = direction * overshoot * Time.deltaTime * (1f / _bounceTime);


					Quaternion newRot = Quaternion.LookRotation(direction);
					Quaternion currentRot = t.Rotation;
					deltaRot = newRot * Quaternion.Inverse(currentRot);
					active = true;
				}
				
				_enabled = true;
				
				if (!active)
					return new List<IModifier>();

				return new List<IModifier>() {
					new PositionModifier(worldDelta),
					new RotationModifier(deltaRot)
				};
			}


			return new List<IModifier>();
		}



		#endregion
	}
}

