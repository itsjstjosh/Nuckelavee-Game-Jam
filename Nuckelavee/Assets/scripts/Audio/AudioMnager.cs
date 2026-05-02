using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioMnager : MonoBehaviour
{
    
    public static AudioMnager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {

            Debug.LogWarning("There is more than one AudioMnager in the scene");
        }
        instance = this;


    }

    

}
