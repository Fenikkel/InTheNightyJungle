using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : PhysicsObject
{
    public float damage; //Valor entre 0 y 100, que se considerará un porcentaje

    // Use this for initialization
    protected override void initialization()
    {
        base.initialization();
    }

    protected override void ComputeVelocity()
    {

    }

    public float GetDamage()
    {
        return damage;
    }

    public virtual void CollideWithPlayer()
    {

    }
}
