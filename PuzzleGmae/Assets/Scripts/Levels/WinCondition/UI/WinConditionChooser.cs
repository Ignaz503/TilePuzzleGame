using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditionChooser : ConditionChooser
{
    public override void Save()
    {
        base.Save();
        editor.SetWinCondition(MakeData());
    }
}
