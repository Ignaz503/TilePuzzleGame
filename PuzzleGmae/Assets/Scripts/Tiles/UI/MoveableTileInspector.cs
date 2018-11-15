using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveableTileInspector : TileInspector
{
    [SerializeField] CustomDropdown colorDropDown;
    [SerializeField] Sprite dropwDownSprite;
    [SerializeField] Image selectedImage;
    MoveableTile moveableTileInspected;
    BaseValueAndColorGenerator gen;

    public void Awake()
    {
        tilePlacer.Editor.OnNewValueAndColorGen += OnNewGenerator;

        OnFadeIn += FadeIn;
        colorDropDown.OnSelectedOptionChanged.AddListener(OnOptionSelected);
    }

    public override void EnableInspector(Tile t)
    {
        base.EnableInspector(t);

        if (!(t is MoveableTile))
            throw new System.Exception($"Trying to inspect wrong type of tile, can only inspect {typeof(MoveableTile)} trying to inspect {t.GetType()}");
        moveableTileInspected = t as MoveableTile;
        selectedImage.color = t.TileColor;
    }

    private void FadeIn()
    {
        if (colorDropDown.OptionsCount == 0)
        {
            WarningWindow.Instance?.GiveWarning("Create Value And Color Generator First", "There are currently no colors you can assign your placed tiles. To change that you first need to create a valid value and color generator and save it");
            colorDropDown.gameObject.SetActive(false);
        }
        else
            colorDropDown.gameObject.SetActive(true);
    }

    void OnNewGenerator(BaseValueAndColorGenerator gen)
    {
        this.gen = gen;
        //set drop down colors
        for (int i = 0; i < gen.ColorPicker.Colors.Length; i++)
        {
            Color c = gen.ColorPicker.Colors[i];
            colorDropDown.AddOption(name = $"{i}", c,dropwDownSprite); 
        }
    }

    void OnOptionSelected()
    {
        //god is this fucking ugly
        string s = colorDropDown.GetSelectedOption();

        int value = int.Parse(s);
        if(gen != null)
        {
            moveableTileInspected.ChangeColor(gen.ColorPicker.GetColor(value));
            selectedImage.color = gen.ColorPicker.GetColor(value);
            tilePlacer.UpdateValue(moveableTileInspected.GridPosition.ToVec2IntXY(), value);
        }
    }

}
