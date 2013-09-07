using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
	using Modifiers;

	public class OrbitController : IController
	{
		protected Camera _targetCamera;
		protected RaycastFromCameraCenter _raycast;
		protected TwoFingerDragInput _input;
		protected float _rotateYFactor = 1.0f;
		protected float _rotateXFactor = 1.0f;
		protected float _startThreshold = 0.01f;
		protected float pitchDegrees = -10;
		protected float aroundDegrees = 0;

		public OrbitController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera, parent.LayerMask);
			_input = new TwoFingerDragInput();

		}

		#region IController implementation

		public List<IModifier> GetModifiers()
		{
			_raycast.Invalidate();

			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (_input.Delta.magnitude > _startThreshold && _raycast.DidHit) {

				TransformClone t = TransformClone.FromTransform(_targetCamera.transform);

				aroundDegrees += _input.Delta.x;
				pitchDegrees += _input.Delta.y;
				float radius = Vector3.Distance(t.Position, _raycast.HitPoint);

				Vector3 currentPos = t.Position;
				Vector3 newPos = Quaternion.Euler(pitchDegrees, aroundDegrees, 0) * (radius * Vector3.forward) + _raycast.HitPoint;
				Vector3 deltaPos = newPos - currentPos;

				Quaternion currentRot = t.Rotation;
				Quaternion newRot = Quaternion.LookRotation(_raycast.HitPoint - currentPos, Vector3.up);
				Quaternion deltaRot = newRot * Quaternion.Inverse(currentRot);

				return new List<IModifier>() {
					new PositionModifier(deltaPos),
					//new RotationModifier(deltaRot)
				};
			}
			return new List<IModifier>();
		}



		#endregion
	}
}

