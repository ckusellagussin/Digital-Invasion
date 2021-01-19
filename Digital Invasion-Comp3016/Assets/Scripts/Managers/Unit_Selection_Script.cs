using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Unit_Selection_Script : MonoBehaviour
{
    List<string> Classes = new List<string>() { "A well sounded soldier, lightly armored balancing durablity and manuverablity firing at enemies from a fair distance with armor piercing shots. \n \nStats: \nMovement:10 \nArmour:5 \nHealth:20 \ndamage:10 \nVision:10 \nFiring Range:10", "Higher movement and vision range than other units but fragile good for finding the enemy or flanking to put on the pressure!\n \n \nStats: \nMovement:15 \nArmour:0 \nHealth:20 \ndamage:10 \nVision:15 \nFiring Range:15", "Comes packing a shotgun, with high damage but a low firing range get in close using cover and deal the damage you need to  \n \n \nStats: \nMovement:10 \nArmour:5 \nHealth:20 \ndamage:15 \nVision:10 \nFiring Range:5", "Heavily armed and armored the support doesn't move far or fast but can take damage and deal it back with Heavy Armor piercing rounds \n  \nStats: \nMovement:8 \nArmour:7 \nHealth:20 \ndamage:10 \nVision:10 \nFiring Range:10" };
    public List<Toggle> toggles;
    public List<Dropdown> dropdowns;
    public Dropdown Unit1;
    public Dropdown Unit2;
    public Dropdown Unit3;
    public Dropdown Unit4;

    public Text Unit1Text;
    public Text Unit2Text;
    public Text Unit3Text;
    public Text Unit4Text;




    public void start()
    {
        FillList();

        Unit1.value = 0;
        Unit2.value = 0;
        Unit3.value = 0;
        Unit4.value = 0;
    }

   public void FillList()
    {
        Unit1.AddOptions(Classes);
        Unit2.AddOptions(Classes);
        Unit3.AddOptions(Classes);
        Unit4.AddOptions(Classes);

    }
    public void DropdownChanged1(Dropdown Unit1)
    {
        Unit1Text.text = Classes[Unit1.value];
        

    }
    public void DropdownChanged2(Dropdown Unit2)
    {
        Unit2Text.text = Classes[Unit2.value];


    }
    public void DropdownChanged3(Dropdown Unit3)
    {
        Unit3Text.text = Classes[Unit3.value];


    }
    public void DropdownChanged4(Dropdown Unit4)
    {
        Unit4Text.text = Classes[Unit4.value];


    }
}
