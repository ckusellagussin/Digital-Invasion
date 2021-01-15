using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        // Dropdown current = UnitScript.toggles[dropdownN].gameObject.GetCompontent<Dropdown>();
        // if (current.value == 0){
        // 
        // TurnScript.goodList[locator].armour = 5; - Find a value
        //
        // set stats for rifleman
        //}

        // if (dropdown.value == 1){
        // set stats for Scout
        //}

    }
}
