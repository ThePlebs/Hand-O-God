using UnityEngine;
using System.Collections;

public class AttackCollider : MonoBehaviour {

    Attacks attackscrpt;
    public GameObject Player;

	// Use this for initialization
	void Start () {
        attackscrpt = Player.GetComponent<Attacks>();
	}
	
	// Update is called once per frame
	void Update () {
        this.collider2D.enabled = false;
        if (attackscrpt.timer >= 0.15 && attackscrpt.timer <= 0.35)
            this.collider2D.enabled = true;
	}
}
