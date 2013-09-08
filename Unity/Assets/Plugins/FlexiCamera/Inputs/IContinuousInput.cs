using UnityEngine;

namespace FlexiCamera.Inputs
{
	public interface IContinuousInput : IInput
	{
		Vector3 Delta1 {
			get;
		}

		Vector3 Delta2 {
			get;
		}

		Vector3 Position1 {
			get;
		}

		Vector3 Position2 {
			get;
		}

		Vector3 NormalisedDelta1 {
			get;
		}

		Vector3 NormalisedDelta2 {
			get;
		}

		Vector3 NormalisedPosition1 {
			get;
		}

		Vector3 NormalisedPosition2 {
			get;
		}

		float Factor {
			get;
		}

	}
}

