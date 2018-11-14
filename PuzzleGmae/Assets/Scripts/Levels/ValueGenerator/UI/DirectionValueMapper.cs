using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirectionValueMapper : MonoBehaviour {

    [SerializeField] InputField upInput;
    [SerializeField] InputField downInput;
    [SerializeField] InputField rightInput;
    [SerializeField] InputField leftInput;

    DirectionMapping mapping;
    // Use this for initialization

    void Start () {
        mapping = new DirectionMapping();
        InitializeInputFields();
	}

    void InitializeInputFields()
    {
        upInput.onEndEdit.AddListener((str) => ValidateInput(upInput, str, Direction.Up));
        downInput.onEndEdit.AddListener((str) => ValidateInput(downInput, str, Direction.Down));
        rightInput.onEndEdit.AddListener((str) => ValidateInput(rightInput, str, Direction.Right));
        leftInput.onEndEdit.AddListener((str) => ValidateInput(leftInput, str, Direction.Left));
    }

    void ValidateInput(InputField f, string str, Direction dir)
    {
        //try parse
        int parse;
        if (int.TryParse(str, out parse))
        {
            mapping.AddValue(dir, parse);
        }
        else
        {
            f.text = "";
        }
    }

    public bool CheckMappingValid()
    {
        return mapping.ValidateMapping();
    }

    public DirectionMapping GetMapping()
    {
        return mapping;
    }
}
