using UnityEngine;
using System.Collections;

public class MoveScript3 : MonoBehaviour {

    //ADD
    public GameObject movingPlatform;
    public bool MP = false;
    public float vloc = 0f;
    public LayerMask isCrusher;
    public bool crusherAbove = false;
    //ADD - END

    public float gravity = 0f;

    public bool doubleJump = false;
    public float jumpForce = 20f;
    bool jumpButtonhit = false;
    bool jumpButtonB4 = false;
    public bool jump = false;
    float jumpBuffer = 0f;
    int jumpCount = 2;

    public float runSpeed = 10f;
    public float accelerationRate = 0.5f;
    public bool grounded = false;
    public bool dead = false;
    public bool locked = false;

    public float moveRAW;
    public bool isLeft = true;

    float drag_x;
    public float velocity = 0f;

    Vector2 position;
    Vector2 spawnPoint;
    Vector2 dPosition;

    RaycastHit2D[] hitIformationRIGHT = new RaycastHit2D[3];
    RaycastHit2D[] hitInformationLEFT = new RaycastHit2D[3];
    RaycastHit2D[] hitInformationDOWN = new RaycastHit2D[3];
    RaycastHit2D[] hitInformationUP = new RaycastHit2D[2];
    Vector2 rayPositionRIGHT;
    Vector2 rayPositionLEFT;
    Vector2 rayPositionDOWN;

    public LayerMask isWall;
    public float edgeLimmit = 0.01f;

	// Use this for initialization
	void Start () {
        position = rigidbody2D.position;
        rigidbody2D.gravityScale = gravity;
        spawnPoint = rigidbody2D.position;
	}
	
	// Update is called once per frame
	void Update () {
        crusherAbove = _crusherCheck();
        drag_x = accelerationRate;
        position = rigidbody2D.position;
        moveRAW = locked ? 0 : Input.GetAxisRaw("Horizontal");
        _setRayInformation();

        grounded = _hitDistance_DOWN() < edgeLimmit;

        if (grounded) {
            jumpBuffer = 0f;
            jumpCount = 2;
        }


        if (!dead) {
            velocity += moveRAW * accelerationRate;

            if (!locked) {
                if (Mathf.Abs(velocity) > runSpeed) {
                    velocity = moveRAW * runSpeed;
                }
                //JUMPING_CONTROLS
                if (Input.GetKey(KeyCode.Space) && !jumpButtonhit) {
                    if (!doubleJump) {
                        if (grounded)
                            jump = true;
                    }
                    else if (doubleJump) {
                        if (jumpCount != 0) {
                            jump = true;
                        }
                    }
                }
                jumpButtonhit = Input.GetKey(KeyCode.Space);
                //JUMPING_CONTROLS - END
            }
            else {

            }
            dPosition = transform.position;
        }
        else {
            _Die();
            transform.position = dPosition;
            rigidbody2D.velocity = new Vector2(0, 0);
            if (Input.GetKey(KeyCode.R))
                _Respawn();
        }

        // MOVEMENT ADJUSTMENTS
        if (_hitDistance_RIGHT() < edgeLimmit && velocity > 0) {
            velocity = 0;
        }
        if (_hitDistance_LEFT() < edgeLimmit && velocity < 0) {
            velocity = 0;
        }

        if (moveRAW == 0) {
            if (velocity < 0) {
                velocity += drag_x;
                if (velocity > 0)
                    velocity = 0;
            }
            if (velocity > 0) {
                velocity -= drag_x;
                if (velocity < 0)
                    velocity = 0;
            }
        }
        //MOVEMENT ADJUSTMENTS - END

        // JUMPING
        if (jump) {
            if (!doubleJump) {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            }
            else if (doubleJump) {
                jumpCount = grounded ? 1 : 0;
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpCount > 0 ? jumpForce : ((float)(jumpForce / 1.25)));
            }
            jump = false;
        }

