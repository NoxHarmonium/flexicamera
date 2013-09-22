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

	public class PanController : IController
	{
		protected Camera _targetCamera;
		protected RaycastFromCameraCenter _raycast;
		protected float _panFactor = 1.0f;
		protected float _startThreshold = 0.01f;
		protected bool _invert = true;
		protected Vector3 _worldDelta;
		protected float _deltaClamp = 15f;
		protected float _dampingFactor = 0.85f;
		protected bool _limited = false;
		protected float _limitStart = 1.25f;
		protected float _limitEnd = 1.5f;
		protected Vector3 _center = Vector3.zero;
		
		protected Vector3 _deltaPos;
		protected bool _pendingUpdate;


		public PanController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera);
		}
		
		public PanController(CameraProcessor parent, bool limited, float limitStart, float limitEnd)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera);
			_limited = limited;
			_limitStart = limitStart;
			_limitEnd = limitEnd;
		}

		#region IController implementation

		public void ProcessMessage(InputMessage message)
		{
			if (message.InputType != InputMessage.InputTypes.OneFingerDrag) {
				return;
			}
			if (message.MessageType == InputMessage.MessageTypes.Update || 
				message.MessageType == InputMessage.MessageTypes.Begin) {	
			
				_raycast.Invalidate();

				Vector2 delta = message.FingerDeltas[0];
				
			
				
				//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
				if (delta.magnitude > _startThreshold && _raycast.DidHit) {

					float angle = AngleMath.AngleSigned(Vector3.forward, _targetCamera.transform.forward, Vector3.up);
					_worldDelta = Quaternion.AngleAxis(angle, Vector3.up) * FlexiUtils.SwapYZ(message.NormalisedFingerDeltas[0]) * (_panFactor * _raycast.Distance) * (_invert ? -1f : 1f);
					_worldDelta = Vector3.ClampMagnitude(_worldDelta, _deltaClamp);
					
					
					float distance = Vector3.Distance(_center, _raycast.HitPoint);
					if (_limited && distance > _limitStart) {
						float overshoot = (distance - _limitStart);
						
						float factor = 1f - Mathf.Clamp((overshoot / _limitEnd), 0f, 1f);
						
						float a = Vector3.Angle(_worldDelta.normalized, -_raycast.HitPoint.normalized);
						
						if (a > 90f)
							_worldDelta *= factor;	
							
						
					}
					
					_deltaPos = _worldDelta;
					_pendingUpdate = true;		
					
					message.Use();	

				}
				
			} else {
				if (_worldDelta.magnitude > _startThreshold) {

					Vector3 mod = _worldDelta;
					_worldDelta *= _dampingFactor;
					_deltaPos = mod;
					_pendingUpdate = true;
					
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

