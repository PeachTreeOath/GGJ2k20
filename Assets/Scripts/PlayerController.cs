using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public string nickname = "";
    public int deviceID = -1;

    private int cash; // Use AddRemoveCash
    private int hp;

    public void AddRemoveCash(int changedCash)
    {
        cash += changedCash;

        // todo Send airconsole messages to update phones
    }

    public void ButtonInput(string input)
    {
        switch (input)
        {
            case "buy":
                if (cash > 100)
                {
                    cash -= 100;
                    Debug.Log("GUN BOUGHT");
                }
                break;
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
}
