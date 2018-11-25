using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour {

    bool insideCheckPoint;
    Animator playerAnimator;

    private void Start()
    {
        playerAnimator = PlayerPlatformController.Instance.GetComponent<Animator>();
    }

    private void Update()
    {
        if (insideCheckPoint && Input.GetKeyDown(KeyCode.E) && !PlayerPlatformController.Instance.Descansando)
        {
            playerAnimator.SetBool("Sitting", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            insideCheckPoint = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            insideCheckPoint = false;
        }
    }
}
