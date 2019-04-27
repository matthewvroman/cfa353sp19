using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bradley.AlienArk
{
	[RequireComponent(typeof(Animator))]
	public class AnimateImage : MonoBehaviour 
	{
		public string animationName;
		void Start () 
		{
			GetComponent<Animator>().Play(animationName);
		}
	}
}
