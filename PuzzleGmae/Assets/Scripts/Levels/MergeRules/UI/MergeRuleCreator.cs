using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO UNIFY WITH CONDITION CREATION
/// </summary>

public class MergeRuleCreator : RuleCreator
{

    protected override void Initialize()
    {
        Assembly ass = Assembly.GetAssembly(typeof(BaseMergeRule));
        foreach (Type t in ass.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseMergeRule))))
        {
            string s = t.ToString();

            s = string.Concat(s.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

            typeMapping.Add(s, t);

            dropDown.AddOption(s);
        }
    }

    protected override string MakeData()
    {
        string data = chosenType.ToString() + "\n";

        foreach (Parser p in parserList)
        {
            data += p.GetParsedValueAsString() + "\n";
        }
        return data;
    }

    public override void Save()
    {
        if (!ValidInput())
        {
            WarningWindow.Instance.GiveWarning("Merge Rule", "The merge rule defined is not valid. Maybe you forgot to enter certain values?");
            return;
        }

        editor.SetMergeRule(MakeData());
    }
}
