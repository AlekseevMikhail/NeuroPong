using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private int playerScore1 = 0;
	private int playerScore2 = 0;
	private int singlePlayerScore = 0;
	private int singlePlayerHighScore = 0;
	private bool pongModeEnabled = true;

	public enum SoundMode { Silence, Sounds, Music};

	private GameObject theBall;
	private GameObject allPongObjects;
	private GameObject allEMDRObjects;
	private Text leftScoreText;
	private Text rightScoreText;
	private Text singlePlayerScoreText;
	private Text singlePlayerHighScoreText;
	private Button bgMusicButton;
	private Button gameModeButton;
	public AudioSource bgMusic;
	public SoundMode mySoundMode;

	// Use this for initialization
	void Start () {
		
		theBall = GameObject.FindGameObjectWithTag("Ball");
		allPongObjects = GameObject.Find("PongObjects");
		allEMDRObjects = GameObject.Find("EMDRObjects");
		leftScoreText = GameObject.Find("LeftScore").GetComponent<Text>();
		rightScoreText = GameObject.Find("RightScore").GetComponent<Text>();
		singlePlayerScoreText = GameObject.Find("SinglePlayerScore").GetComponent<Text>();
		singlePlayerHighScoreText = GameObject.Find("SinglePlayerHighScore").GetComponent<Text>();
		bgMusicButton = GameObject.Find("MusicToggle").GetComponent<Button>();
		gameModeButton = GameObject.Find("GameModeToggle").GetComponent<Button>();
		mySoundMode = SoundMode.Silence;
		AudioSource[] sounds = GetComponents<AudioSource>();
		bgMusic = sounds[0];

		UpdateUIBasedOnGameMode();
		UpdateBgMusicButtonText();
		UpdateGameModeButtonText();
		UpdatePlayerScoreUI();
	}

	public void Score(string wallID) {
		if (wallID == "RightWall") {
			playerScore1++;
		} else {
			playerScore2++;
		}
		singlePlayerScore = 0;
		UpdatePlayerScoreUI();
	}

	public void IncrementSinglePlayerScore()
    {
		singlePlayerScore++;
		if (singlePlayerScore > singlePlayerHighScore)
        {
			singlePlayerHighScore = singlePlayerScore;
        }
		UpdatePlayerScoreUI();
	}

	void OnGUI() {
		/*
		if (playerScore1 == 10) {
			GUI.Label (new Rect (Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS");
			theBall.SendMessage ("ResetBall", null, SendMessageOptions.RequireReceiver);
		} else if (playerScore2 == 10) {
			GUI.Label (new Rect (Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER TWO WINS");
			theBall.SendMessage ("ResetBall", null, SendMessageOptions.RequireReceiver);
		}
		*/
	}

    public void ResetScores()
    {
		playerScore1 = 0;
		playerScore2 = 0;
		singlePlayerScore = 0;
		UpdatePlayerScoreUI();
	}

	public void ToggleBgMusic()
    {
		if (bgMusic == null)
        {
			return;
        }

		int mode = (((int)mySoundMode) + 1) % SoundMode.GetValues(typeof(SoundMode)).Length;
		mySoundMode = (SoundMode)mode;

		if (mySoundMode == SoundMode.Music)
		{
			bgMusic.Play();
		}
        else
        {
			bgMusic.Stop();
        }

		UpdateBgMusicButtonText();
	}

	public void ToggleGameMode()
    {
		pongModeEnabled = !pongModeEnabled;
		UpdateUIBasedOnGameMode();
		UpdateGameModeButtonText();
	}

	private void UpdateUIBasedOnGameMode()
    {
		leftScoreText.gameObject.SetActive(false);
		rightScoreText.gameObject.SetActive(false);
		singlePlayerScoreText.gameObject.SetActive(pongModeEnabled);
		singlePlayerHighScoreText.gameObject.SetActive(pongModeEnabled);
		allPongObjects.SetActive(pongModeEnabled);
		allEMDRObjects.SetActive(!pongModeEnabled);
	}

	private void UpdateBgMusicButtonText()
    {
		int nextMode = (((int)mySoundMode) + 1) % SoundMode.GetValues(typeof(SoundMode)).Length;
		string buttonText = ((SoundMode)nextMode).ToString();
		bgMusicButton.GetComponentInChildren<Text>().text = buttonText;
	}

	private void UpdateGameModeButtonText()
    {
		string buttonText = pongModeEnabled ? "EMDR" : "Pong";
		gameModeButton.GetComponentInChildren<Text>().text = buttonText;
	}

	private void UpdatePlayerScoreUI()
    {
		leftScoreText.text = playerScore1.ToString();
		rightScoreText.text = playerScore2.ToString();
		singlePlayerScoreText.text = "Your Score: " + singlePlayerScore.ToString();
		singlePlayerHighScoreText.text = "High Score: " + singlePlayerHighScore.ToString();
	}

	public bool GetPongModeEnabled()
    {
		return pongModeEnabled;
    }
}
