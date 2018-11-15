using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class RuleCreator : MonoBehaviour
{
    [SerializeField] protected GameObject parserPrefb;
    [SerializeField] protected LevelEditor editor;
    [SerializeField] protected CustomDropdown dropDown;
    [SerializeField] protected Text descriptionDisplay;
    [SerializeField] protected ScrollRect scrollRect;

    protected Dictionary<string, Type> typeMapping;
    protected Type chosenType;
    protected List<Parser> parserList;

    private void Start()
    {
        //set up
        typeMapping = new Dictionary<string, Type>();
        dropDown.OnSelectedOptionChanged.AddListener(OnOptionSelected);
        parserList = new List<Parser>();
        Initialize();
    }

    protected abstract void Initialize();

    void OnOptionSelected()
    {
        string opt = dropDown.GetSelectedOption();
        if (typeMapping.ContainsKey(opt))
        {
            chosenType = typeMapping[opt];
            DescriptionAttribute att = chosenType.GetCustomAttribute<DescriptionAttribute>();
            if (att != null)
                descriptionDisplay.text = att.Description;
            SetupParseableContent();
        }
    }

    void SetupParseableContent()
    {
        parserList.Clear();
        //destroy existing children
        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            Destroy(scrollRect.content.GetChild(i).gameObject);
        }

        //setup new 
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
        BindingFlags.Static | BindingFlags.Instance |
        BindingFlags.DeclaredOnly;
        FieldInfo[] fields = chosenType.GetFields(flags);

        foreach (FieldInfo field in fields)
        {
            ParserTypeAttribute att = field.GetCustomAttribute<ParserTypeAttribute>();

            if (att == null)//no at no inputing
                continue;

            GameObject obj = Instantiate(parserPrefb, scrollRect.content);

            Parser p = obj.GetComponent<Parser>();
            p.Initialize(att);
            parserList.Add(p);
        }
        //set content size
        scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollRect.content.childCount * (parserPrefb.transform as RectTransform).rect.height);
    }

    public virtual bool ValidInput()
    {
        if (chosenType == null)
            return false;
        foreach (Parser p in parserList)
            if (!p.HasValue())
                return false;
        return true;
    }

    public abstract void Save();
    protected abstract string MakeData();
}
