using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class DevoradorBehaviour : EnemyBehaviour
{
    public float appearingTime;
    public float slowDownFactor;

    private bool inside;
    private bool appeared;
    private bool slowDowned;

    private void Start()
    {
        DisappearBodyParts();
        inside = false;
        appeared = false;
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
        if (!inside) slowDowned = false;
    }

    public void Whispering(GameObject player)
    {
        if(inside && appeared)
        {
            if (!slowDowned)
            {
                slowDowned = true;
                player.GetComponent<PlayerPlatformController>().SlowDown(slowDownFactor, 0);
            }
            player.GetComponent<PlayerStatsController>().ChangePatience(damage * Time.deltaTime);
        }
    }
}
