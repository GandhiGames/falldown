
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
	[RequireComponent(typeof (Collider))]
	[RequireComponent(typeof (Rigidbody))]
	public class ObstacleCollisionDetection : MonoBehaviour 
	{
		void OnTriggerEnter(Collider other) 
		{
			if (other.name.Contains ("Player")) 
			{
				var t = GetComponentInParent<ObstacleLogic> ().DoExplosion();
				t.transform.position = other.transform.position;
				FindObjectOfType<GameManager>().GameOver ();
			}
		}
	}
}