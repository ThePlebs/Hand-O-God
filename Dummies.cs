using UnityEngine;
using System.Collections;

public class Dummies : MonoBehaviour {

    BoxCollider2D attackColl;

    bool isLeft = true;
    public float velocity = 0f;
    public float drag = 0f;
    bool canTakeDamage = true;
    AttackCollider attColScrpt;
    MoveScript3 movescrpt;
    public GameObject Player;
    public GameObject Attack_Collider;

    float distanceX, distanceY;

	// Use this for initialization
	void Start () {
        attColScrpt = Player.GetComponent<AttackCollider>();
        attackColl = Attack_Collider.GetComponent<BoxCollider2D>();
        movescrpt = Player.GetComponent<MoveScript3>();
	}
	
	// Update is called once per frame
	void Update () {
        if (isLeft) {
            velocity += drag;
            if (velocity > 0)
                velocity = 0;
        }
        else {
            velocity -= drag;
            if (velocity < 0) {
                velocity = 0;
            }
        }
        if (!Attack_Collider.collider2D.enabled)
            canTakeDamage = true;
        if (Attack_Collider.collider2D.enabled) {
            //canTakeDamage = true;
            distanceX = Mathf.Abs(this.collider2D.bounds.center.x - attackColl.bounds.center.x);
            distanceY = Mathf.Abs(this.collider2D.bounds.center.y - attackColl.bounds.center.y);
            if (distanceX <= (this.collider2D.bounds.size.x / 2 + attackColl.bounds.size.x / 2) && distanceY <= (this.collider2D.bounds.size.y / 2 + attackColl.bounds.size.y / 2)) {
                if (canTakeDamage) {
                    velocity = movescrpt.isLeft ? -20 : 20;
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 20);
                }
            }
            canTakeDamage = false;
        }

    }

    void FixedUpdate() {
        rigidbody2D.velocity = new Vector2(velocity, rigidbody2D.velocity.y);
        if (rigidbody2D.velocity.x > 0 && isLeft)
            _flip();
        if (rigidbody2D.velocity.x < 0 && !isLeft)
            _flip();
        Debug.Log(rigidbody2D.velocity.x);
    }

    void _flip() {
        isLeft = !isLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
}
