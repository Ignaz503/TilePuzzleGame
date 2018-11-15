using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MergeEffectCreator : RuleCreator {


	// Use this for initialization
	protected override void Initialize() {

        Assembly ass = Assembly.GetAssembly(typeof(BaseMergeEffect));
        foreach (Type t in ass.GetTypes().Where(t => t.IsSubclassOf(typeof(BaseMergeEffect))))
        {
            string s = t.ToString();

            s = string.Concat(s.Select(x => char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');

            typeMapping.Add(s, t);

            dropDown.AddOption(s);
        }
    }

    public override void Save()
    {
        if (ValidInput())
        {
            editor.SetMergeEffect(MakeData());
        }
        else
        {
            WarningWindow.Instance.GiveWarning("Merge Effect", "The merge effect is currently not valid");
        }
    }

    protected override string MakeData()
    {
        string s = chosenType.ToString() + "\n";

        foreach(Parser p in parserList)
        {
            s += p.GetParsedValueAsString() + "\n";
        }
        return s;
    }
}
