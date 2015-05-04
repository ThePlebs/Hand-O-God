using UnityEngine;
using System.Collections;

public class SpikeTrigger : MonoBehaviour {

    MoveScript3 movescrpt;
    Collider2D col;
    public GameObject player;

	// Use this for initialization
	void Start () {
        //movescrpt = GetComponent<MoveScript3>();
        movescrpt = player.GetComponent<MoveScript3>();
        col = player.GetComponent<Collider2D>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other == col)
            movescrpt.dead = true;
    }
}
