using System;
using UnityEngine;

namespace FlexiCamera.Inputs
{
	public class TwistInput : IContinuousInput
	{
		//protected InteractionManager _interactionManager;

		public TwistInput()
		{
			//_interactionManager = InteractionManager.Instance;
			//_interactionManager.AddRotationZone(
			//	new Rect(0, 0, Screen.width, Screen.height),
			//	InteractionManager.InteractionLayer.IN_WORLD_A,
			//	StartTwist,
			//	UpdateTwist,
			//	EndTwist);
		}

		bool StartTwist(Vector2 pos, Vector2 pos2, float factor)
		{
			Position1 = pos;
			Position2 = pos2;
			Delta1 = Vector3.zero;
			Factor = factor;
			return false;
		}

		bool UpdateTwist(Vector2 pos, Vector2 pos2, float factor)
		{
			Position1 = pos;
			Position2 = pos2;
			Delta1 = Vector3.zero;
			Factor = factor;
			return false;
		}

		bool EndTwist(Vector2 pos, Vector2 pos2, float factor)
		{
			Position1 = pos;
			Position2 = pos2;
			Delta1 = Vector3.zero;
			Factor = 0.0f;
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

