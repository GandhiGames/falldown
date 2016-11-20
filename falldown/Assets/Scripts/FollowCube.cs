
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/


using UnityEngine;
using System.Collections;

namespace AppAdvisory.XtremNoBrakes
{
	public class FollowCube : MonoBehaviour
	{
		public Transform toFollow;

		void FixedUpdate () 
		{
			transform.position = new Vector3 (0, 0, toFollow.position.z - 11);
		}
	}
}