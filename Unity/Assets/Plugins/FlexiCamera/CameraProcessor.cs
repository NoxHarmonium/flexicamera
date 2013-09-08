using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FlexiCamera.Modifiers;
using FlexiCamera.Controllers;

namespace FlexiCamera
{
	public class CameraProcessor : MonoBehaviour
	{
		protected Camera _targetCamera;
		protected List<List<IController>> _controllers;

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
			_controllers = new List<List<IController>>();

			//TODO: Selectable cameras?



		}

		#region Unity Events

		protected void Awake()
		{
			_targetCamera = Camera.main;


			_controllers.Add(new List<IController> () { 
				new RotateOrbitController(this),
				new PinchZoomController(this),
			 	new PanController(this) ,
				new OrbitController(this) ,

			});

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
			bool cancel = false;

			foreach (List<IController> controllerLevel in _controllers) {

				if (cancel) {
					break;
				}

				foreach (IController controller in controllerLevel) {
					List<IModifier> cModifiers = controller.GetModifiers();
					foreach (IModifier modifier in cModifiers) {
						modifiers.AddLast(modifier);
						cancel = true;
					}
				}
			}
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

