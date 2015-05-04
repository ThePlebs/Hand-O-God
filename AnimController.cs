using UnityEngine;
using System.Collections;

public class AnimController : MonoBehaviour {

    //MoveScript2 moveScrpt;
    //MoveScript2_2 moveScrpt;
    MoveScript3 moveScrpt;
    Animator controller;
    string animState = "";
    bool canMove = true;
    bool dead = false;
    bool locked = false;

	// Use this for initialization
	void Start () {
        //moveScrpt = GetComponent<MoveScript2>();
        //moveScrpt = GetComponent<MoveScript2_2>();
        moveScrpt = GetComponent<MoveScript3>();
        controller = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!dead)
        {
            if (!locked)
            {
                if ((moveScrpt.moveRAW == 0 || moveScrpt.rigidbody2D.velocity.x == 0) && canMove && moveScrpt.grounded)
                {
                    animState = "IDLE";
                }
                if (moveScrpt.moveRAW != 0 && canMove /*&& moveScrpt.rigidbody2D.velocity.x != 0*/ && moveScrpt.grounded)
                {
                    animState = "RUN";
                }
                if (moveScrpt.rigidbody2D.velocity.y > 0 && !moveScrpt.grounded)
                {
                    animState = "JUMP";
                }
                if (moveScrpt.rigidbody2D.velocity.y <= 0 && !moveScrpt.grounded)
                {
                    animState = "FALLING";
                }
            }
            else
            {

            }
        }
        else
        {

        }
        _setAnim(animState);
	}

    void _setAnim(string state)
    {
        int x;
        switch (state)
        {
            case "IDLE":
                {
                    x = 0;
                    controller.SetInteger("Condition", x);
                }
                break;
            case "RUN":
                {
                    x = 1;
                    controller.SetInteger("Condition", x);
                }
                break;
            case "JUMP":
                {
                    x = 2;
                    controller.SetInteger("Condition", x);
                }
                break;
            case "FALLING":
                {
                    x = 3;
                    controller.SetInteger("Condition", x);
                }
                break;
        }
    }
}
