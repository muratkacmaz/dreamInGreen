using System;
using DG.Tweening;
using UnityEngine;

public class Angle : MonoBehaviour
{
    private void Update()
    {
        
        Debug.DrawLine(transform.position, transform.forward *3, Color.blue);
        Debug.DrawLine(transform.position, transform.up *3, Color.green);
    }
}
