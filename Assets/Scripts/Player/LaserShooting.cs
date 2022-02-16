using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooting : MonoBehaviour
{

    public GameObject laserPrefab;

    public Transform laserDirection;

    private PlayerInputs controls;

    private GameObject g;

    


    // [SerializeField]
    // private float _speed = 10.0f;


    private void Awake() 
    {
        controls = new PlayerInputs();
    }

    private void OnEnable() 
    {
        controls.Enable();
    }

    private void OnDisable() 
    {
        controls.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        controls.Player.Shoot.performed += _ => PlayerShoot();
    }


    private void PlayerShoot()
    {
        Vector2 mousePosition = controls.Player.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        g = Instantiate(laserPrefab, laserDirection.position, laserDirection.rotation);
        g.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
} 
