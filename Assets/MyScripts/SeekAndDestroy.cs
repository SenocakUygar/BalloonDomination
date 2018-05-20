using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekAndDestroy : MonoBehaviour {

    public bool outFortheKill = false;
    private GameObject target;
    public float distance = 0.5f;
    public float speed = 4f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (outFortheKill && target != null)
        {
            transform.forward = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, speed * Time.deltaTime, 0.0f);
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if ((transform.position - target.transform.position).magnitude < distance)
            {
                target.GetComponent<Balloon>().explode();
                Destroy(gameObject);
            }
        }
	}

    public void kill(GameObject balloon)
    {
        this.target = balloon;
        outFortheKill = true;
    }
}
