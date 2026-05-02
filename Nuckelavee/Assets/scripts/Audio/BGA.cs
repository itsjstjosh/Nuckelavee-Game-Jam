using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BGA : MonoBehaviour
{
    [SerializeField] private EventReference ambianceLevel;
    FMOD.Studio.EventInstance ambinaceInstace;

    /* public void PlayAmbiance()
     {
         RuntimeManager.PlayOneShot(ambianceLevel);
     }*/

    // Start is called before the first frame update
    void Start()
    {
        ambinaceInstace = RuntimeManager.CreateInstance(ambianceLevel);
        ambinaceInstace.start();    


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        // Release memory
        ambinaceInstace.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
