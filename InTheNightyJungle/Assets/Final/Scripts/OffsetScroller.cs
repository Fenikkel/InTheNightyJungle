using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OffsetScroller : MonoBehaviour {

    public float scrollSpeed;
    private Vector2 savedOffset;

    void Start () {
        savedOffset = GetComponent<SpriteRenderer>().material.GetTextureOffset ("_MainTex");
    }

    void Update () {
        float x = Mathf.Repeat (Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2 (x, savedOffset.y);
        GetComponent<SpriteRenderer>().material.SetTextureOffset ("_MainTex", offset);
    }

    void OnDisable () {
        GetComponent<SpriteRenderer>().material.SetTextureOffset ("_MainTex", savedOffset);
    }
}
