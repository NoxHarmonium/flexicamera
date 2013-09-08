using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
	using Modifiers;

	public class PanController : IController
	{
		protected Camera _targetCamera;
		protected RaycastFromCameraCenter _raycast;
		protected DragInput _input;
		protected float _panFactor = 1.0f;
		protected float _startThreshold = 0.01f;
		protected bool _invert = true;
		protected Vector3 _worldDelta;
		protected float _deltaClamp = 15f;
		protected float _dampingFactor = 0.85f;


		public PanController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera, parent.LayerMask);
			_input = new DragInput();

		}

		#region IController implementation

		public List<IModifier> GetModifiers()
		{
			_raycast.Invalidate();


			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (_input.Delta1.magnitude > _startThreshold && _raycast.DidHit) {

				float angle = AngleMath.AngleSigned(Vector3.forward, _targetCamera.transform.forward, Vector3.up);

			
				_worldDelta = Quaternion.AngleAxis(angle, Vector3.up) * _input.NormalisedDelta1 * (_panFactor * _raycast.Distance) * (_invert ? -1f : 1f);
				_worldDelta = Vector3.ClampMagnitude(_worldDelta, _deltaClamp);

			}
			if (_worldDelta.magnitude > _startThreshold  ) {

				Vector3 mod = _worldDelta;
				_worldDelta *= _dampingFactor;

				return new List<IModifier>() {
					new PositionModifier(mod)
				};
			}
			return new List<IModifier>();
		}



		#endregion
	}
}

