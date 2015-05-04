using UnityEngine;
using System.Collections;

public class Crusher : MonoBehaviour {

    public GameObject player;
    MoveScript3 movescrpt;

    public float accelerationRate = 0f;
    public float stationTime = 1f;
    public float maxSpeed = 1f;
    float ac = 0f;
    public float velocity = 0f;
    bool station = false;
    float timer = 0f;

    RaycastHit2D[] playerCheck = new RaycastHit2D[3];
    public LayerMask isPLayer;

	// Use this for initialization
	void Start () {
        movescrpt = player.GetComponent<MoveScript3>();
    }
	
	// Update is called once per frame
	void Update () {

        playerCheck[0] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, collider2D.bounds.min.y), -Vector2.up, Mathf.Infinity, isPLayer);
        playerCheck[1] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x + collider2D.bounds.size.x / 2, collider2D.bounds.min.y), -Vector2.up, Mathf.Infinity, isPLayer);
        playerCheck[2] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, collider2D.bounds.min.y), -Vector2.up, Mathf.Infinity, isPLayer);


        if (station) {
            timer += Time.deltaTime;
            accelerationRate = 0f;
            velocity = 0f;
            if (timer > stationTime) {
                station = false;
                timer = 0f;
                accelerationRate = -ac;
            }
        }
        else {
            ac = accelerationRate;
            velocity += accelerationRate;
            if (accelerationRate < 0) {
                if (velocity < -maxSpeed)
                    velocity = -maxSpeed;
            }
            if (accelerationRate > 0) {
                if (velocity > maxSpeed)
                    velocity = maxSpeed;
            }
        }
        //Debug.Log(station);
	}

    bool _playerCheck() {
        bool B = false;

        for (int i = 0; i < 3; i++) {
            if (playerCheck[i].collider != null)
                B = true;
        }

        return B;
    }

    void FixedUpdate() {
        rigidbody2D.velocity = new Vector2(0, velocity);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        if (coll.gameObject.tag == "Crusher_Trigger") {
            station = true;
        }
        if (coll.gameObject.tag == "Player") {
            if (rigidbody2D.velocity.y < 0 && movescrpt.crusherAbove && movescrpt.grounded) {
                movescrpt.dead = true;
            }
        }
    }
}
