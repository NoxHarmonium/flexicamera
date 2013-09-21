using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace FlexiCamera.InputAdapters
{
	public class FingerGesturesInputAdapter : InputAdapter
	{
		protected const int INPUT_MESSAGE_LIST_PREALLOC = 10;
		
		List<InputMessage> _updates = new List<InputMessage>(INPUT_MESSAGE_LIST_PREALLOC);
		
		/// <summary>
		/// Gets the updates to gesture states since the last call 
		/// </summary>
		public override List<InputMessage> GetUpdates()
		{
			List<InputMessage> updates = _updates;
			_updates = new List<InputMessage>(INPUT_MESSAGE_LIST_PREALLOC);
			return updates;
		}
		
		//--------------------------------------------------------------------------
		// Finger Gesture handlers
		//--------------------------------------------------------------------------
		
		// Continous Gestures
		
		public void OnFingerDown(FingerDownEvent e)
		{
			_updates.Add(new InputMessage(
				InputMessage.InputTypes.FingerDown, 
				InputMessage.MessageTypes.Transient,
				e.Position
			));
		}

		public void OnFingerUp(FingerUpEvent e)
		{
			_updates.Add(new InputMessage(
				InputMessage.InputTypes.FingerUp, 
				InputMessage.MessageTypes.Transient,
				e.Position
			));
		}

		public void OnFingerStationary(FingerMotionEvent e)
		{
			_updates.Add(new InputMessage(
				InputMessage.InputTypes.FingerStationary, 
				InputMessage.MessageTypes.Transient,
				e.Position
			));
		}

		public void OnFingerMove(FingerMotionEvent e)
		{
		 	_updates.Add(new InputMessage(
				InputMessage.InputTypes.FingerMoving, 
				InputMessage.MessageTypes.Transient,
				e.Position
			));
		}

		// Discrete Gestures
		
		public void OnTap(TapGesture tap)
		{
			if (tap.State == GestureRecognitionState.Recognized) {
				_updates.Add(new InputMessage(
					InputMessage.InputTypes.OneFingerTap, 
					InputMessage.MessageTypes.Transient,
					tap.Fingers.Select(f => f.Position).ToList(),
					tap.Fingers.Select(f => f.DeltaPosition).ToList()
				));
			}
		}
		
		public void OnLongPress(LongPressGesture longPress)
		{
			if (longPress.State == GestureRecognitionState.Recognized) {
				_updates.Add(new InputMessage(
					InputMessage.InputTypes.OneFingerLongTap, 
					InputMessage.MessageTypes.Transient,
					longPress.Fingers.Select(f => f.Position).ToList(),
					longPress.Fingers.Select(f => f.DeltaPosition).ToList()
				));
			}
		}
		
		/*
		// TODO: Impliment Swipe
		
		public void OnSwipe(SwipeGesture swipe)
		{
			Vector2 velocity = new Vector2();

			switch (swipe.Direction) {
				case FingerGestures.SwipeDirection.Right:
					velocity = Vector2.right * swipe.Velocity;
					break;
				case FingerGestures.SwipeDirection.Left:
					velocity = Vector2.right * -swipe.Velocity;
					break;
				case FingerGestures.SwipeDirection.Up:
					velocity = Vector2.up * swipe.Velocity;
					break;
				case FingerGestures.SwipeDirection.Down:
					velocity = Vector2.up * -swipe.Velocity;
					break;
			}

			HandleDragMoveEvent(swipe.Position, velocity, _swipeZones, true);
		}
		
		*/
		
		
		public void OnDrag(DragGesture drag)
		{
			InputMessage.MessageTypes type = InputMessage.MessageTypes.Failed;
			
			switch (drag.Phase) {
				case ContinuousGesturePhase.Started:
					type = InputMessage.MessageTypes.Begin;
					break;
				case ContinuousGesturePhase.Updated:
					type = InputMessage.MessageTypes.Update;
					break;
				case ContinuousGesturePhase.Ended:
					type = InputMessage.MessageTypes.End;
					break;
				default:
					break;

			}
			
			_updates.Add(new InputMessage(
				InputMessage.InputTypes.OneFingerDrag, 
				type,
				drag.Fingers.Select(f => f.Position).ToList(),
				drag.Fingers.Select(f => f.DeltaPosition).ToList()
			));
		}

		public void OnTwoFingerDrag(DragGesture drag)
		{
			InputMessage.MessageTypes type = InputMessage.MessageTypes.Failed;
			
			switch (drag.Phase) {
				case ContinuousGesturePhase.Started:
					type = InputMessage.MessageTypes.Begin;
					break;
				case ContinuousGesturePhase.Updated:
					type = InputMessage.MessageTypes.Update;
					break;
				case ContinuousGesturePhase.Ended:
					type = InputMessage.MessageTypes.End;
					break;
				default:
					break;

			}
			
			_updates.Add(new InputMessage(
				InputMessage.InputTypes.TwoFingerDrag, 
				type,
				drag.Fingers.Select(f => f.Position).ToList(),
				drag.Fingers.Select(f => f.DeltaPosition).ToList()
			));
		}

		public void OnTwist(TwistGesture twist)
		{
			InputMessage.MessageTypes type = InputMessage.MessageTypes.Failed;
			
			switch (twist.Phase) {
				case ContinuousGesturePhase.Started:
					type = InputMessage.MessageTypes.Begin;
					break;
				case ContinuousGesturePhase.Updated:
					type = InputMessage.MessageTypes.Update;
					break;
				case ContinuousGesturePhase.Ended:
					type = InputMessage.MessageTypes.End;
					break;
				default:
					break;

			}
			
			_updates.Add(new InputMessage(
				InputMessage.InputTypes.TwoFingerTwist, 
				type,
				twist.Fingers.Select(f => f.Position).ToList(),
				twist.Fingers.Select(f => f.DeltaPosition).ToList(),
				new List<float> { twist.DeltaRotation }
			));
		}

	public void OnPinch(PinchGesture pinch)
		{
			InputMessage.MessageTypes type = InputMessage.MessageTypes.Failed;
			
			switch (pinch.Phase) {
				case ContinuousGesturePhase.Started:
					type = InputMessage.MessageTypes.Begin;
					break;
				case ContinuousGesturePhase.Updated:
					type = InputMessage.MessageTypes.Update;
					break;
				case ContinuousGesturePhase.Ended:
					type = InputMessage.MessageTypes.End;
					break;
				default:
					break;

			}
			
			_updates.Add(new InputMessage(
				InputMessage.InputTypes.TwoFingerTwist, 
				type,
				pinch.Fingers.Select(f => f.Position).ToList(),
				pinch.Fingers.Select(f => f.DeltaPosition).ToList(),
				new List<float> { pinch.Delta }
			));
		}	
	}
}

