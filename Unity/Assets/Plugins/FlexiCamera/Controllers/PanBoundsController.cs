using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
	using Modifiers;

	public class PanBoundsController : IController
	{
		protected Camera _targetCamera;
		protected RaycastFromCameraCenter _raycast;
		protected float _maxRadius = 1.25f;
		protected float _bounceTime = 1f;
		protected Vector3 _center = Vector3.zero;
		protected float _maxAngle = -7.5f;

		public PanBoundsController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera, parent.LayerMask);

		}

		#region IController implementation

		public List<IModifier> GetModifiers()
		{
			_raycast.Invalidate();

			// TODO: Less ugly way to determine this
			Vector3 angles = Vector3.Scale(Quaternion.LookRotation(_targetCamera.transform.forward).eulerAngles - new Vector3(0f, 180f, 0f), new Vector3(-1f, 1f, 1f));

			float pitchDegrees = angles.x;


			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (_raycast.DidHit  && pitchDegrees < _maxAngle) {
				bool active = false;
				Vector3 worldDelta = Vector3.zero;
				float distance = Vector3.Distance(_center, _raycast.HitPoint);

				if (distance > _maxRadius) {
					float overshoot = distance - _maxRadius;

					Vector3 direction = (-_raycast.HitPoint).normalized;
					worldDelta = direction * Mathf.Pow(overshoot,2.0f) * Time.deltaTime * (1f / _bounceTime);

					active = true;
				}

				if (!active) {
					return new List<IModifier>();
				}

				return new List<IModifier>() {
					new PositionModifier(worldDelta),
				};
			}


			return new List<IModifier>();
		}



		#endregion
	}
}

