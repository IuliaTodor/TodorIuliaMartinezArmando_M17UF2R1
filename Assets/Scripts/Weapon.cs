using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour //Debería heredar de Item

{
    [SerializeField] public int ammo;
    [SerializeField] public int maxAmmo;

    public int damage;
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

    public void RegenerateAmmo(int ammoRegenereateQuantity)
    {
        if (Player.instance.health > 0f && ammo < maxAmmo)
        {
            ammo += ammoRegenereateQuantity;
            UpdateAmmoBar();
        }
    }

    private void UpdateAmmoBar()
    {
        UIManager.instance.UpdateCharacterAmmo(ammo, maxAmmo);
    }
}
