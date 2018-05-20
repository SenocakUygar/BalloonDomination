using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Balloon : MonoBehaviour
{
	private bool exploded;
	private bool flying;
	private float height;
    private float growth;
	private float speed;
	private float critical;
    private float scaleX;
    private Color color;
    private Renderer rend;
    private AudioSource audioSource;

    [SerializeField]
	public GameObject explosion;

    [SerializeField]
    public AudioClip[] boomclips;

    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
        exploded = false;
		flying = false;
        setColor();
		transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
        audioSource = GetComponent<AudioSource>();
        color = GetComponent<Renderer>().material.color;
    }
	
	// Update is called once per frame
	void Update () {
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

    private void setColor()
	{
		rend.material.color = new Color(Random.value, Random.value, Random.value, 1.0f);
    }

    public void setTitan()
    {
        float alpha = color.a;
        color = new Color(0.753f, 0.753f, 0.753f, 1.0f);
        color.a = alpha;
        rend.material.color = color;
    }
		

	public void fill(){
        if (exploded) return;
        if (transform.localScale.x < critical)
        {
			color.a = 1.2f - (transform.localScale.x / critical);
			rend.material.color = color;

			this.transform.localScale = new Vector3(transform.localScale.x + (growth * Time.deltaTime), transform.localScale.y + (growth * Time.deltaTime), transform.localScale.z + (growth * Time.deltaTime));
        }
        else
        {
            explode();
        }
	}


	private void ascend(){
		if (transform.localPosition.y < height) {
			float newY = transform.position.y + (speed * Time.deltaTime);
			float newX = transform.position.x + (Mathf.Sin (Time.time) * Time.deltaTime * speed) * 0.5f;
			transform.position = new Vector3 (newX, newY, 0);
		} else {
			flying = false;
            setWithoutTransparent();
			transform.position = new Vector3 (Random.Range (-0.5f+ transform.position.x, 0.5f+ transform.position.x), transform.position.y, Random.Range(-0.2f, 0.2f));
		}
	}

	private void setWithoutTransparent(){
		color.a = 1.0f;
		rend.material.color = color;
	}

    public void explode()
    {
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        position.z -= 0.1f;
        position.y += 0.05f;
        this.explosion.transform.position = position;
        int ran = Random.Range(0, 5);
        audioSource.clip = boomclips[ran];
        audioSource.Play();
        Instantiate(this.explosion);
        exploded = true;
        setWithoutTransparent();
        this.transform.localScale = new Vector3(0.5f, 0.2f, 0.05f);
        transform.position = new Vector3(Random.Range(-1.0f + transform.position.x, 1.0f + transform.position.x), 0f, Random.Range(-0.2f, 0.1f));
    }
}
