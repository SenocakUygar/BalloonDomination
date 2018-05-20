using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CameraManagement : MonoBehaviour {
	//Camera bewegung
	public float speed;
	Vector3 targetPosition, targetPosition2;
	Vector3 lookAt;
	Vector3 lookAt2;
    bool movement;
    private GameObject panelGame;
    private List<float> points;

	public static string winner = "";

    [SerializeField]
    public AudioClip[] winnerclips;

    [SerializeField]
    public AudioClip endingclip;

    //Timer
    private Text timer;
	private GameObject newGame, extGame;
	public float timeLeft = 10.0f;
    public bool endgame = false;
	public bool cameraHilfe = false;


	// Use this for initialization
	void Start () {
		PlayerScript.spielEnde = false;
		this.timer = GameObject.Find("Timer").transform.FindChild("Text").GetComponent<Text>();
		this.newGame = GameObject.Find("newGame");
		this.extGame = GameObject.Find("menu");
		this.newGame.SetActive (false);
		this.extGame.SetActive (false);
		movement = false;
		speed = 1;
		targetPosition = new Vector3 (transform.position.x, 2.215f, -0.864f);
		targetPosition2 = new Vector3 (transform.position.x, 4f, -3f);
        lookAt = new Vector3(0f, 12.0f, 0f);
        lookAt2 = new Vector3 (0f, 0f, 5.0f);
        this.panelGame = GameObject.Find("PanelGame");
        this.panelGame.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		//Timer
		setTimer ();

		//Move Camera
		if (movement) {
			moveCamera ();
		}
	
	}

    void setTimer()
    {
        if (!endgame)
        {
        timeLeft -= Time.deltaTime;
        if (timeLeft > 0)
        {
            float minutes = Mathf.Floor(timeLeft / 60);
            float seconds = timeLeft % 60;
            this.timer.text = minutes + "  :  " + Mathf.RoundToInt(seconds);
            if (timeLeft < 10)
            {
                if (Mathf.Floor(timeLeft) % 2 == 0)
                    this.timer.color = new Color(1.0f, 1.0f, 1.0f);
                else
                    this.timer.color = Color.red;
            }
        }
        else
        {
				PlayerScript.spielEnde = true;
				this.endgame = true;
				moveCamera();
				GameObject[] playerOne = GameObject.FindGameObjectsWithTag("Player1");
				GameObject[] playerTwo = GameObject.FindGameObjectsWithTag("Player2");
                AudioSource[] audios = GetComponents<AudioSource>();
                if (playerOne [0].GetComponent<PlayerScript> ().scoreP1 > playerTwo [0].GetComponent<PlayerScript> ().scoreP2) {
					winner = "Player 1";
                    audios[1].clip = winnerclips[0];
                } else {
					winner = "Player 2";
                    audios[1].clip = winnerclips[1];
                }
                audios[1].Play();
				this.panelGame.SetActive(true);
				Text gameText = this.panelGame.transform.FindChild("GameText").GetComponent<Text>();
				gameText.text = "The winner is " + winner;
                audios[0].clip = endingclip;
                audios[0].Play();
        }
        } else
        {
            moveCamera();
        }
    }

	void moveCamera(){
		float step = speed * Time.deltaTime;
		if (transform.position == targetPosition)
			cameraHilfe = true;
		//Ende Spiel Camera bewegeung beendet.
		if (transform.position == targetPosition2 && cameraHilfe) {
			this.newGame.SetActive (true);
			this.extGame.SetActive (true);
		}
		if (!cameraHilfe) {
			transform.position = Vector3.MoveTowards (transform.position, targetPosition, step);
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (lookAt), Time.deltaTime);
		} else {
			
			transform.position = Vector3.MoveTowards (transform.position, targetPosition2, step);
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (lookAt2), Time.deltaTime);
		}
	}

}
