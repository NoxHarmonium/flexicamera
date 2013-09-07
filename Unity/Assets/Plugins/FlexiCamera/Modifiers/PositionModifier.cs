using System;
using UnityEngine;

namespace FlexiCamera.Modifiers
{
	public class PositionModifier : IModifier
	{
		Vector3 _delta;

		public PositionModifier(Vector3 delta)
		{
			_delta = delta;
		}

		#region IModifier implementation

		public TransformClone Modify(TransformClone input)
		{
			input.Position += _delta;
			return input;
		}

		#endregion
	}
}

