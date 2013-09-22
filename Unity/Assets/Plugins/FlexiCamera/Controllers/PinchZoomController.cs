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

	public class PinchZoomController : IController
	{
		protected Camera _targetCamera;
		protected RaycastScreenPointFromCamera _raycast;
		protected float _zoomFactor = 0.1f;
		protected float _startThreshold = 0.01f;
		protected float _deltaClamp = 20f;
		protected Vector3 _worldDelta;
		
		protected bool _limited = false;
		protected float _minHeight = 8f;
		protected float _maxHeight = 58f;
		protected float _tolerance = 2f;
		
		protected Vector3 _deltaPos;
		protected bool _pendingUpdate;


		public PinchZoomController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastScreenPointFromCamera(parent.TargetCamera, Vector2.zero);
		}
		
		public PinchZoomController(CameraProcessor parent, bool limited, float minHeight, float maxHeight, float tolerance)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastScreenPointFromCamera(parent.TargetCamera, Vector2.zero);
			this._limited = limited;
			this._minHeight = minHeight;
			this._maxHeight = maxHeight;
			this._tolerance = tolerance;
		}


		#region IController implementation

		public void ProcessMessage(InputMessage message)
		{
			if (message.InputType != InputMessage.InputTypes.TwoFingerPinch) {
				return;
			}
			
			if (message.MessageType == InputMessage.MessageTypes.Update) {	
			
				_raycast.CastPoint = (message.FingerPositions[0] + message.FingerPositions[1]) / 2.0f;
				_raycast.Invalidate();

				//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
				if (Mathf.Abs(message.GestureData[0]) > _startThreshold && _raycast.DidHit) {

					TransformClone t = TransformClone.FromTransform(_targetCamera.transform);
					_worldDelta = (_raycast.HitPoint - t.Position).normalized * Mathf.Clamp(message.GestureData[0], -_deltaClamp, _deltaClamp) * _zoomFactor;
				}
				
				float distance = Vector3.Distance(_targetCamera.transform.position, _raycast.HitPoint);
				if (_limited && ( distance < _minHeight || distance > _maxHeight)) {
					float overshoot;
					float a;
					if (distance < _minHeight)
					{
						overshoot = _minHeight - distance;
						 a = Vector3.Angle(_worldDelta.normalized, -_targetCamera.transform.forward);
					}
					else
					{
						overshoot = distance - _maxHeight;		
						 a = Vector3.Angle(_worldDelta.normalized, _targetCamera.transform.forward);		
					}
					
					float factor = 1f - Mathf.Clamp((overshoot / _tolerance), 0f, 1f);
					
					if (a > 90f)
						_worldDelta *= factor;	
							
						
				}

				if (_worldDelta.magnitude > 0.01f) {

					Vector3 mod = _worldDelta;
					_worldDelta *= 0.85f;

					_deltaPos = mod;
					_pendingUpdate = true;
					
					message.Use();
					return;
				}
			}
		}
		
		public List<IModifier> GetModifiers()
		{
			if (_pendingUpdate ) {
			
				_pendingUpdate = false;

				return new List<IModifier>() {
					new PositionModifier(_deltaPos)
				};
			}
			return new List<IModifier>();
		}



		#endregion
	}
}

