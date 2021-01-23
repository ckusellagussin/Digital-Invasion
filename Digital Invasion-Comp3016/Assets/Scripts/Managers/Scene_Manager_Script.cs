﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Manager_Script : MonoBehaviour
{
    public GameObject Page1;
    public GameObject Page2;
    public GameObject BattleUI;
    public GameObject Camera1;
    public GameObject Camera2;
    public Unit_Selection_Script UnitSelect;
    public Turn_Script TurnScript;
    public GameObject GoodGuy;
    public GameObject BadGuy;

    public GameObject[] weaponsPrefabs;
    public Sprite[] weaponsIcons;

    public void Play()
    {
        int ID = 0;
        int negative = 0;
        for (int i = 0; i < UnitSelect.toggles.Count; i++)
        {
            if (UnitSelect.toggles[i].isOn)
            {
                CreateGoodGuy(ID, i);
                TurnScript.goodList[ID].gameObject.SetActive(true);
                TurnScript.badList[ID].gameObject.SetActive(true);
                ID += 1;
                Debug.Log("Good Guy made");
            }
            else
            {
                TurnScript.allList.Remove(TurnScript.goodList[i - negative]);
                TurnScript.goodList[i - negative].gameObject.SetActive(false);
                TurnScript.goodList.RemoveAt(i - negative);
                TurnScript.allList.Remove(TurnScript.badList[i - negative]);
                TurnScript.badList[i - negative].gameObject.SetActive(false);
                TurnScript.badList.RemoveAt(i - negative);
                negative += 1;
                Debug.Log("Good Guy KILLED");
            }
        }
        Camera2.SetActive(true);
        Camera1.SetActive(false);
        Page2.SetActive(false);
        BattleUI.SetActive(true);
    }

    public void PlayAgain()
    {
        Camera1.SetActive(true);
        Camera2.SetActive(false);
        BattleUI.SetActive(false);
        Page1.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void switch_Pages(int PageNo)
    {
        if (PageNo == 0)
        {
            Page1.SetActive(true);
            Page2.SetActive(false);
        }
        if (PageNo == 1)
        {
            Page1.SetActive(false);
            Page2.SetActive(true);
        }
    }

    public void CreateGoodGuy(int locator, int dropdownN)
    {
        Debug.Log(locator);
        Dropdown current = UnitSelect.dropdowns[dropdownN];
        Debug.Log(current.value);

        if (current.value == 0)
        {

            TurnScript.goodList[locator].maxDistance = 100;
            TurnScript.goodList[locator].viewDistance = 10;
            TurnScript.goodList[locator].maxRange = 10;
            TurnScript.goodList[locator].maxHealth = 20;
            TurnScript.goodList[locator].armour = 5;
            TurnScript.goodList[locator].armourPiercing = 3;
            TurnScript.goodList[locator].damage = 10;

            GameObject handBone = TurnScript.goodList[locator].weaponModel.GetComponent<Weapon_Controller_Script>().HandBone;
            Destroy(TurnScript.goodList[locator].weaponModel);
            TurnScript.goodList[locator].weaponModel = Instantiate(weaponsPrefabs[0], TurnScript.goodList[locator].transform);
            TurnScript.goodList[locator].weaponModel.GetComponent<Weapon_Controller_Script>().HandBone = handBone;
            TurnScript.goodList[locator].classIcon.sprite = weaponsIcons[0];
            TurnScript.goodList[locator].unitClass = "Rifleman";
            TurnScript.goodList[locator].weaponRange.transform.localScale = new Vector3(10 * 2, 5, 10 * 2);
        }

        if (current.value == 1)
        {
            //scout
            TurnScript.goodList[locator].maxDistance = 150;
            TurnScript.goodList[locator].viewDistance = 15;
            TurnScript.goodList[locator].maxRange = 15;
            TurnScript.goodList[locator].maxHealth = 20;
            TurnScript.goodList[locator].armour = 0;
            TurnScript.goodList[locator].armourPiercing = 0;
            TurnScript.goodList[locator].damage = 10;


            GameObject handBone = TurnScript.goodList[locator].weaponModel.GetComponent<Weapon_Controller_Script>().HandBone;
            Destroy(TurnScript.goodList[locator].weaponModel);
            TurnScript.goodList[locator].weaponModel = Instantiate(weaponsPrefabs[1], TurnScript.goodList[locator].transform);
            TurnScript.goodList[locator].weaponModel.GetComponent<Weapon_Controller_Script>().HandBone = handBone;
            TurnScript.goodList[locator].classIcon.sprite = weaponsIcons[1];
            TurnScript.goodList[locator].unitClass = "Scout";
            TurnScript.goodList[locator].weaponRange.transform.localScale = new Vector3(15 * 2, 5, 15 * 2);
        }
        if (current.value == 2)
        {
            //specialist
            TurnScript.goodList[locator].maxDistance = 100;
            TurnScript.goodList[locator].viewDistance = 10;
            TurnScript.goodList[locator].maxRange = 7;
            TurnScript.goodList[locator].maxHealth = 20;
            TurnScript.goodList[locator].armour = 5;
            TurnScript.goodList[locator].armourPiercing = 0;
            TurnScript.goodList[locator].damage = 15;


            GameObject handBone = TurnScript.goodList[locator].weaponModel.GetComponent<Weapon_Controller_Script>().HandBone;
            Destroy(TurnScript.goodList[locator].weaponModel);
            TurnScript.goodList[locator].weaponModel = Instantiate(weaponsPrefabs[2], TurnScript.goodList[locator].transform);
            TurnScript.goodList[locator].weaponModel.GetComponent<Weapon_Controller_Script>().HandBone = handBone;
            TurnScript.goodList[locator].classIcon.sprite = weaponsIcons[2];
            TurnScript.goodList[locator].unitClass = "Specialist";
            TurnScript.goodList[locator].weaponRange.transform.localScale = new Vector3(7 * 2, 5, 7 * 2);
        }
        if (current.value == 3)
        {
            //support
            TurnScript.goodList[locator].maxDistance = 80;
            TurnScript.goodList[locator].viewDistance = 10;
            TurnScript.goodList[locator].maxRange = 10;
            TurnScript.goodList[locator].maxHealth = 20;
            TurnScript.goodList[locator].armour = 7;
            TurnScript.goodList[locator].armourPiercing = 5;
            TurnScript.goodList[locator].damage = 10;


            GameObject handBone = TurnScript.goodList[locator].weaponModel.GetComponent<Weapon_Controller_Script>().HandBone;
            Destroy(TurnScript.goodList[locator].weaponModel);
            TurnScript.goodList[locator].weaponModel = Instantiate(weaponsPrefabs[3], TurnScript.goodList[locator].transform);
            TurnScript.goodList[locator].weaponModel.GetComponent<Weapon_Controller_Script>().HandBone = handBone;
            TurnScript.goodList[locator].classIcon.sprite = weaponsIcons[3];
            TurnScript.goodList[locator].unitClass = "Support";
            TurnScript.goodList[locator].weaponRange.transform.localScale = new Vector3(10 * 2, 5, 10 * 2);
        }

    }
}
