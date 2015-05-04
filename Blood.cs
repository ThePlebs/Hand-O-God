using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {

    public ParticleSystem bloods;
    MoveScript3 movescprt;
    int spl = 0;

	// Use this for initialization
	void Start () {
        movescprt = GetComponent<MoveScript3>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(bloods.isPlaying);
        if (movescprt.dead) {
            if (!bloods.isPlaying) {
                bloods.Play();
            }
            else {
                bloods.Stop();
            }
        }
        else {
            bloods.Clear();
        }
	
	}

    void Splode() {
        Vector3 position = movescprt.rigidbody2D.position;
        bloods.Play();
    }
}
