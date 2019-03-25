using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bradley.AlienArk
{
	public class StateIndicator : MonoBehaviour 
	{
		public RectTransform m_pointer;
		public Vector3 m_offset;
		protected Enemy m_target;

		public virtual void SetIndicator(Enemy target)
		{
			m_target = target;
			transform.position = m_target.transform.position + m_offset;
		}

		private void LateUpdate() 
		{
			UpdatePosition();
			UpdatePointer();
		}

		protected virtual void UpdatePosition()
		{
			Vector3 screenPosition = Camera.main.WorldToScreenPoint(m_target.transform.position + m_offset);
			/*
			screenPosition = new Vector3(Mathf.Clamp(screenPosition.x, 60, Camera.main.scaledPixelWidth - 60), 
				Mathf.Clamp(screenPosition.y, 60, Camera.main.scaledPixelHeight - 120), screenPosition.z);
			screenPosition.z = 10;
			*/
			transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(screenPosition), 0.4f);
		}

		protected virtual void UpdatePointer()
		{
			float angle = Vector2.SignedAngle(-m_pointer.up, m_target.transform.position - transform.position);
			Quaternion rot = m_pointer.transform.rotation;
			m_pointer.transform.rotation = Quaternion.Lerp(rot, Quaternion.Euler(new Vector3(0,0,angle + rot.eulerAngles.z)), 0.4f);
		}
	}
}
