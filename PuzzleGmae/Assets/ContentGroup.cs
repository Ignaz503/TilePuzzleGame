using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentGroup : MonoBehaviour {

    public List<GameObject> Content;

    //currently Active needs deactivate
    [SerializeField]int currentActive = -1;

    public void ActivateContent(int i)
    {
        if(CheckInRange(currentActive))
            DeactivateContent(currentActive);
        SetContentActive(true, i);
        currentActive = i;
    }

    public bool CheckInRange(int i)
    {
        return i < Content.Count && i >= 0;
    }

    void SetContentActive(bool val, int i)
    {
        if (CheckInRange(i))
        {
            Content[i].SetActive(val);
        }
        else
            throw new System.Exception($"{i} is not in range of {Content.Count}");
    }

    public void DeactivateContent(int i)
    {
        if (i == currentActive)
            currentActive = - 1;
        SetContentActive(false, i);
    }

    public void ToggleContent(int i)
    {
        if (CheckInRange(i))
        {
            Content[i].SetActive(!Content[i].activeSelf);
            if (Content[i].activeSelf)//it is now active
                currentActive = i;
            else if (currentActive == i)//contetn was set deactive and it was current active
                currentActive = -1; 
        }
    }

    public void DeactivateAll()
    {
        for (int i = 0; i < Content.Count; i++)
        {
            DeactivateContent(i);
        }
    }
}
