using System;
using UnityEngine;

namespace FlexiCamera.Raycasters
{
	public class RaycastScreenPointFromCamera : IRaycast
	{
		protected Camera _targetCamera;
		protected Vector2 _screenPosition;
		protected RaycastHit _hitInfo;
		protected bool _didHit;
		protected LayerMask _layerMask;
		protected float _maxRayDistance = 100f;

		public RaycastScreenPointFromCamera(Camera targetCamera, Vector2 screenPosition, LayerMask layerMask)
		{
			this._targetCamera = targetCamera;
			this._screenPosition = screenPosition;
			this._layerMask = layerMask;
			Invalidate();
		}


		#region IRaycaster implementation
		public void Invalidate()
		{
			Ray ray = _targetCamera.ScreenPointToRay((Vector3)_screenPosition);
			_didHit = Physics.Raycast(ray, out _hitInfo, _maxRayDistance, _layerMask);
		}
		public bool DidHit {
			get {
				return _didHit;
			}
		}
		public Vector3 HitPoint {
			get {
				if (!_didHit)
					return default(Vector3);
				return _hitInfo.point;
			}
		}
		public float Distance {
			get {
				if (!_didHit)
					return default(float);
				return  _hitInfo.distance;
			}
		}
		public Vector3 CastPoint {
			get {
				return _screenPosition;
			}
			set {
				_screenPosition = value;
			}
		}

		#endregion
	}
}