        if (rigidbody2D.velocity.y > 0 && !grounded) {
            if (Input.GetKey(KeyCode.Space))
                jumpBuffer += Time.deltaTime;
            if (!jumpButtonhit && jumpButtonB4) {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y / (3 - jumpBuffer));
            }
        }

        jumpButtonB4 = jumpButtonhit;
        // JUMPING - END

    }

    void FixedUpdate() {
        rigidbody2D.velocity = new Vector2(velocity, rigidbody2D.velocity.y);
        if (MP) {
            rigidbody2D.velocity = new Vector2(vloc + velocity, rigidbody2D.velocity.y);
        }
        if (moveRAW == -1 && !isLeft) {
            _flip();
        }
        if (moveRAW == 1 && isLeft) {
            _flip();
        }
    }

    public void _flip() {
        isLeft = !isLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void _setRayInformation() {
        for (int i = 0; i < 3; i++) {
            switch (i) {
                case 0: {
                        hitIformationRIGHT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, collider2D.bounds.max.y), Vector2.right, Mathf.Infinity, isWall);
                        hitInformationLEFT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, collider2D.bounds.max.y), -Vector2.right, Mathf.Infinity, isWall);
                        hitInformationDOWN[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, collider2D.bounds.min.y), -Vector2.up, Mathf.Infinity, isWall);
                        hitInformationUP[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, collider2D.bounds.max.y), Vector2.up, Mathf.Infinity, isCrusher);
                    }
                    break;
                case 1: {
                        hitIformationRIGHT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, (collider2D.bounds.max.y - collider2D.bounds.size.y / 2)), Vector2.right, Mathf.Infinity, isWall);
                        hitInformationLEFT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, (collider2D.bounds.max.y - collider2D.bounds.size.y / 2)), -Vector2.right, Mathf.Infinity, isWall);
                        hitInformationDOWN[i] = Physics2D.Raycast(new Vector2((collider2D.bounds.min.x + collider2D.bounds.size.x / 2), collider2D.bounds.min.y), -Vector2.up, Mathf.Infinity, isWall);
                        hitInformationUP[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, collider2D.bounds.max.y), Vector2.up, Mathf.Infinity, isCrusher);
                    }
                    break;
                case 2: {
                        hitIformationRIGHT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, collider2D.bounds.min.y), Vector2.right, Mathf.Infinity, isWall);
                        hitInformationLEFT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, collider2D.bounds.min.y), -Vector2.right, Mathf.Infinity, isWall);
                        hitInformationDOWN[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, collider2D.bounds.min.y), -Vector2.up, Mathf.Infinity, isWall);
                    }
                    break;
            }
        }
    }

    float _hitDistance_RIGHT() {
        float distance = Mathf.Infinity;
        for (int i = 0; i < 3; i++) {
            if (hitIformationRIGHT[i].collider != null)
                if (Mathf.Abs(hitIformationRIGHT[i].point.x - collider2D.bounds.max.x) < distance)
                    distance = Mathf.Abs(hitIformationRIGHT[i].point.x - collider2D.bounds.max.x);
        }
        return distance;
    }

    float _hitDistance_LEFT() {
        float distance = Mathf.Infinity;
        for (int i = 0; i < 3; i++) {
            if (hitInformationLEFT[i].collider != null)
                if (Mathf.Abs(hitInformationLEFT[i].point.x - collider2D.bounds.min.x) < distance)
                    distance = Mathf.Abs(hitInformationLEFT[i].point.x - collider2D.bounds.min.x);
        }
        return distance;
    }

    float _hitDistance_DOWN() {
        float distance = Mathf.Infinity;
        for (int i = 0; i < 3; i++) {
            if (hitInformationDOWN[i].collider != null)
                if (Mathf.Abs(hitInformationDOWN[i].point.y - collider2D.bounds.min.y) < distance)
                    distance = Mathf.Abs(hitInformationDOWN[i].point.y - collider2D.bounds.min.y);
        }
        return distance;
    }

    void OnCollisionStay2D(Collision2D coll) {
        if (coll.gameObject.tag == "Moving_Platform") {
            MP = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll) {
        if (coll.gameObject.tag == "Moving_Platform") {
            MP = false;
        }
    }

    bool _crusherCheck() {
        bool B = false;

        for (int i = 0; i < 2; i++) {
            if (hitInformationUP[i].collider != null)
                B = true;
        }

            return B;
    }

    void _Die() {
        rigidbody2D.gravityScale = 0;
        rigidbody2D.velocity = new Vector2(0, 0);
        this.renderer.material.color = new Color(1f, 1f, 1f, 0f);
        this.collider2D.enabled = false;
    }

    void _Respawn() {
        rigidbody2D.position = spawnPoint;
        rigidbody2D.gravityScale = gravity;
        this.collider2D.enabled = true;
        this.renderer.material.color = new Color(1f, 1f, 1f, 1f);
        dead = false;
    }

}
