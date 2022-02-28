using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    [SerializeField] private MeleeBehavior melee;
    [SerializeField] private PistolBehavior pistol;
    [SerializeField] private ArBehavior rifle;
    [SerializeField] private SniperBehavior sniper;
    [SerializeField] private PlayerController control;
    [SerializeField] private GameObject[] weapons;

    void Update()
    {
        //var input = Input.inputString;
        //switch(input)
        //{
        //    case "1":
        //        weapons[0].SetActive(true);
        //        weapons[1].SetActive(false);
        //        weapons[2].SetActive(false);
        //        weapons[3].SetActive(false);
        //        break;
        //    case "2":
        //        weapons[0].SetActive(false);
        //        weapons[1].SetActive(true);
        //        weapons[2].SetActive(false);
        //        weapons[3].SetActive(false);
        //        break;
        //    case "3":
        //        weapons[0].SetActive(false);
        //        weapons[1].SetActive(false);
        //        weapons[2].SetActive(true);
        //        weapons[3].SetActive(false);
        //        break;
        //    case "4":
        //        weapons[0].SetActive(false);
        //        weapons[1].SetActive(false);
        //        weapons[2].SetActive(false);
        //        weapons[3].SetActive(true);
        //        break;
        //}

        if(control.FirstWeaponInput && !pistol.isAiming && !rifle.isAiming && !sniper.isAiming)
        {
            weapons[0].SetActive(true);
            weapons[1].SetActive(false);
            weapons[2].SetActive(false);
            weapons[3].SetActive(false);
        }
        else if (control.SecondWeaponInput && !pistol.isAiming && !rifle.isAiming && !sniper.isAiming)
        {
            weapons[0].SetActive(false);
            weapons[1].SetActive(true);
            weapons[2].SetActive(false);
            weapons[3].SetActive(false);
        }
        else if (control.ThirdWeaponInput && !pistol.isAiming && !rifle.isAiming && !sniper.isAiming)
        {
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            weapons[2].SetActive(true);
            weapons[3].SetActive(false);
        }
        else if (control.FourthWeaponInput && !pistol.isAiming && !rifle.isAiming && !sniper.isAiming)
        {
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            weapons[2].SetActive(false);
            weapons[3].SetActive(true);
        }
    }
}
