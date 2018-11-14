using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorValueInputField : MonoBehaviour
{
    public event Action<int> OnValueChanged;
    [SerializeField] InputField inputField;
    [SerializeField] Slider slider;

    int currentValue = 0;
    public int CurrentValue {
        get { return currentValue; }
        protected set
        {
            currentValue = value;
            OnValueChanged?.Invoke(currentValue);
        }
    }
    int oldValue;

    private void Start()
    {
        Initiaize();
    }

    void Initiaize()
    {
        slider.onValueChanged.AddListener(SliderChange);

        inputField.onEndEdit.AddListener(OnEndEdit);

        inputField.text = slider.value.ToString();
        oldValue = (int)slider.value;
    }

    void SliderChange(float number)
    {
        oldValue = CurrentValue = (int)number;

        inputField.text = CurrentValue.ToString();

    }

    void OnEndEdit(string input)
    {
        int res;
        if (int.TryParse(input, out res))
        {
            CurrentValue = res;
            slider.value = res;
        }
        else
        {
            CurrentValue = oldValue;
            inputField.text = currentValue.ToString();
        }
    }

    public int GetValue()
    {
        return CurrentValue;
    }

}
