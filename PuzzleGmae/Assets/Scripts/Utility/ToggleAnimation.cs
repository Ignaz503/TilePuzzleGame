using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string[] clipNames;
    [SerializeField] int StartIndex;
    int counter;

    void Start()
    {
        StartIndex = Mathf.Abs(StartIndex);
        counter = StartIndex % clipNames.Length;
    }

    public void PlayNext()
    {
        animator.Play(clipNames[counter]);
        counter = (counter + 1) % clipNames.Length;
    }

}
