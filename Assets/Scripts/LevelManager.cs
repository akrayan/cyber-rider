using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class LevelManager : MonoBehaviour
{
    public GameObject LevelSpawner;
    public GameObject TitlePlayer;
    public GameObject Player;
    public GameObject TitleCanvas;
    public GameObject GameCanvas;
    public GameObject NitroMask;
    public Text DistanceText;
    [System.NonSerialized] public float Speed = 0;
    public float NitroDuration { get => _nitroDuration; }

    [SerializeField] private float _initialSpeed = 1;
    [SerializeField] private float _maxSpeed = 5;
    [SerializeField] private float _maxSpeedDistance = 100;
    [SerializeField] private float _nitroMultiplier = 2;
    [SerializeField] private float _nitroDuration = 5;

    private bool _playMode = false;
    private bool _nitro = false;
    private float _nitroTimer = 0;

    [SerializeField] private AnimationCurve _speedEvolution;
    [SerializeField] [ReadOnly] private float _distance = 0;

    public static LevelManager _instance;

    void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //Speed = _initialSpeed;
        Speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playMode)
        {
            updateSpeed();
            DistanceText.text = (int)_distance + "m";
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.UpArrow))
            StartGame();
    }

    void updateSpeed()
    {
        _distance += Speed * Time.deltaTime;
        Speed = _initialSpeed + _speedEvolution.Evaluate(Mathf.InverseLerp(0, _maxSpeedDistance, _distance)) * (_maxSpeed - _initialSpeed);
        if (_nitro) Speed *= _nitroMultiplier;
    }

    public void NitroActivation()
    {
        NitroMask.SetActive(true);
        _nitro = true;
        _nitroTimer = Time.time;
    }

    public void NitroDesactivation()
    {
        NitroMask.SetActive(false);
        _nitro = false;
    }

    public void StartGame()
    {
        Speed = _initialSpeed;
        _playMode = true;
        GameCanvas.SetActive(true);
        LevelSpawner.SetActive(true);
        Player.SetActive(true);
        TitleCanvas.SetActive(false);
        TitlePlayer.SetActive(false);
    }
}
