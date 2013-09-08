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
		protected float _pitchDegrees = -50;
		protected float _aroundDegrees = 0;
		protected float _minAngle = -89.0f;
		protected float _maxAngle = -7.5f;
		protected float _dampingFactor = 0.75f;
		protected float _deltaClamp = 15f;
		protected Vector2 _inputDelta;

		public OrbitController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera, parent.LayerMask);
			_input = new TwoFingerDragInput();

		}

		#region IController implementation

		public List<IModifier> GetModifiers()
		{
			if (_input.GestureHasReset) {
				_raycast.Invalidate();
				Debug.Log("Invalidate");
			}

			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (_input.Delta1.magnitude > _startThreshold && _raycast.DidHit) {

				_inputDelta = Vector2.ClampMagnitude(_input.Delta1, _deltaClamp);

			}

			if (_inputDelta.magnitude > _startThreshold)
			{

				TransformClone t = TransformClone.FromTransform(_targetCamera.transform);

				_aroundDegrees += _inputDelta.x;
				_pitchDegrees += _inputDelta.y;

				_inputDelta *= _dampingFactor;

				_pitchDegrees = Mathf.Clamp(_pitchDegrees, _minAngle, _maxAngle);

				float radius = Vector3.Distance(t.Position, _raycast.HitPoint);				

				Vector3 currentPos = t.Position;
				Vector3 newPos = Quaternion.Euler(_pitchDegrees, _aroundDegrees, 0) * (radius * Vector3.forward) + _raycast.HitPoint;
				Vector3 deltaPos = newPos - currentPos;

				Quaternion currentRot = t.Rotation;
				Quaternion newRot = Quaternion.LookRotation(_raycast.HitPoint - newPos);
				Quaternion deltaRot = newRot * Quaternion.Inverse(currentRot);

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

