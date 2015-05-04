using UnityEngine;
using System.Collections;

public class MoveScript2_2 : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float accelerationRate = 0.5f;
    public float jumpForce = 20f;
    float velocity = 0f;
    float drag_x;
    public bool grounded = false;

    public float moveRAW;
    bool isLeft = true;
    Vector2 position;
    Rect box;

    RaycastHit2D[] hitIformationRIGHT = new RaycastHit2D[3];
    RaycastHit2D[] hitInformationLEFT = new RaycastHit2D[3];
    RaycastHit2D[] hitInformationDOWN = new RaycastHit2D[3];
    Vector2 rayPositionRIGHT;
    Vector2 rayPositionLEFT;
    Vector2 rayPositionDOWN;

    bool jumpButtonHit = false;
    bool jumpCan = false;
    int jumpCount = 2;
    float jumpBuffer = 0f;
    bool jumpBuff = false;

    public bool locked = false;

    public LayerMask isWall;

    // Use this for initialization
    void Start()
    {
        position = rigidbody2D.position;
        box = new Rect(
            collider2D.bounds.min.x,
            collider2D.bounds.min.y,
            collider2D.bounds.size.x,
            collider2D.bounds.size.y

        );
        //drag_x = accelerationRate;
    }

    void Update()
    {
        drag_x = accelerationRate;
        if (grounded)
        {
            jumpBuffer = 0f;
            jumpBuff = false;
        }
        if (Input.GetKey(KeyCode.G) && !jumpButtonHit && jumpCount != 0 && !locked)
        {
            jumpCan = true;
            jumpButtonHit = true;
        }
        jumpButtonHit = Input.GetKey(KeyCode.G);

        if (!grounded && rigidbody2D.velocity.y > 0 && jumpButtonHit)
        {
            jumpBuffer += Time.deltaTime;
        }
        if (!grounded && rigidbody2D.velocity.y > 0)
        {
            if (!jumpBuff && !jumpButtonHit)
            {
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y / 2.87f);
                jumpBuff = true;
            }
        }
        grounded = _hitDistance_DOWN() < 0.0195f;
        Debug.Log(rigidbody2D.velocity.x);
    //}

    // Update is called once per frame
    /*void FixedUpdate()
    {*/
        moveRAW = locked ? 0 : Input.GetAxisRaw("Horizontal");
        position = rigidbody2D.position;

        _setRayInformation();

        if (grounded)
            jumpCount = 2;

        velocity += moveRAW * accelerationRate;
        if (Mathf.Abs(velocity) > maxSpeed)
            velocity = moveRAW * maxSpeed;

        if (_hitDistance_RIGHT() < 0.0165 && velocity > 0)
            velocity = 0;
        if (_hitDistance_LEFT() < 0.0165 && velocity < 0)
            velocity = 0;

        if (moveRAW == 0)
        {
            if (velocity < 0)
            {
                velocity += drag_x;
                if (velocity > 0)
                    velocity = 0;
            }
            if (velocity > 0)
            {
                velocity -= drag_x;
                if (velocity < 0)
                    velocity = 0;
            }
        }

        if (jumpCan)
        {
            jumpCount = grounded ? 1 : 0;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpCount == 1 ? jumpForce : jumpForce / 1.2f);
            jumpBuff = false;
            jumpCan = false;
        }
    }
    void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(velocity, rigidbody2D.velocity.y);

        if (moveRAW == -1 && !isLeft)
            _flip();
        else if (moveRAW == 1 && isLeft)
            _flip();
    }

    void _flip()
    {
        isLeft = !isLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void _setRayInformation()
    {
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                case 0:
                    {
                        hitIformationRIGHT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, collider2D.bounds.max.y), Vector2.right, Mathf.Infinity, isWall);
                        hitInformationLEFT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, collider2D.bounds.max.y), -Vector2.right, Mathf.Infinity, isWall);
                        hitInformationDOWN[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, collider2D.bounds.min.y), -Vector2.up, Mathf.Infinity, isWall);
                    }
                    break;
                case 1:
                    {
                        hitIformationRIGHT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, (collider2D.bounds.max.y - collider2D.bounds.size.y / 2)), Vector2.right, Mathf.Infinity, isWall);
                        hitInformationLEFT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, (collider2D.bounds.max.y - collider2D.bounds.size.y / 2)), -Vector2.right, Mathf.Infinity, isWall);
                        hitInformationDOWN[i] = Physics2D.Raycast(new Vector2((collider2D.bounds.min.x + collider2D.bounds.size.x / 2), collider2D.bounds.min.y), -Vector2.up, Mathf.Infinity, isWall);
                    }
                    break;
                case 2:
                    {
                        hitIformationRIGHT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, collider2D.bounds.min.y), Vector2.right, Mathf.Infinity, isWall);
                        hitInformationLEFT[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.min.x, collider2D.bounds.min.y), -Vector2.right, Mathf.Infinity, isWall);
                        hitInformationDOWN[i] = Physics2D.Raycast(new Vector2(collider2D.bounds.max.x, collider2D.bounds.min.y), -Vector2.up, Mathf.Infinity, isWall);
                    }
                    break;
            }
        }
    }

    float _hitDistance_RIGHT()
    {
        float distance = Mathf.Infinity;
        for (int i = 0; i < 3; i++)
        {
            if (hitIformationRIGHT[i].collider != null)
                if (Mathf.Abs(hitIformationRIGHT[i].point.x - collider2D.bounds.max.x) < distance)
                    distance = Mathf.Abs(hitIformationRIGHT[i].point.x - collider2D.bounds.max.x);
        }
        return distance;
    }

    float _hitDistance_LEFT()
    {
        float distance = Mathf.Infinity;
        for (int i = 0; i < 3; i++)
        {
            if (hitInformationLEFT[i].collider != null)
                if (Mathf.Abs(hitInformationLEFT[i].point.x - collider2D.bounds.min.x) < distance)
                    distance = Mathf.Abs(hitInformationLEFT[i].point.x - collider2D.bounds.min.x);
        }
        return distance;
    }

    float _hitDistance_DOWN()
    {
        float distance = Mathf.Infinity;
        for (int i = 0; i < 3; i++)
        {
            if (hitInformationDOWN[i].collider != null)
                if (Mathf.Abs(hitInformationDOWN[i].point.y - collider2D.bounds.min.y) < distance)
                    distance = Mathf.Abs(hitInformationDOWN[i].point.y - collider2D.bounds.min.y);
        }
        return distance;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Collided");
    }
}
