using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
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
		
		protected Vector3 _deltaPos;
		protected bool _pendingUpdate;


		public PanController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera, parent.LayerMask);
		}

		#region IController implementation

		public void ProcessMessage(InputMessage message)
		{
			
			if (message.InputType != InputMessage.InputTypes.OneFingerDrag) {
				return;
			}
			
			if (message.MessageType == InputMessage.MessageTypes.Update) {	
			
				_raycast.Invalidate();

				Vector2 delta = message.FingerDeltas[0];
				
				//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
				if (delta.magnitude > _startThreshold && _raycast.DidHit) {

					float angle = AngleMath.AngleSigned(Vector3.forward, _targetCamera.transform.forward, Vector3.up);
					_worldDelta = Quaternion.AngleAxis(angle, Vector3.up) * message.NormalisedFingerDeltas[0] * (_panFactor * _raycast.Distance) * (_invert ? -1f : 1f);
					_worldDelta = Vector3.ClampMagnitude(_worldDelta, _deltaClamp);

				}
				if (_worldDelta.magnitude > _startThreshold) {

					Vector3 mod = _worldDelta;
					_worldDelta *= _dampingFactor;
					_deltaPos = mod;
					_pendingUpdate = true;
					return;
				}
			}
			
			_pendingUpdate = false;
		
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

