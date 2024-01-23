using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour //Deber�a heredar de Item

{

    private Inputs input;
    private GameObject player;
    private Camera cam;
    private Rigidbody2D rb;
    private GameObject sprite;
    private Vector3 offsetPlayer = new Vector3(1.5f, 0, 0);
    [SerializeField] public int ammo;
    [SerializeField] public int maxAmmo;
    [SerializeField] GameObject projectile;

    public int damage;

    public static Weapon instance;

    private void Awake()
    {
        input = new Inputs();
        instance = this; 
    }

    // Start is called before the first frame update
    private void Start()
    {
        ammo = maxAmmo;
        UpdateAmmoBar();
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0).gameObject;
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Reload.performed += HandleReload;
        input.Player.Attack.performed += HandleShoot;
    }
    void OnDisable()
    {
        input.Disable();
        input.Player.Reload.performed -= HandleReload;
        input.Player.Attack.performed -= HandleShoot;
    }




    // Update is called once per frame
    void Update()
    {
        HandleAim();
    }

    /// <summary>
    /// Rotates around player
    /// </summary>
    public void HandleAim() {
        Vector3 aim = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
        Vector3 direction = aim - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        transform.position = player.transform.position;
    }

    public void HandleShoot(InputAction.CallbackContext value)
    {
        if (ammo > 0) {
            UseAmmo(1);
            StartCoroutine(IProjectile());
        }
    }

    private IEnumerator IProjectile()
    {
        Instantiate(projectile, sprite.transform.position, Quaternion.identity);
        FindObjectOfType<AudioManager>().Play("VaporeonProjectile");
        yield return true;
    }

    public void HandleReload(InputAction.CallbackContext value)
    {
        RegenerateAmmo(maxAmmo - ammo);
    }

    public void UseAmmo(int ammoQuantity)
    {
        if (ammo >= ammoQuantity)
        {
            ammo -= ammoQuantity;
            UpdateAmmoBar();
        }
    }

    public void RegenerateAmmo(int ammoRegenerateQuantity)
    {
        //Restaura vida si al jugador le queda vida y no est� muerto 
        if (ammo < maxAmmo && !Player.instance.isDead)
        {
            ammo += ammoRegenerateQuantity;

            //As� la vida no supera a la m�xima si recupera m�s del total
            if (ammo > maxAmmo)
            {
                ammo = maxAmmo;
            }
            UIManager.instance.UpdateCharacterAmmo(ammo, maxAmmo);
        }
    }

    private void UpdateAmmoBar()
    {
        UIManager.instance.UpdateCharacterAmmo(ammo, maxAmmo);
    }
}
