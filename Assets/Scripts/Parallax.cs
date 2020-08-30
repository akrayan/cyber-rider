using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Parallax : MonoBehaviour
{
    public GameObject Prefab;
    public float SpeedMultiplier = 1f;

    [SerializeField] [ReadOnly] float _speed = 0;

    List<GameObject> _tiles;
    Vector2 _screenBoundary;
    float _width;

    void addLayer(int i)
    {
        GameObject g;

        if (_width * (i -2) <= _screenBoundary.x)
        {
            g = Instantiate(Prefab, transform);
            g.transform.localPosition = Vector3.left * _width * i;
            addLayer(i + 1);
            _tiles.Add(g);
        }
    }

    void Start()
    {
        _screenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        transform.position = new Vector3(_screenBoundary.x, transform.position.y, transform.position.z);
        _width = Prefab.GetComponent<SpriteRenderer>().bounds.size.x;
        _tiles = new List<GameObject>();

        addLayer(0);
    }

    void Update()
    {

        foreach (GameObject obj in _tiles)
        {
            obj.transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (_tiles.Count > 0 && _tiles[_tiles.Count - 1].transform.localPosition.x <= -_width)
        {
            Vector3 v = _tiles[_tiles.Count - 1].transform.position;

            v.x += _width;
            _tiles.Add(Instantiate(Prefab, v, Quaternion.identity, transform));
        }
        if (_tiles.Count > 0 && _tiles[0].transform.position.x + _width < _screenBoundary.x * -1)
        {

            GameObject g = _tiles[0];

            Destroy(g);
            _tiles.RemoveAt(0);
        }
        _speed = LevelManager._instance.Speed * SpeedMultiplier;

    }
}
