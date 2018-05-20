using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointsSeeker : MonoBehaviour {

    public bool seeking = false;
    private GameObject target;
    public float distance = 0.5f;
    public float speed = 1f;

    private Text scoreText;
    private float score;
    private float points;

    private GameObject scoreObj;

    // Use this for initialization
    void Start()
    {
		points = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (seeking && target != null)
        {
            //transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, speed * Time.deltaTime, 0.0f);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if ((transform.position - target.transform.position).magnitude < distance)
            {
                scoreText = scoreObj.transform.FindChild("ScoreText").GetComponent<Text>();
                scoreText.text = "Score: " +  this.score;
                Destroy(gameObject);
            }
        }
    }

    public void seek(GameObject obj)
    {
        this.target = obj;
        seeking = true;
    }

    public void setScore(float score)
    {
        this.score = score;
    }

    public void setPoints(float points)
    {
        this.points = points;
        transform.GetComponent<TextMesh>().text = points + "";
    }

    public void setScoreObj(GameObject obj)
    {
        this.scoreObj = obj;
    }
}
