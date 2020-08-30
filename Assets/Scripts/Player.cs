using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject ShieldUI;
    public GameObject NitroUI;
    public Material ShieldMaterial;
    public Material NitroMaterial;
    Material _oldMaterial;
    Renderer _rend;
    Collider _col;
    bool _sheild = false;
    bool _nitro = false;
    bool _sheildStock = false;
    bool _nitroStock = false;
    bool _vulnerable = false;
    float _nitroTimer = 0;
    float _nitroDuration = 0;


    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _col = GetComponent<Collider>();
        _oldMaterial = _rend.material;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && _sheildStock)
            this.shieldActivation();
        if (Input.GetKeyDown(KeyCode.R) && _nitroStock)
            this.nitroActivation();
        if (_nitro && Time.time - _nitroTimer >= _nitroDuration)
        {
            _nitro = false;
            _rend.material = _oldMaterial;
            LevelManager._instance.NitroDesactivation();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_vulnerable)
        {
            Color c = _oldMaterial.color;

            c.a = 1;
            _rend.material.color = c;
            _vulnerable = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");

        switch (collision.tag)
        {
            case "Coin":
                Destroy(collision.gameObject);
                break;
            case "Shield":
                Destroy(collision.gameObject);
                _sheildStock = true;
                ShieldUI.SetActive(true);
                //shieldActivation();
                break;
            case "Nitro":
                Destroy(collision.gameObject);
                _nitroStock = true;
                NitroUI.SetActive(true);
                //nitroActivation();
                break;
            case "Obstacle":
                if (!_sheild && !_nitro)
                    gameOver();
                else if (_sheild)
                {
                    _vulnerable = true;
                    shieldDisable();
                }
                break;
        }
    }

    void shieldDisable()
    {
        Color c = _oldMaterial.color;

        _sheild = false;
        //_vulnerable = true;
        _rend.material = _oldMaterial;
        if (_vulnerable)
        {
            c = _rend.material.color;
            c.a = 0.5f;
            _rend.material.color = c;
        }
    }

    void shieldActivation()
    {
        if (_nitro)
            nitroDisable();
        ShieldUI.SetActive(false);
        _sheildStock = false;
        _sheild = true;
        _rend.material = ShieldMaterial;
    }

    void nitroActivation()
    {
        if (_sheild)
            shieldDisable();
        NitroUI.SetActive(false);
        _nitroStock = false;
        LevelManager._instance.NitroActivation();
        _nitroDuration = LevelManager._instance.NitroDuration;
        _nitroTimer = Time.time;
        _nitro = true;
        _rend.material = NitroMaterial;
    }

    void nitroDisable()
    {
        _nitro = false;
        _rend.material = _oldMaterial;
        LevelManager._instance.NitroDesactivation();
    }

    void gameOver()
    {
        SceneManager.LoadScene(0);
    }
}
