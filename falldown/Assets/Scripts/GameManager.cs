
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/


using UnityEngine;
using System.Collections;

#if APPADVISORY_ADS
using AppAdvisory.Ads;
#endif

#if VS_SHARE
using AppAdvisory.SharingSystem;
#endif

namespace AppAdvisory.XtremNoBrakes
{
	public class GameManager : MonoBehaviour 
	{
		public int numberOfPlayToShowInterstitial = 5;

		public string VerySimpleAdsURL = "http://u3d.as/oWD";

		public GameObject obstacleParentPrefab;

		public AudioClip pointSound;

		public AudioClip[] musics;

		public AudioSource audioSource;

		public Transform player;

		bool isGameOver;

		public int _point = 0;
		public int point
		{
			set 
			{
				_point = value;

				if (!isGameOver) 
				{
					_canvasManager.SetPoint (value);

					if (PlayerPrefs.GetInt ("SOUND", 1) == 1)
						audioSource.PlayOneShot (pointSound);
				}
			}
			get 
			{
				return _point;
			}
		}

		bool doRotate = false;
		int direction = 1;
		public float rotateSpeed = 200f;

		public Transform wallParent;

		public Transform spawnPoint;

		public Material wallMaterial;

		public float obstacleSpeedMultiplicator = 2;

		public float scrollSpeed = 0.5F;
		public float _scrollSpeed
		{
			get 
			{
				return scrollSpeed + Mathf.Pow(((float)point),1.2f) / 10f;
			}
		}



		public int maxObstacles = 5;

		public float maxWaitBetwwenObstacles = 2f;

		public Vector3 gravity = new Vector3 (0, -30, 0);

		CanvasManager _canvasManager;

		void Awake()
		{
			_canvasManager = FindObjectOfType<CanvasManager>();

			isGameOver = true;
			audioSource = GetComponent<AudioSource> ();

			Physics.gravity = gravity;
			Application.targetFrameRate = 60;

			RenderSettings.ambientLight = Color.white;
		}

		public void StartGame()
		{
			var obst = FindObjectsOfType<ObstacleLogic> ();

			foreach (var o in obst) 
			{
				Destroy (o.gameObject);
			}

			player.gameObject.SetActive (true);



			StartCoroutine (DoLerpAlphaCanvasGroup (1, 0, 1));


			StartCoroutine (DoLerpAlphaTextTuto (0, 1, 1,1));

			StartCoroutine (DoLerpAlphaTextTuto (1, 0, 1,5));

			StartCoroutine (SpawnCorout ());

			PlayRandomMusic ();

			_point = 0;

			_canvasManager.SetPoint (0);

			isGameOver = false;
		}

		public void GameOver()
		{
			if (isGameOver)
				return;

			isGameOver = true;

			SetScores ();

			player.gameObject.SetActive (false);

			StartCoroutine (GameOverCorout ());

			ShowAds();

			#if VS_SHARE
			VSSHARE.DOTakeScreenShot();
			#endif
		}

		void SetScores()
		{
			PlayerPrefs.SetInt ("LAST_SCORE", point);

			int best = PlayerPrefs.GetInt ("BEST_SCORE", 0);

			if(point > best)
				PlayerPrefs.SetInt ("BEST_SCORE", point);

			PlayerPrefs.Save ();

			_canvasManager.SetScoresCanvas ();
		}

		IEnumerator GameOverCorout()
		{


			yield return new WaitForSeconds (1);

			StartCoroutine (DoLerpAlphaCanvasGroup (0, 1, 1));

			StopMusic ();

			_point = 0;
		}

		void PlayRandomMusic()
		{
			if (PlayerPrefs.GetInt ("SOUND", 1) == 0)
				return;

			audioSource.clip = musics [Random.Range (0, musics.Length)];
			audioSource.Play ();
		}

		void StopMusic()
		{
			audioSource.Stop ();
		}

		IEnumerator SpawnCorout()
		{
			while (true) 
			{
				var go = Instantiate (obstacleParentPrefab) as GameObject;

				go.transform.position = spawnPoint.position;

				go.transform.parent = spawnPoint;

				go.transform.localRotation = Quaternion.identity;

				yield return new WaitForSeconds(Mathf.Max(maxWaitBetwwenObstacles - ((float)point)/10f, maxWaitBetwwenObstacles/3f));

				while (maxObstacles <= spawnPoint.childCount) 
				{
					yield return 0;
				}

				if (isGameOver)
					break;

				yield return 0;
			}
		}



		void Update()
		{
			//		if (doRotate) 
			//		{
			//			wallParent.Rotate(0, 0, direction*rotateSpeed*Time.deltaTime, Space.World);
			//		}

			//
			wallMaterial.SetTextureOffset("_MainTex", new Vector2(0,-Time.time * _scrollSpeed));


			if (isGameOver) 
			{
				StopRotate ();
				return;
			}

			for (var i = 0; i < Input.touchCount; ++i) 
			{
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved) {
					if (touch.position.x > (Screen.width / 2)) {
						TurnRight ();
					} else {
						TurnLeft ();
					}
				} 

				if (touch.phase == TouchPhase.Ended)
				{
					StopRotate ();
				} 
			}

			if (Input.GetKeyDown (KeyCode.LeftArrow)) 
			{
				TurnLeft ();
			} 

			if (Input.GetKeyDown (KeyCode.RightArrow)) 
			{
				TurnRight ();
			}

			if (Input.GetKeyUp (KeyCode.RightArrow) || Input.GetKeyUp (KeyCode.LeftArrow))
			{
				StopRotate ();
			}
		}

