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
		protected RaycastFromCameraCenter _raycast;
		protected TwistInput _input;
		protected float _rotateFactor = 1f;
		protected float _startThreshold = 0.01f;


		public RotateOrbitController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera, parent.LayerMask);
			_input = new TwistInput();

		}

		#region IController implementation

		public List<IModifier> GetModifiers()
		{
			_raycast.Invalidate();

			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (Mathf.Abs(_input.Factor) > _startThreshold && _raycast.DidHit) {

				TransformClone t = TransformClone.FromTransform(_targetCamera.transform);

				float delta =  _input.Factor * _rotateFactor;

				//Debug.Log("Delta: " + delta);

				Quaternion deltaRot = Quaternion.AngleAxis(delta, Vector3.up);

				Vector3 currentPos = t.Position;
				Vector3 newPos = currentPos;
				newPos = newPos - _raycast.HitPoint;
				newPos = deltaRot * newPos;
				newPos += _raycast.HitPoint;


				Vector3 deltaPos = newPos - currentPos;


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

