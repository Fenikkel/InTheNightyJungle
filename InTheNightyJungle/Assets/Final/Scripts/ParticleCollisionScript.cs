using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollisionScript : MonoBehaviour {

    public ParticleSystem fireSparks; //lo pongo asi por si reutilizamos el script

    private void OnParticleCollision(GameObject other)
    {
        //no hace falta comprobar el tag porque ya esta configurado en el particle system para que solo colisione con los layers que le he dicho
        other.gameObject.SetActive(false);
        //activar animacion muerte y desactivar su box colider o lo que sea
        other.GetComponent<EnemyBehaviour>().Death();
    }
}
