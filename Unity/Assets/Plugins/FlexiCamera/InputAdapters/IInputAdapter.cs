using System;
using System.Collections.Generic;

namespace FlexiCamera.InputAdapters
{
	public interface IInputAdapter
	{
		/// <summary>
		/// Gets the updates to gesture states since the last call 
		/// </summary>
		List<InputMessage> GetUpdates();
	}
}

