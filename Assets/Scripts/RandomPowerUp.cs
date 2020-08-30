using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerUp : MonoBehaviour
{
    public GameObject[] Prefabs;

    void Start()
    {
        Instantiate(Prefabs[Random.Range(0, Prefabs.Length)], transform);
    }
}
