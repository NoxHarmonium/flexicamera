using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.InputAdapters
{
	public class InputAdapter : MonoBehaviour
	{
		/// <summary>
		/// Gets the updates to gesture states since the last call 
		/// </summary>
		public virtual List<InputMessage> GetUpdates()
		{
		
			return null;
		}
		
	}
}

