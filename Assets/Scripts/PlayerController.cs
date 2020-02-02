using System;
using System.Collections;
using System.Collections.Generic;
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

    private int cash; // Use AddRemoveCash
    public float HP => Bot.HealthPercentage;

	public BotBase Bot { get; set; }

	public event EventHandler<WeaponUpgradeEventArgs> WeaponUpgraded;

	public Dictionary<WeaponType, int> WeaponLevel { get; set; } = new Dictionary<WeaponType, int>();

    public void AddRemoveCash(int changedCash)
    {
        cash += changedCash;

        // todo Send airconsole messages to update phones
    }

	// needs reference to what weapons this has

	private void AddWeapon(WeaponType weapon)
	{
		if (new List<WeaponType>(WeaponLevel.Keys).Contains(weapon))
		{
			WeaponLevel[weapon] += 1;
			WeaponUpgraded?.Invoke(this, new WeaponUpgradeEventArgs() { Weapon = weapon, Level = WeaponLevel[weapon] });
		}
		else
		{
			WeaponLevel.Add(weapon, 1);
			WeaponUpgraded?.Invoke(this, new WeaponUpgradeEventArgs() { Weapon = weapon, Level = 1 });
		}
	}

    public Result ButtonInput(string input)
    {
        switch (input)
        {
            case "buy_flamethrower":
                return BuyWeapon(WeaponType.Flamethrower);
			case "buy_knife":
				return BuyWeapon(WeaponType.Knife);
			case "buy_nuke":
				return BuyWeapon(WeaponType.Nuke);
			case "repair_shield":
				return BuyRepair(RepairType.Shield);
			case "repair_body":
				return BuyRepair(RepairType.Body);
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
		if (cash >= Constants.WeaponCost[weaponType][WeaponLevel[weaponType] + 1])
		{
			cash -= Constants.WeaponCost[weaponType][WeaponLevel[weaponType] + 1];
			AddWeapon(weaponType);
			Debug.Log(weaponType.ToString() + " BOUGHT");
			return Result.Success;
		}
		return Result.NotEnoughMoney;
	}

	private Result BuyRepair(RepairType repairType)
	{
		if (cash >= Constants.RepairCost[repairType])
		{
			cash -= Constants.RepairCost[repairType];
			Bot.TakeDamage(-10f);
			Debug.Log(repairType.ToString() + " BOUGHT");
			return Result.Success;
		}
		return Result.NotEnoughMoney;
	}
}
