using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection; 
using System.Linq; 
using UnityEngine;
using UnityEngine.UI;

public abstract class ConditionChooser : RuleCreator {


    [SerializeField] protected ConditionAttribute.ConditionType ConditionType;

    protected override void Initialize()
    {
        // setup drop down
        Assembly ass = Assembly.GetAssembly(typeof(Condition));
        foreach (Type t in ass.GetTypes().Where(t => t.IsSubclassOf(typeof(Condition))))
        {
            ConditionAttribute attribute = t.GetCustomAttribute<ConditionAttribute>();

            if (attribute == null || attribute.Type != ConditionType)
                continue;

            string s = t.ToString();

            s = string.Concat(s.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

            typeMapping.Add(s, t);

            dropDown.AddOption(s);
        }
    }

    protected override string MakeData()
    {
        string data = chosenType.ToString() + "\n";

        foreach(Parser p in parserList)
        {
            data += p.GetParsedValueAsString() + "\n";
        }
        return data;
    }

    public override void Save()
    {
        if (!ValidInput())
        {
            WarningWindow.Instance.GiveWarning("Conditions not valid", "The conditions you chose are not valid in their current form. Check if you still need to enter any data");
            return;
        }
    }
}
