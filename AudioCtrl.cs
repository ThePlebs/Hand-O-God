using UnityEngine;
using System.Collections;

public class AudioCtrl : MonoBehaviour {

    public AudioClip JumpSound;
    public AudioClip LandSound;
    AudioSource source;

    //MoveScript2 moveScrpt;
    MoveScript2_2 moveScrpt;
    AnimController animC;

    bool groundedB4 = false;
    bool jumpPressedB4 = false;
    int jumpCount = 0;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        //moveScrpt = GetComponent<MoveScript2>();
        moveScrpt = GetComponent<MoveScript2_2>();
        animC = GetComponent<AnimController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!jumpPressedB4 && Input.GetKey(KeyCode.G))
        {
            //Debug.Log(jumpCount);
            if (jumpCount != 3)
                jumpCount = moveScrpt.grounded ? 1 : 2;
            if (jumpCount == 1 || jumpCount == 2)
            {
                source.PlayOneShot(JumpSound, 1f);
                jumpCount = 3;
            }
        }
        if (!groundedB4 && moveScrpt.grounded)
            source.PlayOneShot(LandSound, 1f);
        if (moveScrpt.grounded)
            jumpCount = 0;

        groundedB4 = moveScrpt.grounded;
        jumpPressedB4 = Input.GetKey(KeyCode.G);
	}
}
