using UnityEngine;

namespace FlexiCamera.Inputs
{
	public interface IContinuousInput : IInput
	{
		Vector3 Delta {
			get;
		}

		Vector3 Position {
			get;
		}

		Vector3 NormalisedDelta {
			get;
		}

		Vector3 NormalisedPosition {
			get;
		}

		float Factor {
			get;
		}

	}
}

