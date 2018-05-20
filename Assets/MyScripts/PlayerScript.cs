using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerScript: MonoBehaviour
{
	public static bool spielEnde = false;

    public int comboStart = 75;
    public float scoreP1, scoreP2;
	private float critical;
    private Balloon balloon;
    public GameObject balloonObj;
	private List<GameObject> balloonList;
	private bool endeBallonExploxionHilfe;
    private GameObject panelScoreP1;
	private GameObject panelScoreP2;
    private Text panelPowerP1;
    private Text panelPowerP2;
    private Text panelComboP1, panelComboP2;
    private AudioSource audioCombo;
    private AudioSource audioPower;
    private AudioSource audioPoints;
    private GameObject currentPoints;
    private bool powerStop = false;
    private bool showBlink = false;
    private string temp;

    public GameObject specialer;

    [SerializeField]
    public GameObject needlePrefab;

    [SerializeField]
	public GameObject explosion;


    [SerializeField]
    public GameObject prefab;

    public GameObject P1Points;
    public GameObject P2Points;

    private GameObject P1PointsLoc;
    private GameObject P2PointsLoc;

    public GameObject[] comboCubes;
    public GameObject[] powerCubes;

    public AudioClip pointClip;

    public bool PlayerOne;
    public float growthRate = 1.0f;
    public float criticalUpperLimit = 1.4f;
    public float criticalLowerLimit = 1.0f;
    public float heightUpperLimit = 6.0f;
    public float heightLowerLimit = 3.5f;
    public float speed = 1.0f;
    public int pointsThreshhold = 60;
    public int combo = 1;
    public int comboCounter = 0;
    public int powerCounter = 0;
    public bool powerUp = false;
    public bool titanium = false;
    private string powerString = "";

	private Coroutine corotine;
	//private Coroutine corotineP2;

    [SerializeField]
    public AudioClip[] comboclips;

    [SerializeField]
    public AudioClip[] powerupclips;

    [SerializeField]
    public AudioClip[] boomclips;


    // Use this for initialization
    void Start() {
        this.combo = 1;
        this.comboCounter = 0;
        this.powerCounter = 0;
        this.scoreP1 = 0;
		this.scoreP2 = 0;
        this.powerUp = false;
        this.titanium = false;
		balloonList = new List<GameObject> ();
        checkNext();
        this.panelScoreP1 = GameObject.Find("PanelScoreP1");
		this.panelScoreP2 = GameObject.Find("PanelScoreP2");
        this.P1PointsLoc = GameObject.Find("P1PointsLoc");
        this.P2PointsLoc = GameObject.Find("P2PointsLoc");
        this.panelComboP1 = GameObject.Find("PanelComboP1").transform.FindChild("ComboText").GetComponent<Text>();
		this.panelComboP2 = GameObject.Find("PanelComboP2").transform.FindChild("ComboText").GetComponent<Text>();
        this.panelPowerP1 = GameObject.Find("PanelPowerP1").transform.FindChild("PowerText").GetComponent<Text>();
        this.panelPowerP2 = GameObject.Find("PanelPowerP2").transform.FindChild("PowerText").GetComponent<Text>();
        AudioSource[] audios = GetComponents<AudioSource>();
        audioCombo = audios[0];
        audioPower = audios[1];
        audioPoints = audios[2];
        audioPoints.clip = pointClip;

        endeBallonExploxionHilfe = false;
    }

    // Update is called once per frame
    void Update() {
		if (!spielEnde) {
			if (PlayerOne) {
				if (Input.GetKey (KeyCode.Space) && !this.balloon.getExploded ())
					fill ();
				if (Input.GetKeyUp (KeyCode.Space)) {
					if (!powerUp) {
						this.panelPowerP1.text = "PowerUP:";
						if (corotine != null) {
							StopCoroutine (corotine);
							this.panelPowerP1.color = new Color32(50, 50, 50, 255);
						}
					}
                    if (!this.balloon.getExploded ())
						fly ();
                    currentPoints = Instantiate(P1Points, new Vector3(transform.position.x, transform.position.y, transform.position.z -0.1f), Quaternion.identity);
                    this.scoreP1 += setScore (panelComboP1, panelPowerP1);
                    //set score of instantiated object
                    currentPoints.GetComponent<PointsSeeker>().setScore(scoreP1);
                    //seek score
                    currentPoints.GetComponent<PointsSeeker>().setScoreObj(panelScoreP1);
                    currentPoints.GetComponent<PointsSeeker>().seek(P1PointsLoc);
                    //panelScoreP1.text = "Total Score: " + scoreP1;
                    checkNext();
					this.titanium = false;
				}
				if (Input.GetKey (KeyCode.LeftControl) && !this.balloon.getExploded () && this.powerUp) {
                    showBlink = false;
                    panelPowerP1.color = Color.black;
                    specialer.SetActive(false);
                    if (powerString.Equals("Titan"))
                    {
                        audioPower.clip = powerupclips[3];
                        audioPower.Play();
                        this.critical = 4.0f;
                        this.balloon.setCritical(critical);
                        this.balloon.setTitan();
                        this.titanium = true;
                    } else
                    {
                        //Needle
                        audioPower.clip = powerupclips[2];
                        audioPower.Play();
                        Vector3 spawnPos = new Vector3(transform.position.x-1, transform.position.x, transform.position.z-1);
                        GameObject needle = Instantiate(needlePrefab);
                        needle.transform.position = spawnPos;
                        needle.GetComponent<SeekAndDestroy>().kill(GameObject.Find("Player 2").GetComponent<PlayerScript>().balloonObj);
                        powerStop = false;
                    }
					this.panelPowerP1.text = "PowerUP: ";
                    this.powerUp = false;
                    this.powerCounter = 0;
                    //cubes zerstoeren
                    foreach (GameObject powerCube in powerCubes)
                    {
                        powerCube.SetActive(false);
                    }
                }
			} else {
				if (Input.GetKey (KeyCode.RightShift) && !this.balloon.getExploded ())
					fill ();
				if (Input.GetKeyUp (KeyCode.RightShift)) {
					if (!this.balloon.getExploded ())
						fly ();
					if (!powerUp) {
						this.panelPowerP2.text = "PowerUP:";
						if (corotine != null) {
							this.panelPowerP2.color = new Color32(50, 50, 50, 255);
							StopCoroutine (corotine);
						}
					}
                    currentPoints = Instantiate(P2Points, new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f), Quaternion.identity);
                    this.scoreP2 += setScore (panelComboP2, panelPowerP2);
                    //set score of instantiated object
                    currentPoints.GetComponent<PointsSeeker>().setScore(scoreP2);
                    //seek score
                    currentPoints.GetComponent<PointsSeeker>().setScoreObj(panelScoreP2);
                    currentPoints.GetComponent<PointsSeeker>().seek(P2PointsLoc);
                    //panelScoreP2.text = "Total Score: " + scoreP2;
                    checkNext ();
					this.titanium = false;
				}
				if (Input.GetKey (KeyCode.Return) && !this.balloon.getExploded () && this.powerUp) {
                    
                    showBlink = false;
                    panelPowerP2.color = Color.black;
                    specialer.SetActive(false);
                    if (powerString.Equals("Titan"))
                    {
                        audioPower.clip = powerupclips[3];
                        audioPower.Play();
                        this.critical = 4.0f;
                        this.balloon.setCritical(critical);
                        this.balloon.setTitan();
                        this.titanium = true;
                    }
                    else
                    {
                        //Needle
                        audioPower.clip = powerupclips[2];
                        audioPower.Play();
                        Vector3 spawnPos = new Vector3(transform.position.x + 1, transform.position.x, transform.position.z - 1);
                        GameObject needle = Instantiate(needlePrefab);
                        needle.transform.position = spawnPos;
                        needle.GetComponent<SeekAndDestroy>().kill(GameObject.Find("Player 1").GetComponent<PlayerScript>().balloonObj);
                        powerStop = false;
                    }
                    this.titanium = true;
					this.panelPowerP2.text = "PowerUP: ";

					this.powerUp = false;
                    this.powerCounter = 0;
                    //cubes zerstoeren
                    foreach (GameObject powerCube in powerCubes)
                    {
                        powerCube.SetActive(false);
                    }
                }
			}
		} else {
			if (!endeBallonExploxionHilfe) {
				if (CameraManagement.winner == "Player 1" && !PlayerOne) {
					StartCoroutine (balloonsVonVerliereExplodieren ());
				}
				if (CameraManagement.winner == "Player 2" && PlayerOne) {
					StartCoroutine (balloonsVonVerliereExplodieren ());
				}
				endeBallonExploxionHilfe = true;
			}
		}

    }
    
    private void checkNext()
    {
        this.critical = Random.Range(criticalLowerLimit, criticalUpperLimit);
        this.balloonObj = (GameObject)Instantiate(prefab, transform.position, Quaternion.identity);
        this.balloonObj.transform.rotation = transform.rotation;
        this.balloon = this.balloonObj.GetComponent<Balloon>();
        this.balloon.setHeight(Random.Range(heightLowerLimit, heightUpperLimit));
        this.balloon.setSpeed(speed);
        this.balloon.setGrowth(growthRate);
        this.balloon.setCritical(critical);

		balloonList.Add (this.balloonObj);
    }

    private void fly()
    {
        this.balloon.setFlying(true);
    }

    private void fill()
    {
        this.balloon.fill();
    }

	private float setScore(Text panelCombo, Text panelPower){
		float points = 0, score = 0;
        if (!this.balloon.getExploded())
        {
            points = Mathf.Round((this.balloonObj.transform.localScale.x / this.critical) * 100.0f);
            if (points > 100) points = 100;
            if (this.critical == 4.0f) points = points * 4;
            if ( points < pointsThreshhold && this.powerCounter < 21 && !powerStop)
            {
                int index = powerCounter;
                if (powerCounter > 10)
                {
                    index = powerCounter - 11;
                }
                powerCubes[index].SetActive(true);
                this.powerCounter++;
            }  else
            {
                if (this.powerCounter > 10 && !powerStop) this.powerCounter = 11;
                else if (!powerStop) this.powerCounter = 0;
                //cubes zerstoeren
                foreach (GameObject powerCube in powerCubes)
                {
                    powerCube.SetActive(false);
                }
            }
        }
        else
        {
            this.combo = 1;
            this.comboCounter = 0;
            //cubes zerstoeren
            foreach (GameObject comboCube in comboCubes)
            {
                comboCube.SetActive(false);
            }
            if (this.powerCounter > 10) this.powerCounter = 11;
            else this.powerCounter = 0;
            //cubes zerstoeren
            foreach (GameObject powerCube in powerCubes)
            {
                powerCube.SetActive(false);
            }
        }
        //Debug.Log(points);
        if (points < pointsThreshhold)
        {
            points = 0;
        }
        else if (points >= comboStart)
        {
            comboCubes[comboCounter].SetActive(true);
			float screenWidth = Screen.width;
			//buttonPositionWidth = screenWidth / 2;

			float screenHeight = Screen.height;
			this.comboCounter++;
        }
        if (this.comboCounter > 4)
        {
            this.comboCounter = 0;
            //cubes zerstoeren
            foreach (GameObject comboCube in comboCubes)
            {
                comboCube.SetActive(false);
            }
            this.combo++;
            if (this.combo > 6)
            {
                audioCombo.clip = comboclips[4];
                audioCombo.Play();
            } else if (this.combo > 1)
            {
                audioCombo.clip = comboclips[this.combo-2];
                audioCombo.Play();
            }
        }
        if (this.powerCounter == 10)
        {
            if (PlayerOne)
                audioPower.clip = powerupclips[0];
            else
                audioPower.clip = powerupclips[1];
            audioPower.Play();
            this.powerString = "Titan";
            temp = "PowerUP: " + powerString + " ready!";
            panelPower.text = temp;
            showBlink = true;
            corotine = StartCoroutine(BlinkText(panelPower));
			panelPower.color = Color.green;
            this.powerUp = true;
            this.powerCounter++;
            specialer.SetActive(true);
            foreach (GameObject powerCube in powerCubes)
            {
                powerCube.SetActive(false);
            }
        } else if (this.powerCounter == 21)
        {
            if (PlayerOne)
                audioPower.clip = powerupclips[4];
            else
                audioPower.clip = powerupclips[5];
            audioPower.Play();
            this.powerString = "Needle";
            temp = "PowerUP: " + powerString + " ready!";
            panelPower.text = temp;
			panelPower.color = Color.red;
            this.powerUp = true;
            this.powerCounter++;
            specialer.SetActive(true);
            foreach (GameObject powerCube in powerCubes)
            {
                powerCube.SetActive(false);
            }
            powerStop = true;
        }
        if (points > pointsThreshhold) audioPoints.Play();
        points = points * this.combo;
       	panelCombo.text = "Combo: x" + this.combo;
        //panelPoints.text = "Points: " + points;
		score = points;
        //set points of instantiated object
        currentPoints.GetComponent<PointsSeeker>().setPoints(points);
        return score;
	}
		
	public IEnumerator balloonsVonVerliereExplodieren()
	{
        int ran = Random.Range(0, 6);
        audioCombo.clip = boomclips[ran];
        audioCombo.Play();
        foreach (GameObject bal in this.balloonList) {
			if (bal != null) {
                bal.GetComponent<Balloon>().explode();
                yield return new WaitForSeconds (0.2f);
			}
		}
		CameraManagement.winner = "";
	}

	//function to blink the text
	public IEnumerator BlinkText(Text panelPower){
		//blink it forever. You can set a terminating condition depending upon your requirement
		while (this.showBlink) {
			//set the Text's text to blank
			panelPower.text = "";
			//display blank text for 0.5 seconds
			yield return new WaitForSeconds (0.1f);
			//display “I AM FLASHING TEXT” for the next 0.5 seconds
			panelPower.text = temp;
			yield return new WaitForSeconds (0.5f);
		}
	}		
}