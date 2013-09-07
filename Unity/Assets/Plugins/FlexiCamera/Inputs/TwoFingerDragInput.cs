using System;
using UnityEngine;

namespace FlexiCamera.Inputs
{
	public class TwoFingerDragInput : IContinuousInput
	{
		protected InteractionManager _interactionManager;
		public DragStates DragState {
			get;
			protected set;
		}

		public enum DragStates
		{
			Started,
			InProgress,
			Ended
		}

		public TwoFingerDragInput()
		{
			_interactionManager = InteractionManager.Instance;
			_interactionManager.AddTwoFingerDragZone(
				new Rect(0, 0, Screen.width, Screen.height),
				InteractionManager.InteractionLayer.IN_WORLD_A,
				StartDrag,
				UpdateDrag,
				EndDrag);
		}

		bool StartDrag(Vector2 pos, Vector2 velocity)
		{
			Position = pos;
			Delta = velocity;
			DragState = DragStates.Started;
			Debug.Log("S");
			return false;
		}

		bool UpdateDrag(Vector2 pos, Vector2 velocity)
		{
			Position = pos;
			Delta = velocity;
			DragState = DragStates.InProgress;
			Debug.Log("U");
			return false;
		}

		bool EndDrag(Vector2 pos, Vector2 velocity)
		{
			Position = pos;
			Delta = velocity;
			DragState = DragStates.Ended;
			Debug.Log("E");
			return false;
		}

		#region IContinuousInput implementation
		public UnityEngine.Vector3 Delta {
			get ;
			protected set;
		}
		public UnityEngine.Vector3 NormalisedDelta {
			get {
				return new Vector3(this.Delta.x/Screen.width, 0f, this.Delta.y/Screen.height);
			}
		}
		public UnityEngine.Vector3 Position {
			get ;
			protected set;
		}
		public UnityEngine.Vector3 NormalisedPosition
		{
			get {
				return new Vector3(this.Position.x/Screen.width, 0f, this.Position.y/Screen.height);
			}
		}
		public float Factor {
			get ;
			protected set;
		}
		#endregion


	}
}

