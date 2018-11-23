using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathHandler : MonoBehaviour {

    public ParticleSystem firePS;

    public void EnableBreathEvent()
    {

        firePS.Play();

    }
    public void DisableBreathEvent()
    {

        firePS.Stop();

    }

}
