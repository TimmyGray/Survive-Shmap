using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Weapons;
using PassiveImprovments;

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
        foreach (GameObject weapon in player.allWeapons)
        {
            ChangeWeapon(Instantiate(weapon, transform.position, Quaternion.identity));
        }

        // Initialize the player's current passive improvments
        // Only for the testing phase
        foreach (GameObject passiveImprovment in player.allPassiveImprovments)
        {
            ChangePassiveImprovment(Instantiate(passiveImprovment, transform.position, Quaternion.identity));
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
            else if (isWeaponExist)
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
    /// <param name="weapon">The weapon to be changed.</param>
    /// <param name="index">The index of the weapon to be changed. If null, the weapon will be added to the player's current weapons or level up if the weapon is already in the player's current weapons.</param>
    public void ChangeWeapon(GameObject weapon, int? index = null)
    {
        if(player.currentWeapons.Count==0)
        {
            SetWeaponPosition(weapon);
            player.currentWeapons.Add(weapon);
        }
        else
        {
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
                    WeaponController newWeaponController = weapon.GetComponent<WeaponController>();
                    GameObject oldWeapon = player.currentWeapons.Find(oldWeapon => oldWeapon.GetComponent<WeaponController>().weapon.type == newWeaponController.weapon.type);
                    if (oldWeapon != null)
                    {
                        WeaponController oldWeaponController = oldWeapon.GetComponent<WeaponController>();
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
    /// <param name="weapon">The weapon to be set.</param>
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

    /// <summary>
    /// Set the passive improvment's position and parent gameobject to the correct passive improvment slot.
    /// </summary>
    /// <param name="passiveImprovment">The passive improvment to be set.</param>
    public void SetPassiveImprovmentPosition(GameObject passiveImprovment)
    {
        Transform passiveImprovmentSlot = transform.Find("Passive Improvment");
        passiveImprovment.transform.position = passiveImprovmentSlot.position;
        passiveImprovment.transform.parent = passiveImprovmentSlot;
    }

    /// <summary>
    /// Change the player's current passive improvment. Add, Change or Level up the passive improvment depending on the situation.
    /// Should be called when the player levels up from the level up menu.
    /// </summary>
    /// <param name="passiveImprovment">The passive improvment to be changed.</param>
    /// <param name="index">The index of the passive improvment to be changed. If null, the passive improvment will be added to the player's current passive improvments or level up if the passive improvment is already in the player's current passive improvments.</param>
    public void ChangePassiveImprovment(GameObject passiveImprovment, int? index = null)
    {
        PassiveImprovmentController newPassiveImprovmentController = passiveImprovment.GetComponent<PassiveImprovmentController>();
        GameObject oldPassiveImprovment = player.currentPassiveImprovments.Find(oldPassiveImprovment => oldPassiveImprovment.GetComponent<PassiveImprovmentController>().passiveImprovment.type == newPassiveImprovmentController.passiveImprovment.type);

        if(player.currentPassiveImprovments.Count==0)
        {
            SetPassiveImprovmentPosition(passiveImprovment);
            player.currentPassiveImprovments.Add(passiveImprovment);
            newPassiveImprovmentController.Activate();
        }
        else
        {
            switch(index.HasValue)
            {
                case true:
                {
                    oldPassiveImprovment.GetComponent<PassiveImprovmentController>().Deactivate(true);
                    SetPassiveImprovmentPosition(passiveImprovment);
                    player.currentPassiveImprovments[index.Value] = passiveImprovment;
                    newPassiveImprovmentController.Activate();
                    break;
                }
                case false:
                {
                    if (oldPassiveImprovment != null)
                    {
                        oldPassiveImprovment.GetComponent<PassiveImprovmentController>().LevelUp();
                    }
                    else
                    {
                        SetPassiveImprovmentPosition(passiveImprovment);
                        player.currentPassiveImprovments.Add(passiveImprovment);
                        newPassiveImprovmentController.Activate();
                    }
                    break;
                }
            }
        }
    }

    /// <summary>
    /// Add a perk to the player.
    /// </summary>
    /// <param name="perk">The perk to be added.</param>
    public void AddPerk(GameObject perk)
    {
        PerkController newPerkController = perk.GetComponent<PerkController>();
        newPerkController.ApplyPerk(player);
        player.perks.Add(newPerkController.perk);
    }

    /// <summary>   
    /// Remove a perk from the player.
    /// </summary>
    /// <param name="perk">The perk to be removed.</param>
    public void RemovePerk(GameObject perk)
    {
        PerkController perkController = perk.GetComponent<PerkController>();
        perkController.RemovePerk(player);
        player.perks.Remove(perkController.perk);   
    }
        

    /// <summary>
    /// Get the player's current perks.
    /// </summary>
    public List<Perk> GetPerks()
    {
        return player.perks; 
    }

    /// <summary>
    /// Currently, this fucntion required for the testing phase. 
    /// As we are adding new weapons and passive improvments to the game, 
    /// we need to reset the player's current weapons, passive improvments 
    /// and perks when the application quits.
    /// </summary>
    public void OnApplicationQuit() 
    {  
        player.currentWeapons = new List<GameObject>();
        player.currentPassiveImprovments = new List<GameObject>();
        player.perks = new List<Perk>();
    }
}
