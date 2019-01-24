using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class DevoradorBehaviour : EnemyBehaviour
{
    public float appearingTime;
    public float slowDownFactor;

    private bool inside;
    private bool slowDowned;

    [SerializeField]
    private AudioSource whisperSound;

    private void Start()
    {
        DisappearBodyParts();
        inside = false;
        slowDowned = false;
    }

    private void DisappearBodyParts()
    {
        foreach (SpriteMeshInstance part in bodyParts)
            part.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    private void Update()
    {
        if(inside && !appeared)
        {
            StartCoroutine(FadeIn(appearingTime));
        }
        if (!inside && appeared)
        {
            StartCoroutine(FadeOut(appearingTime, false));
        }
    }

    public void SetInside(bool param)
    {
        inside = param;
        if (!inside) 
        {
            slowDowned = false;
            anim.SetBool("whisper", false);
            whisperSound.Stop();
        }
    }

    public void Whispering(GameObject player)
    {
        if(inside)
        {
            anim.SetBool("whisper", true);
            if(GetComponent<Transform>().localScale.x > 0 && player.GetComponent<Transform>().position.x < GetComponent<Transform>().position.x) 
                GetComponent<Transform>().localScale = new Vector3(-Mathf.Abs(GetComponent<Transform>().localScale.x), GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);
            else if(GetComponent<Transform>().localScale.x < 0 && player.GetComponent<Transform>().position.x > GetComponent<Transform>().position.x)
                GetComponent<Transform>().localScale = new Vector3(Mathf.Abs(GetComponent<Transform>().localScale.x), GetComponent<Transform>().localScale.y, GetComponent<Transform>().localScale.z);

            if(appeared)
            {
                if (!slowDowned)
                {
                    slowDowned = true;
                    player.GetComponent<PlayerPlatformController>().SlowDown(slowDownFactor, 0);
                    whisperSound.Play();
                }
                player.GetComponent<PlayerStatsController>().ChangePatience(-damage * Time.deltaTime);
            }
        }
    }
}
