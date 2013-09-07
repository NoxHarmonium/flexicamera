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

		void Invalidate();
	}
}

