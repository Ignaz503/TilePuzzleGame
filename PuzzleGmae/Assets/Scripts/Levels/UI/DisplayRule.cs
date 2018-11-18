using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRule : MonoBehaviour
{
    [SerializeField] GameMaster gm;
    [SerializeField] Animator animator;
    [SerializeField] Text display;
    [SerializeField] Text title;

    public void DisplayMergeRuleDescription()
    {
        display.text = gm.GetMergeRuleDescription();
        title.text = "Merge Rule";
        FadeIn();
    }

    public void DisplayMergeEffectDescription()
    {
        display.text = gm.GetMergeEffectDescription();
        title.text = "Merge Effect";
        FadeIn();
    }

    public void DisplayWinConditionDescription()
    {
        display.text = gm.GetWinConditionDescription();
        title.text = "Win Condition";
        FadeIn();
    }

    public void DisplayLoseConditionDescription()
    {
        display.text = gm.GetLoseConditionDescription();
        title.text = "Lose Condition";
        FadeIn();
    }

    public void DisplayValueAndColorGeneratorDescription()
    {
        display.text =  gm.GetValueAndColorGeneratorDescription();
        title.text = "Value And Color Generator";
        FadeIn();
    }

    void FadeIn()
    {
        animator.Play("Fade-in");
    }

}