		void TurnLeft()
		{
			doRotate = true;
			direction = 1;
		}

		void TurnRight()
		{
			doRotate = true;
			direction = -1;
		}

		void StopRotate()
		{
			doRotate = false;
		}

		void FixedUpdate()
		{

			if (doRotate) 
			{
				wallParent.Rotate(0, 0, direction*rotateSpeed*Time.deltaTime, Space.World);
			}

			//		CheckPlayerPosition ();
		}

		void CheckPlayerPosition()
		{
			if (player.position.x < 0) 
			{
				if (player.position.x < -4.5f) 
				{
					player.position = new Vector3 (-4.5f, player.position.y, player.position.z);
				}
			}

			if (player.position.x > 0) 
			{
				if (player.position.x > 4.5f) 
				{
					player.position = new Vector3 (4.5f, player.position.y, player.position.z);
				}
			}

			if (player.position.y < 0) 
			{
				if (player.position.y < -4.5f) 
				{
					player.position = new Vector3 (player.position.x, -4.5f, player.position.z);
				}
			}

			if (player.position.y > 0) 
			{
				if (player.position.y > 4.5f) 
				{
					player.position = new Vector3 (player.position.x, 4.5f, player.position.z);
				}
			}
		}
		//	IEnumerator ChangeColor(){
		//
		//		while (true) {
		//
		//			Color colorTemp = colors [UnityEngine.Random.Range (0, colors.Length)];
		//
		//			StartCoroutine(DoLerp(sprite.color, colorTemp, 3f));
		//
		//			yield return new WaitForSeconds (10f);
		//		}
		//
		//	}
		//
		public IEnumerator DoLerpAlphaCanvasGroup(float from, float to, float time)
		{
			_canvasManager.menu.alpha = from;
			float timer = 0;
			while (timer <= time)
			{
				timer += Time.deltaTime;
				_canvasManager.menu.alpha = Mathf.Lerp(from, to, timer / time);
				yield return null;
			}
			_canvasManager.menu.alpha = to;
		}

		public IEnumerator DoLerpAlphaTextTuto(float from, float to, float time, float wait)
		{
			yield return new WaitForSeconds (wait);

			_canvasManager.TextTutoDesktop.gameObject.SetActive (!Application.isMobilePlatform);
			_canvasManager.TextTutoMobile.gameObject.SetActive (Application.isMobilePlatform);

			_canvasManager.TextTutoDesktop.color = new Color (_canvasManager.TextTutoMobile.color.r, _canvasManager.TextTutoMobile.color.g, _canvasManager.TextTutoMobile.color.b, from);
			_canvasManager.TextTutoMobile.color = new Color (_canvasManager.TextTutoMobile.color.r, _canvasManager.TextTutoMobile.color.g, _canvasManager.TextTutoMobile.color.b, from);

			float timer = 0;
			while (timer <= time)
			{
				timer += Time.deltaTime;
				float alpha = Mathf.Lerp(from, to, timer / time);
				_canvasManager.TextTutoDesktop.color = new Color (_canvasManager.TextTutoMobile.color.r, _canvasManager.TextTutoMobile.color.g, _canvasManager.TextTutoMobile.color.b, alpha);
				_canvasManager.TextTutoMobile.color = new Color (_canvasManager.TextTutoMobile.color.r, _canvasManager.TextTutoMobile.color.g, _canvasManager.TextTutoMobile.color.b, alpha);
				yield return null;
			}
			_canvasManager.TextTutoDesktop.color = new Color (_canvasManager.TextTutoMobile.color.r, _canvasManager.TextTutoMobile.color.g, _canvasManager.TextTutoMobile.color.b, to);
			_canvasManager.TextTutoMobile.color = new Color (_canvasManager.TextTutoMobile.color.r, _canvasManager.TextTutoMobile.color.g, _canvasManager.TextTutoMobile.color.b, to);


			#if VS_SHARE
			VSSHARE.DOOpenScreenshotButton();
			#endif
		}

		public void ShowAds()
		{
			int count = PlayerPrefs.GetInt("GAMEOVER_COUNT",0);
			count++;
			PlayerPrefs.SetInt("GAMEOVER_COUNT",count);
			PlayerPrefs.Save();

			#if APPADVISORY_ADS
			if(count > numberOfPlayToShowInterstitial)
			{
			#if UNITY_EDITOR
			print("count = " + count + " > numberOfPlayToShowINterstitial = " + numberOfPlayToShowInterstitial);
			#endif
			if(AdsManager.instance.IsReadyInterstitial())
			{
			#if UNITY_EDITOR
				print("AdsManager.instance.IsReadyInterstitial() == true ----> SO ====> set count = 0 AND show interstial");
			#endif
				PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
				AdsManager.instance.ShowInterstitial();
			}
			else
			{
			#if UNITY_EDITOR
				print("AdsManager.instance.IsReadyInterstitial() == false");
			#endif
			}

		}
		else
		{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
		}
		PlayerPrefs.Save();
			#else
		if(count >= numberOfPlayToShowInterstitial)
		{
			Debug.LogWarning("To show ads, please have a look to Very Simple Ad on the Asset Store, or go to this link: " + VerySimpleAdsURL);
			Debug.LogWarning("Very Simple Ad is already implemented in this asset");
			Debug.LogWarning("Just import the package and you are ready to use it and monetize your game!");
			Debug.LogWarning("Very Simple Ad : " + VerySimpleAdsURL);
			PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
		}
		else
		{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
		}
		PlayerPrefs.Save();
			#endif
	}

}
}