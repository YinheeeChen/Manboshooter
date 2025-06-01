using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManger : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Wave[] wave;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public struct Wave
{
    public string name;
    public List<WaveSegment> segments;
}

[System.Serializable]
public struct WaveSegment
{
    public GameObject prefab;
    
}
