
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
	public class ObstacleLogic : MonoBehaviour
	{
		public GameObject explosion;

		//bool behindPlayer = false;

		GameManager _gameManager;

		void Awake ()
		{
			_gameManager = FindObjectOfType<GameManager> ();
		}

		void OnEnable ()
		{
			//behindPlayer = false;

			int count = transform.childCount;

			int rand = Random.Range (0, count);

			foreach (Transform t in transform) {
				t.gameObject.SetActive (false);
			}

			transform.GetChild (rand).gameObject.SetActive (true);
		}

		void Update ()
		{
			transform.Translate (_gameManager.obstacleSpeedMultiplicator * Vector3.back * _gameManager._scrollSpeed * Time.deltaTime * 5f);

			//float dist = Vector3.Distance (Camera.main.transform.position, transform.position);

			//print (dist);

			//if (dist < 10) {
			//behindPlayer = true;
			//}

			//if (behindPlayer && dist < 1) 
			if (transform.position.z < 0f) {
				_gameManager.point++;
				Destroy (gameObject);
			}
		}

		public Transform DoExplosion ()
		{
			var go = Instantiate (explosion) as GameObject;

			return go.transform;
		}

	}
}