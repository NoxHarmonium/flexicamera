using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.InputAdapters
{
	public class InputMessage
	{
		#region Enums
		
		public enum InputTypes
		{
			// Discrete gestures
			OneFingerTap,
			OneFingerLongTap,
			OneFingerDrag,
			TwoFingerDrag,
			TwoFingerTwist,
			TwoFingerPinch,
			
			// Continuous gestures
			FingerDown,
			FingerUp,
			FingerStationary,
			FingerMoving
		}
		
		public enum MessageTypes
		{
			Transient,
			Begin,
			Update,
			End
		}
		
		#endregion
		
		#region Public Properties
		
		/// <summary>
		/// Gets the input type of this message
		/// </summary>
		public InputTypes InputType {
			get;
			protected set;
		}
		
		/// <summary>
		/// Gets the message type of this message
		/// </summary>
		public MessageTypes MessageType {
			get;
			protected set;
		}
		
		/// <summary>
		/// Gets the positions of each finger utilised in gesture that triggered this message 
		/// </summary>
		public List<Vector2> FingerPositions {
			get;
			protected set;
		}
			
		/// <summary>
		/// Gets the change in position of each finger utilised in gesture that triggered this message 
		/// </summary>
		public List<Vector2> FingerDeltas {
			get;
			protected set;
		}
		
		/// <summary>
		/// Gets data associated with the gesture such as twist or pinch amounts
		/// </summary>
		public List<float> 	 GestureData {
			get;
			protected set;
		}
		
		#endregion
		
		#region Constuctors
		
		public InputMessage(
			InputTypes inputType, MessageTypes messageType, 
			List<Vector2> fingerPositions, List<Vector2> fingerDeltas = null, 
			List<float> gestureData = null
		)
		{
			this.InputType = inputType;
			this.MessageType = messageType;
			this.FingerPositions = fingerPositions;
			this.FingerDeltas = fingerDeltas;
			this.GestureData = gestureData;
		
		}
		
		public InputMessage(
			InputTypes inputType, MessageTypes messageType, 
			Vector2 fingerPosition, Vector2 fingerDelta = null, 
			float gestureData = null
		)
		{
			this.InputType = inputType;
			this.MessageType = messageType;
			this.FingerPositions = new List<Vector2>() { fingerPosition };
			this.FingerDeltas = new List<Vector2>() { fingerDelta };
			this.GestureData = new List<float>() { gestureData };
		
		}
		
		#endregion
		
		
	}
}

