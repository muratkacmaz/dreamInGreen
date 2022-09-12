using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _transform;

    private void Awake() => _transform = transform;

    // Update is called once per frame
    void Update()
    {
        _transform.Rotate(Vector3.right * _speed * Time.deltaTime);
    }
}
