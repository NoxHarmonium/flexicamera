using System;
using UnityEngine;

namespace FlexiCamera.Inputs
{
	public class DragInput : IContinuousInput
	{
		protected InteractionManager _interactionManager;

		public DragInput()
		{
			_interactionManager = InteractionManager.Instance;
			_interactionManager.AddDragZone(
				new Rect(0, 0, Screen.width, Screen.height),
				InteractionManager.InteractionLayer.IN_WORLD_A,
				StartDrag,
				UpdateDrag,
				EndDrag);
		}

		bool StartDrag(Vector2 pos, Vector2 velocity)
		{
			Position1 = pos;
			Delta1 = velocity;
			return false;
		}

		bool UpdateDrag(Vector2 pos, Vector2 velocity)
		{
			Position1 = pos;
			Delta1 = velocity;
			return false;
		}

		bool EndDrag(Vector2 pos, Vector2 velocity)
		{
			Position1 = pos;
			Delta1 = velocity;
			return false;
		}

		#region IContinuousInput implementation
		public UnityEngine.Vector3 Delta1 {
			get ;
			protected set;
		}
		public UnityEngine.Vector3 NormalisedDelta1 {
			get {
				return new Vector3(this.Delta1.x/Screen.width, 0f, this.Delta1.y/Screen.height);
			}
		}
		public UnityEngine.Vector3 Position1 {
			get ;
			protected set;
		}
		public UnityEngine.Vector3 NormalisedPosition1
		{
			get {
				return new Vector3(this.Position1.x/Screen.width, 0f, this.Position1.y/Screen.height);
			}
		}

		public UnityEngine.Vector3 Delta2 {
			get ;
			protected set;
		}
		public UnityEngine.Vector3 NormalisedDelta2 {
			get {
				return new Vector3(this.Delta2.x/Screen.width, 0f, this.Delta2.y/Screen.height);
			}
		}
		public UnityEngine.Vector3 Position2 {
			get ;
			protected set;
		}
		public UnityEngine.Vector3 NormalisedPosition2
		{
			get {
				return new Vector3(this.Position2.x/Screen.width, 0f, this.Position2.y/Screen.height);
			}
		}

		public float Factor {
			get ;
			protected set;
		}
		#endregion


	}
}

