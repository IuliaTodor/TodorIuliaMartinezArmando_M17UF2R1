using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour //Deber�a heredar de Item

{
    [SerializeField] public int ammo;
    [SerializeField] public int maxAmmo;

    public int damage;

    public static Weapon instance;

    private void Awake()
    {
        instance = this; 
    }

    // Start is called before the first frame update
    private void Start()
    {
        ammo = maxAmmo;
        UpdateAmmoBar();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            UseAmmo(1);
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            RegenerateAmmo(1);
        }
    }

    public void HandleShoot()
    {
        
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
