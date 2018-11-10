using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalbucienteBehaviour : PhysicsObject {

    private BoxCollider2D influenceZone;
    public Vector2 sizeInfluenceZone = new Vector2(1, 1);

	// Use this for initialization
	void Start () {
        influenceZone = GetComponent<BoxCollider2D>();
        influenceZone.size = sizeInfluenceZone;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
