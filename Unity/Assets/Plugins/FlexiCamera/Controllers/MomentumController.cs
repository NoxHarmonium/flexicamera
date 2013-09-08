using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
	using Modifiers;

	public class MomentumController : IController
	{
		protected Camera _targetCamera;
		protected float _mass = 10f;
		protected float _drag = 1.5f;
		protected float _damper = 0.75f;
		protected Vector3 _posMomentum = Vector3.zero;
		protected Vector3 _rotMomentum = Vector3.zero;

		protected Vector3 _lastPosDelta = Vector3.zero;
		protected Vector3 _lastPosition = Vector3.zero;

		protected Vector3 _lastRotDelta = Vector3.zero;
		protected Vector3 _lastRotation = Vector3.zero;


		public MomentumController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			_lastPosition = this._targetCamera.transform.position;
			_lastRotation = this._targetCamera.transform.eulerAngles;

		}

		#region IController implementation

		public List<IModifier> GetModifiers()
		{
			TransformClone t = TransformClone.FromTransform(_targetCamera.transform);

			//Position
			Vector3 posDelta = t.Position - _lastPosition;

			posDelta -= _lastPosDelta;
			Vector3 mPos = posDelta * _mass * _damper;
			_posMomentum += mPos;
			_posMomentum *= 1 / _drag;

			//Position
			Vector3 rotDelta = FlexiUtils.EulerDifference(t.Rotation.eulerAngles, _lastRotation);
			rotDelta = FlexiUtils.EulerSubtract(rotDelta, _lastRotDelta);
			Vector3 mRot = rotDelta * _mass * _damper;
			_rotMomentum += mRot;
			_rotMomentum *= 1f / _drag;

			_lastPosition = t.Position;
			_lastRotation = t.Rotation.eulerAngles;

			_lastPosDelta = (_posMomentum / _mass);
			_lastRotDelta = (_rotMomentum / _mass);

			Debug.Log(rotDelta);

			return new List<IModifier>() { 
				//new PositionModifier(_lastPosDelta), 
				//new RotationModifier(Quaternion.Euler(_lastRotDelta)) 
			};






		}



		#endregion
	}
}

