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

	public class OrbitController : IController
	{
		protected Camera _targetCamera;
		protected RaycastFromCameraCenter _raycast;
		protected float _rotateYFactor = 1.0f;
		protected float _rotateXFactor = 1.0f;
		protected float _startThreshold = 0.01f;
		protected float _pitchDegrees = -50;
		protected float _aroundDegrees = 0;
		protected float _minAngle = -89.0f;
		protected float _maxAngle = -7.5f;
		protected float _dampingFactor = 0.9f;
		protected float _deltaClamp = 15f;
		protected Vector2 _inputDelta;
		protected bool _pendingUpdate;
		protected Vector3 _deltaPos;
		protected Quaternion _deltaRot;

		public OrbitController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera);

		}
		#region IController implementation
		public void ProcessMessage(InputMessage message)
		{
			bool usingInput = false;
			
			if (message.InputType == InputMessage.InputTypes.TwoFingerDrag) {
			
				usingInput = true;
				
				if (message.MessageType == InputMessage.MessageTypes.Begin) {	
			
					if (_pitchDegrees < _maxAngle) {
						_raycast.Invalidate();

						// TODO: Less ugly way to determine this
						Vector3 angles = Vector3.Scale(Quaternion.LookRotation(_targetCamera.transform.forward).eulerAngles - new Vector3(0f, 180f, 0f), new Vector3(-1f, 1f, 1f));

						_pitchDegrees = angles.x;
						_aroundDegrees = angles.y;

					}
				}
			
				if (message.MessageType == InputMessage.MessageTypes.Update) {
					Vector2 fingerDelta = message.FingerDeltas[0];

					//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
					if (fingerDelta.magnitude > _startThreshold && _raycast.DidHit) {

						_inputDelta = Vector2.ClampMagnitude(fingerDelta, _deltaClamp);
						message.Use();
					}

					
				}
			}
			
			
			
		}

		public List<IModifier> GetModifiers()
		{
			if (_inputDelta.magnitude > _startThreshold) {
				
				if(!this._pendingUpdate)
				{
					_raycast.Invalidate();
					this._pendingUpdate = true;
				}
				
				TransformClone t = TransformClone.FromTransform(_targetCamera.transform);

				_aroundDegrees += _inputDelta.x;
				_pitchDegrees += _inputDelta.y;

				_inputDelta *= _dampingFactor;

				if (_pitchDegrees > _maxAngle) {
					return new List<IModifier>();
				}

				_pitchDegrees = Mathf.Clamp(_pitchDegrees, _minAngle, _maxAngle);

				float radius = Vector3.Distance(t.Position, _raycast.HitPoint);				

				Vector3 currentPos = t.Position;
				Vector3 newPos = Quaternion.Euler(_pitchDegrees, _aroundDegrees, 0) * (radius * Vector3.forward) + _raycast.HitPoint;
				_deltaPos = newPos - currentPos;

				Quaternion currentRot = t.Rotation;
				Quaternion newRot = Quaternion.LookRotation(_raycast.HitPoint - newPos);
				_deltaRot = newRot * Quaternion.Inverse(currentRot);
				_pendingUpdate = true;
				
			}
			
			if (this._pendingUpdate) {
				_pendingUpdate = false;
				return new List<IModifier>() {
					new PositionModifier(_deltaPos),
					new RotationModifier(_deltaRot)
				};
			}
			return new List<IModifier>();
		}
		#endregion
	}
}

