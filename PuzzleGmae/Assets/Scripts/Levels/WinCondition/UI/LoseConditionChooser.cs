using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseConditionChooser : ConditionChooser
{
    public override void Save()
    {
        base.Save();
        editor.SetLoseCondition(MakeData());
    }
}
