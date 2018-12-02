using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class DevoradorBehaviour : EnemyBehaviour
{

    public SpriteMeshInstance[] bodyParts;
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
            StartCoroutine(FadeOut(appearingTime));
        }
    }

    IEnumerator FadeOut(float time)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            foreach (SpriteMeshInstance part in bodyParts)
            {
                part.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 1.0f), new Color(1.0f, 1.0f, 1.0f, 0.0f), elapsedTime / time);
            }
            yield return null;
        }
        foreach (SpriteMeshInstance part in bodyParts)
        {
            part.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        appeared = false;
    }

    IEnumerator FadeIn(float time)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            foreach (SpriteMeshInstance part in bodyParts)
            {
                part.color = Color.Lerp(new Color(1.0f, 1.0f, 1.0f, 0.0f), new Color(1.0f, 1.0f, 1.0f, 1.0f), elapsedTime / time);
            }
            yield return null;
        }
        foreach (SpriteMeshInstance part in bodyParts)
        {
            part.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
        appeared = true;
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
