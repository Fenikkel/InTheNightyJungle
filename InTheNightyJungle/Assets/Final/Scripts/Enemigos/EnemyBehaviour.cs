using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour

{
    public Animator anim;
    public bool death = false;
    public float damage; //Valor entre 0 y 100, que se considerará un porcentaje

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public float GetDamage()
    {
        return damage;
    }

    public virtual void CollideWithPlayer()
    {

    }

    public void OnCollisionEnter2D(Collision2D col)
    {
       if (col.gameObject.tag.Equals("fire"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        } 
    }
    public void Death()
    {
        death = true;
        anim.SetBool("idle", true);
        gameObject.SetActive(false);
    }

}
