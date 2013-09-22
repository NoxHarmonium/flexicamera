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

	public class RotateOrbitController : IController
	{
		protected Camera _targetCamera;
		protected RaycastScreenPointFromCamera _raycast;
		protected float _rotateFactor = 1f;
		protected float _startThreshold = 0.01f;
		protected float _dampingFactor = 0.85f;
		protected float _rotateDelta;

		protected Vector3 _deltaPos;
		protected Quaternion _deltaRot;
		protected bool _pendingUpdate;

		public RotateOrbitController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastScreenPointFromCamera(parent.TargetCamera, Vector2.zero);
		}

		#region IController implementation

		public void ProcessMessage(InputMessage message)
		{
			if (message.InputType != InputMessage.InputTypes.TwoFingerTwist) {
				return;
			}
			
			if (message.MessageType == InputMessage.MessageTypes.Update) {	
				_raycast.CastPoint = (message.FingerPositions[0] + message.FingerPositions[1]) / 2.0f;
				_raycast.Invalidate();

				//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
				if (Mathf.Abs(message.GestureData[0]) > _startThreshold && _raycast.DidHit) {
					_rotateDelta = message.GestureData[0];
				}

				if (Math.Abs(_rotateDelta) > _startThreshold) {
					float delta = _rotateDelta * _rotateFactor;
					TransformClone t = TransformClone.FromTransform(_targetCamera.transform);
					//Debug.Log("Delta: " + delta);

					_deltaRot = Quaternion.AngleAxis(delta, Vector3.up);

					Vector3 currentPos = t.Position;
					Vector3 newPos = currentPos;
					newPos = newPos - _raycast.HitPoint;
					newPos = _deltaRot * newPos;
					newPos += _raycast.HitPoint;


					_deltaPos = newPos - currentPos;

					_rotateDelta *= _dampingFactor;
					_pendingUpdate = true;
					
					message.Use();
					return;

				}
			}
		
		}
		
		public List<IModifier> GetModifiers()
		{
			if (_pendingUpdate) {
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

