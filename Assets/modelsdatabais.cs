using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class modelsdatabais : ScriptableObject
{
    public modelsui[] models;
   

    public int modelscount
    {
        get
        {
            return models.Length;
        }
    } 

    public modelsui Getmodel(int index)
    {
        return models[index];
    }
}
