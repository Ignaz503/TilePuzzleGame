using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ValueGneratorCreator : MonoBehaviour {

    [SerializeField] LevelEditor levelEditor;
    [SerializeField] DemoPanelAnim menuAnim;
    [SerializeField]List<string> options;
    [SerializeField] CustomDropdown dropDown;
    [SerializeField] DirectionValueMapper mapping;
    [SerializeField] ColorPickerCreator colorPallet;
    Dictionary<string, Type> optionsMapping;

    Type chosenType;

    private void Awake()
    {
        optionsMapping = new Dictionary<string, Type>();

        dropDown.OnSelectedOptionChanged.AddListener(() => { chosenType = optionsMapping[dropDown.GetSelectedOption()]; });

        Assembly ass = Assembly.GetAssembly(typeof(BaseValueAndColorGenerator));
        foreach(Type t in ass.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseValueAndColorGenerator))))
        {
            string s = t.ToString();

            s = string.Concat(s.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
            options.Add(s);
            optionsMapping.Add(s, t);

            dropDown.AddOption(s);           
        }
    }

    public void Save()
    {
        if(chosenType == null)
        {
            //info window text + "You need to chose a type"
            WarningWindow.Instance.GiveWarning("No Gnerator Type Chosen", "You have currently no generator type chosen, so it can't be saved.");
            return;
        }
        if (!mapping.CheckMappingValid())
        {
            //info window mapping invalid
            WarningWindow.Instance.GiveWarning("Direction Mapping Invalid", "The direction mapping is currently not in a valid state. Make sure that all directions have integer values assigned");
            return;
        }
        if(!colorPallet.ValidatePicker())
        {
            //info window color pallet not valid
            WarningWindow.Instance.GiveWarning("Collor Pallet Invalid", "The color pallet is currently invalid. Make sure that you have at least two colors in it");
            return;
        }

        levelEditor.SetValueAndColorGen(MakeData());
        menuAnim.newPanel(0);
    }

    string MakeData()
    {
        return chosenType.ToString() + "\n" + colorPallet.GetColorPicker().Serialize() + "\n" + mapping.GetMapping().Serialize();
    }

}
