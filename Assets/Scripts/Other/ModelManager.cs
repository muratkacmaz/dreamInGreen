using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelManager : MonoBehaviour
{
    public static ModelManager instance;

    [SerializeField] private GameObject[] models;
    
    private Dictionary<string, int> modelKeyIndexCache = new Dictionary<string, int>();

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < models.Length; i++)
        {
            modelKeyIndexCache.Add(models[i].name, i);
        }
    }

    public GameObject GetModel(string key)
    {
        print("Key subs : " + key);
        
        if(modelKeyIndexCache.ContainsKey(key))
            return models[modelKeyIndexCache[key]].gameObject;
        else
        {
            return null;
        }
    }
}
