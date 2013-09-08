using UnityEngine;

namespace FlexiCamera.Raycasters
{
	public interface IRaycast
	{
		bool DidHit {
			get;
		}

		Vector3 HitPoint {
			get;
		}

		float Distance {
			get;
		}
		Vector3 CastPoint {
			get;
			set;
		}

		void Invalidate();
	}
}

