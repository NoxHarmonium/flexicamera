using System;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using InputAdapters;
	
	public interface IController
	{
		List<IModifier> GetModifiers();
		void ProcessMessage(InputMessage message);
	}
}

