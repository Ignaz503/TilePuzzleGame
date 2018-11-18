using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOnMouseOver : MonoBehaviour {

    [SerializeField] Animator animator;
    [SerializeField] string clipToPlay;

    public void OnMouseEnter()
    {
        animator.Play(clipToPlay);
    }
}
