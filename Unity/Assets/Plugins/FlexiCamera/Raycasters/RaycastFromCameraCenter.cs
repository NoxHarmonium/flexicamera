using System;
using UnityEngine;

namespace FlexiCamera.Raycasters
{
	public class RaycastFromCameraCenter : IRaycast
	{
		protected Camera _targetCamera;
		protected RaycastHit _hitInfo;
		protected LayerMask _layerMask;
		protected bool _didHit;

		public RaycastFromCameraCenter(Camera targetCamera, LayerMask layerMask)
		{
			this._targetCamera = targetCamera;
			this._layerMask = layerMask;
			Invalidate();
		}


		#region IRaycaster implementation
		public void Invalidate()
		{
			Ray ray = _targetCamera.ViewportPointToRay(CastPoint);
			_didHit = Physics.Raycast(ray, out _hitInfo);
			Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.cyan, 0.2f);
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
		public Vector3 CastPoint
		{
			get {
				return Vector2.one * 0.5f;
			}
			set {

			}
		}
		#endregion
	}
}

