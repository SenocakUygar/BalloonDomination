using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class BalloonIntro : MonoBehaviour
{
    private bool exploded;
    private bool flying;
    public float height;
    private float growth;
    private float speed;
    private float critical;
    private float scaleX;

    [SerializeField]
    public GameObject explosion;

    // Use this for initialization
    void Start()
    {
        exploded = false;
        flying = false;
        setColor();
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (flying && !exploded) ascend();
    }

    public void setGrowth(float growth)
    {
        this.growth = growth;
    }
    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public void setHeight(float height)
    {
        this.height = height;
    }

    public void setFlying(bool flying)
    {
        this.flying = flying;
    }

    public bool getExploded()
    {
        return this.exploded;

    }
    public void setCritical(float critical)
    {
        this.critical = critical;
    }

    void setColor()
    {
        Renderer rend = GetComponent<Renderer>();
        //rend.material.shader = Shader.Find("Transparent/Diffuse");
        rend.material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
    }


    public void fill()
    {
        if (exploded) return;
        if (transform.localScale.x < critical)
        {

            this.transform.localScale = new Vector3(transform.localScale.x + (growth * Time.deltaTime), transform.localScale.y + (growth * Time.deltaTime), transform.localScale.z + (growth * Time.deltaTime));
        }
    }


    private void ascend()
    {
        if (transform.localPosition.y < height)
        {
            float newY = this.transform.position.y + (speed * Time.deltaTime);
            float newX = this.transform.position.x + (Mathf.Sin(Time.time) * Time.deltaTime * speed) * 0.5f;
            transform.position = new Vector3(newX, newY, this.transform.localPosition.z);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void explode()
    {
        Instantiate(this.explosion);
    }
}
