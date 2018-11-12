using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ValueGneratorCreator : MonoBehaviour {

    [SerializeField]List<string> options;
    [SerializeField] CustomDropdown dropDown;
    Dictionary<string, Type> optionsMapping;

    Type chosenType;

    private void Awake()
    {
        optionsMapping = new Dictionary<string, Type>();

        dropDown.OnSelectedOptionChanged.AddListener(() => { chosenType = optionsMapping[dropDown.GetSelectedOption()]; Debug.Log(chosenType.ToString()); });

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

}
