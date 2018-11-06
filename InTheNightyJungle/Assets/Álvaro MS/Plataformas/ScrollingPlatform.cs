using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingPlatform : MonoBehaviour {

    private Material mat;
    public float speedMax;

    private float speedX;
    private float speedY;
    
    // Use this for initialization
	void Start () {
        mat = GetComponent<SpriteRenderer>().material;
        speedX = Random.Range(0, speedMax);
        speedY = Random.Range(0, speedMax);
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 currentOffset = mat.GetTextureOffset("_MainTex");
        mat.SetTextureOffset("_MainTex", new Vector2(currentOffset.x + speedX * Time.deltaTime, currentOffset.y + speedY * Time.deltaTime));
	}
}
