using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCreator : MonoBehaviour {

    public event Action<Color> OnColorChanged;

    [SerializeField] ColorValueInputField R;
    [SerializeField] ColorValueInputField G;
    [SerializeField] ColorValueInputField B;
    [SerializeField] Image colorDisplay;

    private void Start()
    {
        R.OnValueChanged += OnValueChanged;
        G.OnValueChanged += OnValueChanged;
        B.OnValueChanged += OnValueChanged;
    }

    void OnValueChanged(int val)
    {
        colorDisplay.color = new Color32((byte)R.GetValue(), (byte)G.GetValue(), (byte)B.GetValue(),255);

        OnColorChanged?.Invoke(colorDisplay.color);
    }

    public Color GetColor()
    {
        return colorDisplay.color;
    }

}
