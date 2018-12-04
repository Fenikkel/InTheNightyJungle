using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour {
    public int coinvalue;

     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            soundsmanager.PlaySound("Coin");
            Destroy(gameObject);
            Score.scorevalue += coinvalue;
        }
    }

}
