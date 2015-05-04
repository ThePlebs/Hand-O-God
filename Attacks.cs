using UnityEngine;
using System.Collections;

public class Attacks : MonoBehaviour {

    MoveScript3 moveScrpt;
    AnimControl_Gauntlet animC;
    
    public float t_attack = 0f;
    public float timer = 0f;

    bool attackPressedB4 = false;
    bool attackPressed = false;
    public bool inAttack = false;
    public int attackCounter = 0;
    bool dash = false;
    bool dashControl = false;

	// Use this for initialization
	void Start () {
        moveScrpt = GetComponent<MoveScript3>();
        animC = GetComponent<AnimControl_Gauntlet>();
	}
	
	// Update is called once per frame
	void Update () {
        attackPressed = Input.GetKey(KeyCode.LeftControl);
        if (inAttack) {
            moveScrpt.locked = true;
            timer += Time.deltaTime;
            if (attackPressed && !attackPressedB4 && timer > 0.4f) {
                attackCounter++;
                attackCounter = (attackCounter % 3);
                dash = true;
                timer = 0;

                if (Input.GetAxisRaw("Horizontal") == -1 && !moveScrpt.isLeft)
                    moveScrpt._flip();
                if (Input.GetAxisRaw("Horizontal") == 1 && moveScrpt.isLeft)
                    moveScrpt._flip();
            }
            if (timer > 0.15 && timer < 0.20) {
                if (dash) { //&& Input.GetAxisRaw("Horizontal") != 0) {
                    //rigidbody2D.velocity = new Vector2(moveScrpt.isLeft ? -7 : 7, rigidbody2D.velocity.y);
                    if (Input.GetAxisRaw("Horizontal") != 0)
                        moveScrpt.velocity = moveScrpt.isLeft ? -20 : 20;
                    if (!moveScrpt.grounded && attackCounter == 1)
                        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 20);
                    dash = false;
                }

            }
            if (timer > 0.60f && !attackPressed) {
                if (Input.GetAxisRaw("Horizontal") != 0) {
                    inAttack = false;
                    timer = 0;
                }
                if (Input.GetKey(KeyCode.Space)) {
                    inAttack = false;
                    moveScrpt.jump = true;
                    timer = 0;
                }
            }
            if (timer > t_attack) {
                inAttack = false;
                timer = 0;
            }
        }
        else {
            attackCounter = 0;
            moveScrpt.locked = false;
            timer = 0;
            if (attackPressed && !attackPressedB4) {
                inAttack = true;
                attackCounter++;
                dash = true;
            }
        }
        /*if (attackPressed && !attackPressedB4) {
            t++;
            Debug.Log(t);
            t = (t % 3);
            Debug.Log(t);
        }*/
        attackPressedB4 = attackPressed;
	}
}
