using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parser : MonoBehaviour
{
    [SerializeField] protected InputField inputField;
    [SerializeField] protected Text parserName;
    string inputedValue;

    private void Awake()
    {
        inputedValue = "";
        inputField.onEndEdit.AddListener(str => inputedValue = str);
    }

    public void Initialize(ParserTypeAttribute type)
    {
        parserName.text = type.DisplayName;
        switch (type.Type)
        {
            case ParserTypeAttribute.ParseableTypes.FLOAT:
                inputField.characterValidation = InputField.CharacterValidation.Decimal;
                break;
            case ParserTypeAttribute.ParseableTypes.INTEGER:
                inputField.characterValidation = InputField.CharacterValidation.Integer;
                break;
            case ParserTypeAttribute.ParseableTypes.STRING:
                inputField.characterValidation = InputField.CharacterValidation.Alphanumeric;
                break;
            default:
                throw new System.Exception("Can't parse value type");
        }
    }

    public string GetParsedValueAsString()
    {
        return inputedValue;
    }

    public bool HasValue()
    {
        return inputedValue != "" && inputedValue != null;
    }
}