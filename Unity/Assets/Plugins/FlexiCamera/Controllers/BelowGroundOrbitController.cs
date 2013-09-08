using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
	using Modifiers;

	public class BelowGroundOrbitController : IController
	{
		protected Camera _targetCamera;
		protected RaycastFromCameraCenter _raycast;
		protected TwoFingerDragInput _input;
		protected float _rotateYFactor = 1.0f;
		protected float _rotateXFactor = 1.0f;
		protected float _startThreshold = 0.01f;
		protected float _pitchDegrees = -50;
		protected float _aroundDegrees = 0;
		protected float _minAngle = -7.5f;
		protected float _maxAngle = -80f;
		protected float _crossoverAngle = 0f;
		protected float _dampingFactor = 0.75f;
		protected float _deltaClamp = 15f;
		protected float _tiltUpFactor = 0.05f;
		protected Vector2 _inputDelta;

		public BelowGroundOrbitController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera, parent.LayerMask);
			_input = new TwoFingerDragInput();

		}

		#region IController implementation

		public List<IModifier> GetModifiers()
		{
			if (_input.GestureHasReset && _pitchDegrees < _minAngle) {
				_raycast.Invalidate();

				// TODO: Less ugly way to determine this
				Vector3 angles = Vector3.Scale(Quaternion.LookRotation(_targetCamera.transform.forward).eulerAngles - new Vector3(0f, 180f, 0f), new Vector3(-1f, 1f, 1f));

				_pitchDegrees = angles.x;
				_aroundDegrees = angles.y;

			}

			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (_input.Delta1.magnitude > _startThreshold) {

				_inputDelta = Vector2.ClampMagnitude(_input.Delta1, _deltaClamp);

			}

			if (_inputDelta.magnitude > _startThreshold)
			{

				TransformClone t = TransformClone.FromTransform(_targetCamera.transform);

				_aroundDegrees += _inputDelta.x;
				_pitchDegrees += _inputDelta.y;

				_inputDelta *= _dampingFactor;
			
				if (_pitchDegrees < _minAngle) {
					return new List<IModifier>();
				}

				Vector3 pivot = _raycast.HitPoint  + (Vector3.up * _tiltUpFactor * (_pitchDegrees - _minAngle));

				float radius = Vector3.Distance(t.Position, _raycast.HitPoint);				

				
				Vector3 currentPos = t.Position;
				Vector3 newPos = Quaternion.Euler(_minAngle, _aroundDegrees, 0) * (radius * Vector3.forward) + _raycast.HitPoint;
				Vector3 deltaPos = newPos - currentPos;

				Quaternion currentRot = t.Rotation;
				Quaternion newRot = Quaternion.LookRotation(pivot - newPos);
				Quaternion deltaRot = newRot * Quaternion.Inverse(currentRot);


				//Debug.Log(string.Format("{0} = {1}",  Vector3.Scale(Quaternion.LookRotation(_targetCamera.transform.forward).eulerAngles - new Vector3(0f, 180f, 0f), new Vector3(-1f, 1f, 0f)),  new Vector3(_pitchDegrees, _aroundDegrees, 0)));
				//Debug.Log(_pitchDegrees - _minAngle);
			

				return new List<IModifier>() {
					new RotationModifier(deltaRot),
					new PositionModifier(deltaPos),
				};
			}
			return new List<IModifier>();
		}



		#endregion
	}
}

