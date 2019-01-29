using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Anima2D;

public class EnemyBehaviour : MonoBehaviour
{
    public float damage; //Valor entre 0 y 100, que se considerará un porcentaje
    protected bool death;
    protected bool appeared;
    public SpriteMeshInstance[] bodyParts;

    protected Animator anim;
    protected CapsuleCollider2D collider;

    [SerializeField]
    protected AudioSource hitSound;

    [SerializeField]
    protected Transform enemyCore;

    [SerializeField]
    protected AudioSource deathSoundSource;
    [SerializeField]
    protected AudioClip[] deathSounds;
    
    [SerializeField]
    protected GameObject moneyInPocket;
    [SerializeField]
    protected GameObject collectibleInPocket;
    [SerializeField]
    protected Sprite collectibleInInventory;
    [SerializeField]
    protected string collectibleName, collectibleDescription;
    protected bool stolen;

    void Awake()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();

        stolen = false;
    }

    public float GetDamage()
    {
        return damage;
    }

    public virtual void CollideWithPlayer()
    {

    }

    public virtual IEnumerator Steal(Transform rightHand)
    {
        GameObject moneyObj = null;
        GameObject collectibleObj = null;
        if(!stolen)
        {
            stolen = true;
            moneyObj = Instantiate(moneyInPocket, enemyCore.position, Quaternion.identity);

            if(collectibleInPocket != null)
            {
                collectibleObj = Instantiate(collectibleInPocket, enemyCore.position, Quaternion.identity);
                collectibleObj.GetComponent<ObtainingCollectible>().InitializeInfo(collectibleInInventory, collectibleName, collectibleDescription);
            }

            yield return new WaitForSeconds(0.5f);
            moneyObj.GetComponent<Transform>().SetParent(rightHand);
            moneyObj.GetComponent<ObtainingMoney>().StolenByPlayer();
            moneyInPocket = null;
        
            if(collectibleInPocket != null)
            {
                collectibleObj.GetComponent<Transform>().SetParent(rightHand);
                collectibleObj.GetComponent<ObtainingCollectible>().StolenByPlayer();
                collectibleInPocket = null;
            }
        }
    }

    public virtual IEnumerator DropItems()
    {
        GameObject moneyObj = null;
        GameObject collectibleObj = null;
        if(!stolen)
        {
            stolen = true;
            moneyObj = Instantiate(moneyInPocket, enemyCore.position, Quaternion.identity);
            Vector2 initialScale = Vector2.zero;
            Vector2 finalScale1 = moneyObj.GetComponent<Transform>().localScale;
            Vector2 finalScale2 = Vector2.zero;
            moneyObj.GetComponent<Transform>().localScale = Vector2.zero;

            if(collectibleInPocket != null)
            {
                collectibleObj = Instantiate(collectibleInPocket, enemyCore.position, Quaternion.identity);
                collectibleObj.GetComponent<ObtainingCollectible>().InitializeInfo(collectibleInInventory, collectibleName, collectibleDescription);
                finalScale2 = collectibleObj.GetComponent<Transform>().localScale;
                collectibleObj.GetComponent<Transform>().localScale = Vector2.zero;
            }

            //yield return new WaitForSeconds(0.5f);

            Vector2 originPosition = enemyCore.position;
            Vector2 targetPosition1 = originPosition + new Vector2(1,0);
            Vector2 targetPosition2 = originPosition + new Vector2(-1,0);

            float elapsedTime = 0.0f;
            while(elapsedTime < 1.0f)
            {
                elapsedTime += Time.deltaTime;
                moneyObj.GetComponent<Transform>().position = Parabola(originPosition, targetPosition1, 1.0f, elapsedTime);
                moneyObj.GetComponent<Transform>().localScale = Vector2.Lerp(initialScale, finalScale1, elapsedTime);
                if(collectibleInPocket != null)
                {
                    collectibleObj.GetComponent<Transform>().position = Parabola(originPosition, targetPosition2, 1.0f, elapsedTime);
                    collectibleObj.GetComponent<Transform>().localScale = Vector2.Lerp(initialScale, finalScale2, elapsedTime);                    
                }
                yield return null;
            }
            moneyInPocket = null;
            collectibleInPocket = null;
        }
    }

    private Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        float f = -4 * height * t * t + 4 * height * t;

        Vector2 mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f + Mathf.Lerp(start.y, end.y, t));
    }

    public virtual void Death(int normalDirection)
    {
        if(!death)
        {
            StartCoroutine(DropItems());
            gameObject.layer = LayerMask.NameToLayer("UntargetedPlayer");
            death = true;
            if(normalDirection == Mathf.Sign(GetComponent<Transform>().localScale.x))
                anim.SetTrigger("FrontDeath");
            else 
                anim.SetTrigger("BackDeath");

            
            PlayDeathSound();
        }
    }

    private void PlayDeathSound()
    {
        int aux = Random.Range(0, deathSounds.Length);
        deathSoundSource.clip = deathSounds[aux];
        deathSoundSource.Play();
    }

    public void DisappearInDeath()
    {
        StartCoroutine(FadeOut(1.0f, true));
    }

    protected IEnumerator FadeOut(float time, bool death)
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
        if(death) Destroy(gameObject);
    }

    protected IEnumerator FadeIn(float time)
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
}
