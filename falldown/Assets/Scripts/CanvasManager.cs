
/***********************************************************************************************************
 * Produced by App Advisory	- http://app-advisory.com													   *
 * Facebook: https://facebook.com/appadvisory															   *
 * Contact us: https://appadvisory.zendesk.com/hc/en-us/requests/new									   *
 * App Advisory Unity Asset Store catalog: http://u3d.as/9cs											   *
 * Developed by Gilbert Anthony Barouch - https://www.linkedin.com/in/ganbarouch                           *
 ***********************************************************************************************************/


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AppAdvisory.XtremNoBrakes
{
	public class CanvasManager : MonoBehaviour 
	{
		public Text textPoint;

		public CanvasGroup menu;

		Button buttonSound;
		Button buttonPlay;
		Button buttonRate;
		Button buttonFacebook;

		public Text lastScore;
		public Text bestScore;

		public Text TextTutoDesktop;
		public Text TextTutoMobile;

		GameManager _gameManager;
		CanvasManager _canvasManager;

		void Awake()
		{
			_gameManager = FindObjectOfType<GameManager>();
			_canvasManager = FindObjectOfType<CanvasManager>();

			menu.gameObject.SetActive (true);
		}

		void Start()
		{
			buttonSound = menu.transform.FindChild ("ButtonSound").GetComponent<Button>();
			buttonPlay = menu.transform.FindChild ("ButtonPlay").GetComponent<Button>();
			buttonRate = menu.transform.FindChild ("ButtonRate").GetComponent<Button>();
			buttonFacebook = menu.transform.FindChild ("ButtonFacebook").GetComponent<Button>();

			buttonSound.onClick.AddListener (OnClickedSoundButton);
			buttonPlay.onClick.AddListener (OnClickedPlayButton);
			buttonRate.onClick.AddListener (OnClickedRateButton);
			buttonFacebook.onClick.AddListener (OnClickedFacebookButton);

			lastScore = menu.transform.FindChild ("LastScore").GetComponentInChildren<Text> ();
			bestScore = menu.transform.FindChild ("BestScore").GetComponentInChildren<Text> ();

			SetScoresCanvas ();

			SetSoundIcon ();

			TextTutoDesktop = transform.FindChild ("TextTutoDesktop").GetComponentInChildren<Text> ();
			TextTutoMobile = transform.FindChild ("TextTutoMobile").GetComponentInChildren<Text> ();

			TextTutoDesktop.gameObject.SetActive (false);
			TextTutoMobile.gameObject.SetActive (false);
		}

		public void SetScoresCanvas()
		{
			int last = PlayerPrefs.GetInt ("LAST_SCORE", 0);
			int best = PlayerPrefs.GetInt ("BEST_SCORE", 0);

			lastScore.text = "LAST\nSCORE\n" + last.ToString ();
			bestScore.text = "BEST\nSCORE\n" + best.ToString ();
		}

		void Update()
		{

			bool alphaIsOne = _canvasManager.menu.alpha == 1;
			_canvasManager.menu.blocksRaycasts = alphaIsOne;
			textPoint.gameObject.SetActive (!alphaIsOne);

		}

		void OnClickedSoundButton()
		{
			int soundOn = PlayerPrefs.GetInt ("SOUND", 1);

			if (soundOn == 1)
				TurnSoundOff ();
			else
				TurnSoundOn ();

			SetSoundIcon ();
		}

		void SetSoundIcon()
		{
			int soundOn = PlayerPrefs.GetInt ("SOUND", 1);

			buttonSound.transform.GetChild (0).gameObject.SetActive (soundOn == 0);
			buttonSound.transform.GetChild (1).gameObject.SetActive (soundOn == 1);
		}

		void TurnSoundOn()
		{
			PlayerPrefs.SetInt ("SOUND", 1);
			PlayerPrefs.Save ();
		}

		void TurnSoundOff()
		{
			PlayerPrefs.SetInt ("SOUND", 0);
			PlayerPrefs.Save ();
		}

		void OnClickedPlayButton()
		{
			_gameManager.StartGame ();
		}

		void OnClickedRateButton()
		{
		}

		void OnClickedFacebookButton()
		{
		}

		public void SetPoint(int point)
		{
			textPoint.text = point.ToString ();
		}
	}
}