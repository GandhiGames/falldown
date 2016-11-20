
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
	[RequireComponent(typeof(ParticleSystem))]
	public class AutoDestructParticle : MonoBehaviour
	{
		public bool OnlyDeactivate;

		void OnEnable()
		{
			StartCoroutine("CheckIfAlive");
		}

		IEnumerator CheckIfAlive ()
		{
			while(true)
			{
				yield return new WaitForSeconds(0.5f);
				if(!GetComponent<ParticleSystem>().IsAlive(true))
				{
					if(OnlyDeactivate)
					{
						#if UNITY_3_5
						this.gameObject.SetActiveRecursively(false);
						#else
						this.gameObject.SetActive(false);
						#endif
					}
					else
						GameObject.Destroy(this.gameObject);
					break;
				}
			}
		}
	}
}