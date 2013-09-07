using UnityEngine;

namespace FlexiCamera
{
	public struct TransformClone
	{
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 Scale;

		private TransformClone(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			this.Position = position;
			this.Rotation = rotation;
			this.Scale = scale;
		}

		public static TransformClone FromTransform(Transform t)
		{
			return new TransformClone(t.position, t.rotation, t.localScale);;
		}

		public void ApplyToTransform(Transform t){
			t.position = this.Position;
			t.rotation = this.Rotation;
			t.localScale = this.Scale;
		}

	}
}

