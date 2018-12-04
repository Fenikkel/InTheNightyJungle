using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour

{
    public bool death = false;
    public float damage; //Valor entre 0 y 100, que se considerará un porcentaje

    public float GetDamage()
    {
        return damage;
    }

    public virtual void CollideWithPlayer()
    {

    }
    public void Death()
    {
        death = true;

    }
}
