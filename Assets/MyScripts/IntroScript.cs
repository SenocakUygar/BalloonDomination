using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    private float critical;
    private BalloonIntro balloon;
    private GameObject balloonObj;

    [SerializeField]
    public GameObject prefab;
    public GameObject easterEgg;
    public float growthRate = 1.0f;
    public float criticalUpperLimit = 1.4f;
    public float criticalLowerLimit = 1.0f;
    public float speed = 1.0f;
    public bool easter = false;

    private int counter = 0;

    // Use this for initialization
    void Start()
    {
        checkNext();
    }

    // Update is called once per frame
    void Update()
    {
            if (!this.balloon.getExploded()) fill();
            if (this.balloonObj.transform.localScale.x > this.critical - 0.2)
            {
                if (!this.balloon.getExploded()) fly();
                checkNext();
            }
    }

    private void checkNext()
    {
        this.critical = Random.Range(criticalLowerLimit, criticalUpperLimit);
        this.balloonObj = (GameObject)Instantiate(prefab, this.transform.position, Quaternion.identity);
        this.balloonObj.transform.position = this.transform.position;
        this.balloonObj.transform.rotation = this.transform.rotation;
        this.balloon = this.balloonObj.GetComponent<BalloonIntro>();
        this.balloon.setSpeed(speed);
        this.balloon.setGrowth(growthRate);
        this.balloon.setCritical(critical);
        counter++;
        if (counter % 9 == 0 && easter)
        {
            GameObject gorilla = Instantiate(easterEgg, new Vector3(transform.position.x, transform.position.y - 2, transform.position.z), Quaternion.identity);
            gorilla.transform.parent = this.balloonObj.transform;
        }
    }

    private void fly()
    {
        this.balloon.setFlying(true);
    }

    private void fill()
    {
        this.balloon.fill();
    }
}