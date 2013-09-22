using System;
using UnityEngine;

namespace FlexiCamera.Raycasters
{
	public class RaycastScreenPointFromCamera : IRaycast
	{
		protected Camera _targetCamera;
		protected Vector2 _screenPosition;
		protected bool _didHit;
		protected float _maxRayDistance = 100f;
		protected Plane _plane;
		protected float _distance;

		public RaycastScreenPointFromCamera(Camera targetCamera, Vector2 screenPosition)
		{
			this._targetCamera = targetCamera;
			this._screenPosition = screenPosition;
			_plane = new Plane(Vector3.down, Vector3.zero);
			Invalidate();
		}


		#region IRaycaster implementation
		public void Invalidate()
		{
			Ray ray = _targetCamera.ScreenPointToRay((Vector3)_screenPosition);
			_didHit = _plane.Raycast(ray, out _distance);
			_distance = Mathf.Abs(_distance);
			HitPoint = ray.GetPoint(_distance);
		
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

