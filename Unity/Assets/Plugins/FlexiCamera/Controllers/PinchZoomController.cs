using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
	using Modifiers;

	public class PinchZoomController : IController
	{
		protected Camera _targetCamera;
		protected RaycastScreenPointFromCamera _raycast;
		protected PinchInput _input;
		protected float _zoomFactor = 0.1f;
		protected float _startThreshold = 0.01f;
		protected float _deltaClamp = 20f;
		protected Vector3 _worldDelta;


		public PinchZoomController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastScreenPointFromCamera(parent.TargetCamera, Vector2.zero, parent.LayerMask);
			_input = new PinchInput();

		}

		#region IController implementation

		public List<IModifier> GetModifiers()
		{
			_raycast.CastPoint = (_input.Position1 + _input.Position2) / 2.0f;
			_raycast.Invalidate();

			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (Mathf.Abs(_input.Factor) > _startThreshold && _raycast.DidHit) {

				TransformClone t = TransformClone.FromTransform(_targetCamera.transform);
				_worldDelta = (_raycast.HitPoint - t.Position).normalized * Mathf.Clamp(_input.Factor, -_deltaClamp, _deltaClamp) * _zoomFactor;
			}

			if (_worldDelta.magnitude > 0.01f) {

				Vector3 mod = _worldDelta;
				_worldDelta *= 0.85f;

				return new List<IModifier>() {
					new PositionModifier(mod)
				};
			}

			return new List<IModifier>();
		}



		#endregion
	}
}

