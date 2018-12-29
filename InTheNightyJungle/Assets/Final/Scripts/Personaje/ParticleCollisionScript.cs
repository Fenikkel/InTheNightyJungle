using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionScript : MonoBehaviour {

    public ParticleSystem fireSparks; //lo pongo asi por si reutilizamos el script
    public List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        fireSparks = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = fireSparks.GetCollisionEvents(other, collisionEvents);
        /*int i = 0;

        while (i < numCollisionEvents)
        {
            print(collisionEvents[i].normal);
            i++;
        }*/

        other.GetComponent<EnemyBehaviour>().Death((int) Mathf.Sign(collisionEvents[0].normal.x));
    }
}
