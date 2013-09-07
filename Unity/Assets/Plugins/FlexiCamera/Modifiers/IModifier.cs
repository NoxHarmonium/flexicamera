using System;
using UnityEngine;

namespace FlexiCamera
{
	public interface IModifier
	{
		TransformClone Modify(TransformClone input);
	}
}

