using System;
using UnityEngine;

namespace FlexiCamera.Modifiers
{
	public class RotationModifier : IModifier
	{
		Quaternion _delta;

		public RotationModifier(Quaternion delta)
		{
			_delta = delta;
		}

		#region IModifier implementation

		public TransformClone Modify(TransformClone input)
		{
			input.Rotation = _delta * input.Rotation;

			return input;
		}

		#endregion
	}
}

