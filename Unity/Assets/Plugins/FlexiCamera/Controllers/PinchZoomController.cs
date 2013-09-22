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
		
		protected Vector3 _deltaPos;
		protected bool _pendingUpdate;


		public PinchZoomController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastScreenPointFromCamera(parent.TargetCamera, Vector2.zero);
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

