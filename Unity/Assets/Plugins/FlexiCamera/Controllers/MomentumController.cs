/* ========================================================================
 * FlexiCamera for Unity3D: 
 * https://github.com/NoxHarmonium/flexicamera 
 * <!!filename!!>
 * ========================================================================
 * Copyright Sean Dawson 2013 
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ======================================================================== */

using System;
using UnityEngine;
using System.Collections.Generic;

namespace FlexiCamera.Controllers
{
	using Raycasters;
	using Modifiers;
	using InputAdapters;

	public class MomentumController : IController
	{
		protected Camera _targetCamera;
		protected float _mass = 10f;
		protected float _drag = 1.5f;
		protected float _damper = 0.75f;
		protected Vector3 _posMomentum = Vector3.zero;
		protected Vector3 _rotMomentum = Vector3.zero;

		protected Vector3 _lastPosDelta = Vector3.zero;
		protected Vector3 _lastPosition = Vector3.zero;

		protected Vector3 _lastRotDelta = Vector3.zero;
		protected Vector3 _lastRotation = Vector3.zero;


		public MomentumController(CameraProcessor parent)
		{
			this._targetCamera = parent.TargetCamera;
			_lastPosition = this._targetCamera.transform.position;
			_lastRotation = this._targetCamera.transform.eulerAngles;

		}

		#region IController implementation
		
		public void ProcessMessage(InputMessage message)
		{
		
		
		}
		
		public List<IModifier> GetModifiers()
		{
			TransformClone t = TransformClone.FromTransform(_targetCamera.transform);

			//Position
			Vector3 posDelta = t.Position - _lastPosition;

			posDelta -= _lastPosDelta;
			Vector3 mPos = posDelta * _mass * _damper;
			_posMomentum += mPos;
			_posMomentum *= 1 / _drag;

			//Position
			Vector3 rotDelta = FlexiUtils.EulerDifference(t.Rotation.eulerAngles, _lastRotation);
			rotDelta = FlexiUtils.EulerSubtract(rotDelta, _lastRotDelta);
			Vector3 mRot = rotDelta * _mass * _damper;
			_rotMomentum += mRot;
			_rotMomentum *= 1f / _drag;

			_lastPosition = t.Position;
			_lastRotation = t.Rotation.eulerAngles;

			_lastPosDelta = (_posMomentum / _mass);
			_lastRotDelta = (_rotMomentum / _mass);

			Debug.Log(rotDelta);

			return new List<IModifier>() { 
				//new PositionModifier(_lastPosDelta), 
				//new RotationModifier(Quaternion.Euler(_lastRotDelta)) 
			};






		}



		#endregion
	}
}

