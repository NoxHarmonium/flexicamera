using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
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
			this._raycast = new RaycastScreenPointFromCamera(parent.TargetCamera, Vector2.zero, parent.LayerMask);
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
					return;

				}
				_pendingUpdate = false;
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

