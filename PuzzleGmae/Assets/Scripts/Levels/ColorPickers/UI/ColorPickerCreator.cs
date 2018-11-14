using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerCreator : MonoBehaviour {


    [SerializeField] GameObject ColorPickerPrefab;
    [SerializeField] Dictionary<ColorCreator,Color> colors;
    [SerializeField] ScrollRect scrollRect;

    private void Awake()
    {
        colors = new Dictionary<ColorCreator, Color>();   
    }

    public void AddNewColorToPallet()
    {
        GameObject obj = Instantiate(ColorPickerPrefab,scrollRect.content);

        scrollRect.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, scrollRect.content.childCount * (obj.transform as RectTransform).rect.height + 10f);

        ColorCreator creator = obj.GetComponent<ColorCreator>();

        colors.Add(creator, creator.GetColor());

        creator.OnColorChanged += (c) => { colors[creator] = c; };
    }

    public ColorPicker GetColorPicker()
    {
        return new ColorPickerValueAsIndex(colors.Values.ToArray());
    }

    public bool ValidatePicker()
    {
        return colors.Values.Count > 1;
    }
}
