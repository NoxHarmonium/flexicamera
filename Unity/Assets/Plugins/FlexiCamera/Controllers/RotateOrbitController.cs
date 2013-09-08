using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
	using Modifiers;

	public class RotateOrbitController : IController
	{
		protected Camera _targetCamera;
		protected RaycastScreenPointFromCamera _raycast;
		protected TwistInput _input;
		protected float _rotateFactor = 1f;
		protected float _startThreshold = 0.01f;
		protected float _dampingFactor = 0.85f;
		protected float _rotateDelta;


		public RotateOrbitController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastScreenPointFromCamera(parent.TargetCamera, Vector2.zero, parent.LayerMask);
			_input = new TwistInput();

		}

		#region IController implementation

		public List<IModifier> GetModifiers()
		{
			_raycast.CastPoint = (_input.Position1 + _input.Position2) / 2.0f;
			_raycast.Invalidate();



			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (Mathf.Abs(_input.Factor) > _startThreshold && _raycast.DidHit) {
				_rotateDelta = _input.Factor;
			}

			if (Math.Abs(_rotateDelta) > _startThreshold) {
				float delta =  _rotateDelta * _rotateFactor;
				TransformClone t = TransformClone.FromTransform(_targetCamera.transform);
				//Debug.Log("Delta: " + delta);

				Quaternion deltaRot = Quaternion.AngleAxis(delta, Vector3.up);

				Vector3 currentPos = t.Position;
				Vector3 newPos = currentPos;
				newPos = newPos - _raycast.HitPoint;
				newPos = deltaRot * newPos;
				newPos += _raycast.HitPoint;


				Vector3 deltaPos = newPos - currentPos;

				_rotateDelta *= _dampingFactor;

				return new List<IModifier>() {
					new PositionModifier(deltaPos),
					new RotationModifier(deltaRot)
				};

			}
			return new List<IModifier>();
		}



		#endregion
	}
}

