using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection; 
using System.Linq; 
using UnityEngine;
using UnityEngine.UI;

public abstract class ConditionChooser : MonoBehaviour {

    [SerializeField] GameObject parserPrefb;

    [SerializeField] protected LevelEditor editor;
    [SerializeField] protected ConditionAttribute.ConditionType ConditionType;
    [SerializeField] protected CustomDropdown conditionDropDown;
    [SerializeField] protected Text descriptionDisplay;
    [SerializeField] protected ScrollRect scrollContent;

    Dictionary<string, Type> typeMapping;
    List<Parser> parserList;

    Type chosenType;

    private void Start()
    {
        typeMapping = new Dictionary<string, Type>();
        conditionDropDown.OnSelectedOptionChanged.AddListener(OnOptionSelected);
        parserList = new List<Parser>();
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

            conditionDropDown.AddOption(s);
        }
    }

    void SetupParseableContent()
    {
        parserList.Clear();
        //destroy existing children
        for (int i = 0; i < scrollContent.content.childCount; i++)
        {
            Destroy(scrollContent.content.GetChild(i).gameObject);
        }

        //setup new 
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | 
        BindingFlags.Static | BindingFlags.Instance |
        BindingFlags.DeclaredOnly;
        FieldInfo[] fields = chosenType.GetFields(flags);

        foreach(FieldInfo field in fields)
        {
            ParserTypeAttribute att = field.GetCustomAttribute<ParserTypeAttribute>();

            if (att == null)//no at no inputing
                continue;

            GameObject obj = Instantiate(parserPrefb,scrollContent.content);

            Parser p = obj.GetComponent<Parser>();
            p.Initialize(att);
            parserList.Add(p);
        }
        //set content size
        scrollContent.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollContent.content.childCount * (parserPrefb.transform as RectTransform).rect.height);
    }

    void OnOptionSelected()
    {
        string opt = conditionDropDown.GetSelectedOption();
        if (typeMapping.ContainsKey(opt))
        {
            chosenType= typeMapping[opt];
            DescriptionAttribute att = chosenType.GetCustomAttribute<DescriptionAttribute>();
            if (att != null)
                descriptionDisplay.text = att.Description;
            SetupParseableContent();
        }
    }

    protected string MakeData()
    {
        string data = chosenType.ToString() + "\n";

        foreach(Parser p in parserList)
        {
            data += p.GetParsedValueAsString() + "\n";
        }
        return data;
    }

    public virtual void Save()
    {
        if (!ValidInput())
        {
            WarningWindow.Instance.GiveWarning("Conditions not valid", "The conditions you chose are not valid in their current form. Check if you still need to enter any data");
            return;
        }
    }

    public bool ValidInput()
    {
        if (chosenType == null)
            return false;
        foreach (Parser p in parserList)
            if (!p.HasValue())
                return false;
        return true;
    }

}
