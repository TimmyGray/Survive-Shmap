using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Weapons;

public class PlayerController : MonoBehaviour
{
    public Character player; 

    private Rigidbody2D myRigidbody2D;

    private bool isMoving = false;

    private Vector2 moveDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();

        // Initialize the player's current weapons
        // Only for the testing phase
        for(int i = 0; i < player.allWeapons.Count; i++)
        {
            GameObject weapon = Instantiate(player.allWeapons[i], transform.position, Quaternion.identity);
            SetWeaponPosition(weapon);
            player.currentWeapons.Add(weapon);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving) 
        {
            Moving();
        }

        Fire();
    }

    public void OnMove(InputValue moveValue)
    {
        moveDirection = moveValue.Get<Vector2>().normalized;

        if (Mathf.Abs(moveDirection.magnitude) > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }

    /// <summary>
    /// Fire the player's current weapons and update the time to next attack for each weapon.
    /// </summary>
    public void Fire()
    {
        foreach (GameObject weapon in player.currentWeapons)
        {
            bool isWeaponExist = weapon.TryGetComponent<WeaponController>(out WeaponController weaponController);
            if (isWeaponExist && weaponController.timeToNextAttack <= 0)
            {
                weaponController.Fire();
            }
            else
            {
                weaponController.timeToNextAttack -= Time.fixedDeltaTime;
            }
        }
    }

    /// <summary>
    /// Move the player according to the move direction. Using rigidbody2D and force to move the player.
    /// </summary>
    private void Moving()
    {
        myRigidbody2D.AddForce(new Vector2(moveDirection.x * player.speed, moveDirection.y * player.speed));
    }


    public void LevelUp(){}

    /// <summary>
    /// Change the player's current weapon. Add, Change or Level up the weapon depending on the situation.
    /// Should be called when the player levels up from the level up menu.
    /// </summary>
    public void ChangeWeapon(GameObject weapon, int? index)
    {
        WeaponController newWeaponController = weapon.GetComponent<WeaponController>();
        foreach (GameObject oldWeapon in player.currentWeapons)
        {
            WeaponController oldWeaponController = oldWeapon.GetComponent<WeaponController>();
            switch(index.HasValue)
            {
                case true:
                {
                    SetWeaponPosition(weapon);
                    player.currentWeapons[index.Value] = weapon;
                    break;
                }
                case false:
                {
                    if (oldWeaponController.weapon.type == newWeaponController.weapon.type)
                    {
                        oldWeaponController.LevelUp();
                    }
                    else
                    {
                        SetWeaponPosition(weapon);
                        player.currentWeapons.Add(weapon);
                    }
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Set the weapon's position and parent gameobject to the correct weapon slot according to the weapon type.
    /// </summary>
    private void SetWeaponPosition(GameObject weapon)
    {
        switch(weapon.GetComponent<WeaponController>().weapon.type)
        {
            case WEAPON_TYPE.PLASMA:
            {
                Transform firstWeapon = transform.Find("First Weapon");
                weapon.transform.position = firstWeapon.position;
                weapon.transform.parent = firstWeapon;
                break;
            }
            case WEAPON_TYPE.LASER:
            {
                Transform secondWeapon = transform.Find("Second Weapon");
                weapon.transform.position = secondWeapon.position;
                weapon.transform.parent = secondWeapon;
                break;
            }
            case WEAPON_TYPE.EXPLOSIVE:
            {
                Transform thirdWeapon = transform.Find("Third Weapon");
                weapon.transform.position = thirdWeapon.position;
                weapon.transform.parent = thirdWeapon;
                break;
            }
            case WEAPON_TYPE.DEFAULT:
            {
                Transform firstWeapon = transform.Find("First Weapon");
                weapon.transform.position = firstWeapon.position;
                weapon.transform.parent = firstWeapon;
                break;
            }
        }
    }

    public void ChangePassiveImprovment(PassiveImprovment passiveImprovment){}

/// <summary>
/// Currently, this fucntion required for the testing phase. 
/// As we are adding new weapons and passive improvments to the game, 
/// we need to reset the player's current weapons, passive improvments 
/// and perks when the application quits.
/// </summary>
    public void OnApplicationQuit() 
    {  
        player.currentWeapons = new List<GameObject>();
        player.currentPassiveImprovments = new List<PassiveImprovment>();
        player.perks = new List<Perk>();
    }
}
