using System;
using UnityEngine;

namespace FlexiCamera.Inputs
{
	public class TwistInput : IContinuousInput
	{
		protected InteractionManager _interactionManager;

		public TwistInput()
		{
			_interactionManager = InteractionManager.Instance;
			_interactionManager.AddRotationZone(
				new Rect(0, 0, Screen.width, Screen.height),
				InteractionManager.InteractionLayer.IN_WORLD_A,
				StartTwist,
				UpdateTwist,
				EndTwist);
		}

		bool StartTwist(Vector2 pos, Vector2 pos2, float factor)
		{
			Position = pos;
			Delta = Vector3.zero;
			Factor = factor;
			return false;
		}

		bool UpdateTwist(Vector2 pos, Vector2 pos2, float factor)
		{
			Position = pos;
			Delta = Vector3.zero;
			Factor = factor;
			return false;
		}

		bool EndTwist(Vector2 pos, Vector2 pos2, float factor)
		{
			Position = pos;
			Delta = Vector3.zero;
			Factor = 0.0f;
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

