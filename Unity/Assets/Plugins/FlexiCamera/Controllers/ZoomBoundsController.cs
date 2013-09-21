using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Inputs;
	using Modifiers;
	using InputAdapters;

	public class ZoomBoundsController : IController
	{
		protected Camera _targetCamera;
		protected RaycastFromCameraCenter _raycast;
		protected float _minDistance = 20f;
		protected float _maxDistance = 100f;
		protected float _bounceTime = 0.5f;

		public ZoomBoundsController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			this._raycast = new RaycastFromCameraCenter(parent.TargetCamera, parent.LayerMask);

		}

		#region IController implementation

		public void ProcessMessage(InputMessage message)
		{
			
		
		}
		
		public List<IModifier> GetModifiers()
		{
			_raycast.Invalidate();

			//Debug.Log(string.Format("mag: {0}, didHit: {1}", _input.Delta.magnitude, _raycast.DidHit));
			if (_raycast.DidHit) {
				Vector3 worldDelta = default(Vector3);
				Quaternion deltaRot = default(Quaternion);
				TransformClone t = TransformClone.FromTransform(_targetCamera.transform);
				bool active = false;

				//Debug.Log(_raycast.Distance);

				if (_raycast.Distance < _minDistance || _raycast.Distance > _maxDistance) {
					float overshoot = 0f;
					if (_raycast.Distance < _minDistance) {
						overshoot = _raycast.Distance - _minDistance;
					} else if (_raycast.Distance > _maxDistance) {
						overshoot = _raycast.Distance - _maxDistance;
					}

					Vector3 direction = (_raycast.HitPoint - t.Position).normalized;
					worldDelta = direction * overshoot * Time.deltaTime * (1f / _bounceTime);


					Quaternion newRot = Quaternion.LookRotation(direction);
					Quaternion currentRot = t.Rotation;
					deltaRot = newRot * Quaternion.Inverse(currentRot);
					active = true;
				}

				if (!active)
					return new List<IModifier>();

				return new List<IModifier>() {
					new PositionModifier(worldDelta),
					new RotationModifier(deltaRot)
				};
			}


			return new List<IModifier>();
		}



		#endregion
	}
}

