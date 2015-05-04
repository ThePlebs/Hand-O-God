using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour {

    public GameObject player;
    MoveScript3 movescprt;
    public float velocity = 0f;
    Vector2 pos;
    float vloc;

	// Use this for initialization
	void Start () {
        movescprt = player.GetComponent<MoveScript3>();
        pos = rigidbody2D.position;
    }
	
	// Update is called once per frame
	void Update () {
        rigidbody2D.velocity = new Vector2(velocity, rigidbody2D.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Plat_Trigger") {
            velocity = -velocity;
        }
    }

    void OnCollisionStay2D(Collision2D coll) {
        movescprt.vloc = rigidbody2D.velocity.x;
    }
}
