using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planeta : MonoBehaviour
{    
    //Скрипт - задается вращение планты

    [SerializeField] private float _rotation;

    [SerializeField] private Vector3 _toRot;

    public bool _isFinalPlanet;

    private void FixedUpdate()
    {
        RotatePlanet();
    }
    public void RotatePlanet()
    {
        transform.Rotate(_toRot * _rotation);
    }
}
