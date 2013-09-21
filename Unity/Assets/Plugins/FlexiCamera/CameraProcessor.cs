using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FlexiCamera.Modifiers;
using FlexiCamera.Controllers;
using FlexiCamera.InputAdapters;

namespace FlexiCamera
{
	public class CameraProcessor : MonoBehaviour
	{
		protected Camera _targetCamera;
		protected List<IController> _controllers;

		public Camera TargetCamera
		{
			get {
				return _targetCamera;
			}
		}

		public LayerMask LayerMask {
			get ;
			set ;
		}

		public CameraProcessor()
		{
			_controllers = new List<IController>();

			//TODO: Selectable cameras?



		}

		#region Unity Events

		protected void Awake()
		{
			_targetCamera = Camera.main;
			this.LayerMask = 1 << LayerMask.NameToLayer("Environment");

			_controllers = new List<IController> () { 
				new RotateOrbitController(this),
				new PinchZoomController(this),
			 	new PanController(this) ,
				new OrbitController(this) ,
				new BelowGroundOrbitController(this) ,
				new ZoomBoundsController(this),
				new PanBoundsController(this)
				//new MomentumController(this)

			};

		}

		protected void Update()
		{
			TransformClone source = TransformClone.FromTransform(_targetCamera.transform);

			LinkedList<IModifier> mods = BuildModifierStack();
			TransformClone output = ApplyModifierStack(mods, source);

			output.ApplyToTransform(_targetCamera.transform);

		}

		#endregion

		#region Protected Methods

		protected LinkedList<IModifier> BuildModifierStack()
		{
			LinkedList<IModifier> modifiers = new LinkedList<IModifier>();
			bool showDebugDivider = false;
			
			List<InputMessage> inputMessages = new List<InputMessage>();

			foreach (IController controller in _controllers) {
				foreach (InputMessage inputMessage in inputMessages)
				{
					controller.ProcessMessage(inputMessage);
				}
				List<IModifier> cModifiers = controller.GetModifiers();
				if (cModifiers.Count > 0) {
					Debug.Log("Controller: " + controller.GetType().Name + " Modifiers: " + cModifiers.Count);
					showDebugDivider = true;
				}
				foreach (IModifier modifier in cModifiers) {
					modifiers.AddLast(modifier);
				}
			}

			if (showDebugDivider)
				Debug.Log("------------");

			return modifiers;
		}

		protected TransformClone ApplyModifierStack(IEnumerable<IModifier> mods, TransformClone input)
		{
			foreach (IModifier mod in mods) {
				input = mod.Modify(input);
			}
			return input;
		}

		#endregion


	}
}

