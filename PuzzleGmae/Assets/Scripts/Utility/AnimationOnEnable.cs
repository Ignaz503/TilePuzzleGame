using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationOnEnable : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string clipToPlay;
    [SerializeField] Button butt;

    private void OnEnable()
    {
        animator.Play(clipToPlay);
    }
}
