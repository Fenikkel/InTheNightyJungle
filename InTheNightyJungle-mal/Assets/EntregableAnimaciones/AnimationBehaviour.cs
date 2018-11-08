using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour {

    public Animator[] anims;
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            PlayAnimation("jump");
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayAnimation("run");
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            PlayAnimation("special");
        }
	}

    private void PlayAnimation(string parameter)
    {
        for(int i = 0; i < anims.Length; i++)
        {
            anims[i].SetTrigger(parameter);
        }
    }
}
