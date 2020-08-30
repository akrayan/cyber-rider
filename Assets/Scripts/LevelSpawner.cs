using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class LevelSpawner : MonoBehaviour
{
    //public Transform transform;
    public GameObject[] Prefabs;

    public float SpeedMultiplier = 1f;
    //public float MaxDistance = 20;
    public float DistanceBetweenPrefabs = 3f;

    [SerializeField] [ReadOnly] float _speed = 0;
    List<GameObject> _obstacles;
    Vector2 _screenBoundary;
    float _width;


    void addLayer()
    {
        GameObject g;

        g = Instantiate(Prefabs[Random.Range(0, Prefabs.Length)], transform);
        _obstacles.Add(g);
    }

    void Start()
    {
        _screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        transform.position = new Vector3(_screenBoundary.x, transform.position.y, transform.position.z);
        //_width = Prefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;
        _obstacles = new List<GameObject>();


        //addLayer(0);
        addLayer();
    }

    // Update is called once per frame
    void Update()
    {
        _speed = LevelManager._instance.Speed * SpeedMultiplier;

        foreach (GameObject obj in _obstacles)
        {
            obj.transform.Translate(Vector3.left * _speed * Time.deltaTime);
            //obj.transform.Find("End").transform.position.x;
        }
        if (_obstacles.Count > 0 && _obstacles[_obstacles.Count - 1].transform.Find("End").transform.position.x <= -DistanceBetweenPrefabs)
        {
            _obstacles.Add(Instantiate(Prefabs[Random.Range(0, Prefabs.Length)], transform));
        }/*
        if (_obstacles.Count > 0 && _obstacles[_obstacles.Count - 1].transform.localPosition.x <= -_width - DistanceBetweenPrefabs)
        {
            _obstacles.Add(Instantiate(Prefabs[Random.Range(0, Prefabs.Length)], transform));
        }*/
        if (_obstacles.Count > 0 && _obstacles[0].transform.Find("End").transform.position.x < _screenBoundary.x * -1)
        {
            GameObject g = _obstacles[0];

            Destroy(g);
            _obstacles.RemoveAt(0);
        }
        /*
        if (_obstacles.Count > 0 && _obstacles[0].transform.position.x + _width < _screenBoundary.x * -1)
        {
            GameObject g = _obstacles[0];

            Destroy(g);
            _obstacles.RemoveAt(0);
        }*/
    }
}
