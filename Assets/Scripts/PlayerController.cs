using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponUpgradeEventArgs : EventArgs
{
    public WeaponType Weapon { get; set; }
    public int Level { get; set; }
}

public class PlayerController : MonoBehaviour
{

    public string nickname = "";
    public int deviceID = -1;
    public Color playerColor;

    private int cash; // Use AddRemoveCash
    public float HP => Bot.HealthPercentage;

    public BotBase Bot { get; set; }

    public event EventHandler<WeaponUpgradeEventArgs> WeaponUpgraded;

	//public List<Weapon> WeaponList = new List<Weapon>();
    public Dictionary<WeaponType, Weapon> WeaponLevel { get; set; } = new Dictionary<WeaponType, Weapon>();

    public void AddRemoveCash(int changedCash)
    {
        cash += changedCash;

        // todo Send airconsole messages to update phones
    }

    // needs reference to what weapons this has

    private void AddWeapon(WeaponType weapon)
    {
        if (WeaponLevel.Keys.Any(x => x == weapon))
        {
            WeaponLevel[weapon].Level += 1;
            WeaponUpgraded?.Invoke(this, new WeaponUpgradeEventArgs()
			{
				Weapon = weapon, Level = WeaponLevel[weapon].Level
			});
        }
        else
        {
			// TODO: figure out how to add a weapon component as child of BotBase dynamically
			// Apparently this means finding the first instance of a child prefab that matches WeaponType
			GetComponentsInChildren<Weapon>(true).First(x => x.TypeOfWeapon == weapon)?.gameObject.SetActive(true);
            WeaponUpgraded?.Invoke(this, new WeaponUpgradeEventArgs() { Weapon = weapon, Level = 1 });
        }
    }

    public Result ButtonInput(string input)
    {
        switch (input)
        {
            case "buy_flamethrower":
                return BuyWeapon(WeaponType.Flamethrower);
            case "buy_sword":
                return BuyWeapon(WeaponType.Sword);
            case "buy_boxglove":
                return BuyWeapon(WeaponType.BoxGlove);
            case "buy_cannon":
                return BuyWeapon(WeaponType.Cannon);
            case "buy_mines":
                return BuyWeapon(WeaponType.Mines);
            case "buy_shurikens":
                return BuyWeapon(WeaponType.Shurikens);

            case "repair_flamethrower":
                return BuyRepair(WeaponType.Flamethrower);
            case "repair_sword":
                return BuyRepair(WeaponType.Sword);
            case "repair_boxglove":
                return BuyRepair(WeaponType.BoxGlove);
            case "repair_cannon":
                return BuyRepair(WeaponType.Cannon);
            case "repair_mines":
                return BuyRepair(WeaponType.Mines);
            case "repair_shurikens":
                return BuyRepair(WeaponType.Shurikens);

            default:
                return Result.Error;
                /*
                    case "right":
                        anim.SetBool("isRunning", true);
                        rightButton = true;
                        spr.flipX = false;
                        break;
                    case "left":
                        anim.SetBool("isRunning", true);
                        leftButton = true;
                        spr.flipX = true;
                        break;
                    case "right-up":
                        rightButton = false;
                        anim.SetBool("isRunning", false);
                        break;
                    case "left-up":
                        leftButton = false;
                        anim.SetBool("isRunning", false);
                        break;
                    case "jump":
                        jumpButton = true;
                        anim.SetBool("isJumping", true);
                        break;
                    case "jump-up":
                        jumpButton = false;
                        break;
                    case "interact":
                        interactButton = true;
                        break;
                */
        }
    }

    private Result BuyWeapon(WeaponType weaponType)
    {
        if (cash >= Constants.WeaponCost[weaponType][WeaponLevel[weaponType].Level + 1])
        {
            cash -= Constants.WeaponCost[weaponType][WeaponLevel[weaponType].Level + 1];
            AddWeapon(weaponType);
            Debug.Log(weaponType.ToString() + " BOUGHT");
            return Result.Success;
        }
        return Result.NotEnoughMoney;
    }

    private Result BuyRepair(WeaponType repairType)
    {
        if (cash >= Constants.RepairCost[repairType])
        {
            cash -= Constants.RepairCost[repairType];
            Bot.HealDamage(10f);
            Debug.Log(repairType.ToString() + " BOUGHT");
            return Result.Success;
        }
        return Result.NotEnoughMoney;
    }
}
