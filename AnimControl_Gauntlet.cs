using UnityEngine;
using System.Collections;

public class AnimControl_Gauntlet : MonoBehaviour {

    MoveScript3 moveScrpt;
    Attacks attScrpt;
    Animator controller;
    string animState = "";
    bool canMove = true;
    bool dead = false;
    bool locked = false;

	// Use this for initialization
	void Start () {
        moveScrpt = GetComponent<MoveScript3>();
        attScrpt = GetComponent<Attacks>();
        controller = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!dead) {
            if (!locked) {
                if ((moveScrpt.moveRAW == 0 || moveScrpt.rigidbody2D.velocity.x == 0) && canMove && moveScrpt.grounded) {
                    animState = "IDLE";
                }
                if (moveScrpt.moveRAW != 0 && canMove /*&& moveScrpt.rigidbody2D.velocity.x != 0*/ && moveScrpt.grounded) {
                    animState = "RUN";
                }
                if (moveScrpt.rigidbody2D.velocity.y > 0 && !moveScrpt.grounded) {
                    animState = "JUMP";
                }
                if (moveScrpt.rigidbody2D.velocity.y <= 0 && !moveScrpt.grounded) {
                    animState = "FALLING";
                }

                if (attScrpt.inAttack) {
                    if (attScrpt.attackCounter == 1)
                        animState = "ATTACK_1";
                    if (attScrpt.attackCounter == 2)
                        animState = "ATTACK_2";
                    if (attScrpt.attackCounter == 0)
                        animState = "ATTACK_3";
                }

            }
            else {
                //Debug.Log(animState);
            }
        }
        else {

        }
        _setAnim(animState);
        //Debug.Log(animState);
    }

    void _setAnim(string state) {
        int x;
        switch (state) {
            case "IDLE": {
                    x = 0;
                    controller.SetInteger("State_Gauntlet", x);
                }
                break;
            case "RUN": {
                    x = 1;
                    controller.SetInteger("State_Gauntlet", x);
                }
                break;
            case "JUMP": {
                    x = 2;
                    controller.SetInteger("State_Gauntlet", x);
                }
                break;
            case "FALLING": {
                    x = 3;
                    controller.SetInteger("State_Gauntlet", x);
                }
                break;
            case "ATTACK_1": {
                    x = 4;
                    controller.SetInteger("State_Gauntlet", x);
                }
                break;
            case "ATTACK_2": {
                    x = 5;
                    controller.SetInteger("State_Gauntlet", x);
                }
                break;
            case "ATTACK_3": {
                    x = 6;
                    controller.SetInteger("State_Gauntlet", x);
                }
                break;
        }
    }
}
