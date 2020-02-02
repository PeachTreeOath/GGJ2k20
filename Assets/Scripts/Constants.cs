using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
	Flamethrower,
	Sword,
    Cannon,
    Mines,
    BoxGlove,
    Shurikens
}

public enum RepairType
{
	Shield,
	Body
}

public enum Result
{
	Success,
	NotEnoughMoney,
	NotAllowed,
	Error
}

public class Constants : Singleton<Constants>
{
	public static readonly Dictionary<WeaponType, Dictionary<int, int>> WeaponCost = new Dictionary<WeaponType, Dictionary<int, int>>()
	{
		{ WeaponType.BoxGlove, new Dictionary<int, int>() {
			{ 1, 10 },
			{ 2, 20 },
			{ 3, 30 } } },
		{ WeaponType.Cannon, new Dictionary<int, int>() {
			{ 1, 10 },
			{ 2, 20 },
			{ 3, 30 } } },
		{ WeaponType.Flamethrower, new Dictionary<int, int>() {
			{ 1, 10 },
			{ 2, 20 },
			{ 3, 30 } } },
        { WeaponType.Mines, new Dictionary<int, int>() {
            { 1, 10 },
            { 2, 20 },
            { 3, 30 } } },
        { WeaponType.Shurikens, new Dictionary<int, int>() {
            { 1, 10 },
            { 2, 20 },
            { 3, 30 } } },
        { WeaponType.Sword, new Dictionary<int, int>() {
            { 1, 10 },
            { 2, 20 },
            { 3, 30 } } }
    };

	public static readonly Dictionary<WeaponType, int> RepairCost = new Dictionary<WeaponType, int>()
	{
		{ WeaponType.BoxGlove, 1 },
		{ WeaponType.Cannon, 1 },
        { WeaponType.Flamethrower, 1 },
        { WeaponType.Mines, 1 },
        { WeaponType.Shurikens, 1 },
        { WeaponType.Sword, 1 },
    };
}
