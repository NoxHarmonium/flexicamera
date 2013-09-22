using System;
using UnityEngine;

namespace FlexiCamera.Raycasters
{
	public class RaycastFromCameraCenter : IRaycast
	{
		protected Camera _targetCamera;
		protected RaycastHit _hitInfo;
		protected bool _didHit;
		protected float _maxRayDistance = 100f;
		protected Plane _plane;
		protected float _distance;

		public RaycastFromCameraCenter(Camera targetCamera)
		{
			this._targetCamera = targetCamera;
			_plane = new Plane(Vector3.down, Vector3.zero);
			Invalidate();
		}


		#region IRaycaster implementation
		public void Invalidate()
		{
			Ray ray = _targetCamera.ViewportPointToRay(CastPoint);
			this.Direction = ray.direction;
			_didHit = _plane.Raycast(ray, out _distance);
			
			_distance = Mathf.Abs(_distance);
			
			HitPoint = ray.GetPoint(_distance);
			Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.cyan, 0.2f);
			
			Debug.DrawLine(_targetCamera.transform.position, HitPoint, Color.green, 0.1f);
		}
		public bool DidHit {
			get {
				if (!_didHit && _distance == 0f)
				{
					return false; // Paralell to plane
				}
				return true; 
			}
		}
		public Vector3 HitPoint {
			get;
			protected set;
		}
		public float Distance {
			get {
				return _distance;
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
		public Vector3 Direction {
			get;
			protected set;

		}
		#endregion
	}
}

