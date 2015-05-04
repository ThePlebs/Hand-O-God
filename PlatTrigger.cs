using UnityEngine;
using System.Collections;

public class PlatTrigger : MonoBehaviour {

    public GameObject movingPl;
    public Collision2D colli;
    Collider2D colldr;
    MovingPlatform movingScrpt;

	// Use this for initialization
	void Start () {
        colldr = movingPl.GetComponent<Collider2D>();
        movingScrpt = movingPl.GetComponent<MovingPlatform>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Moving_Platform") {
            movingScrpt.velocity = -movingScrpt.velocity;
        }
    }
}
