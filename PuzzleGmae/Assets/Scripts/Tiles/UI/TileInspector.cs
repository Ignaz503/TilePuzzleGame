using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class TileInspector : MonoBehaviour
{
    public event Action OnFadeIn; 
    [SerializeField] protected TilePlacer tilePlacer;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Text positionText;
    Tile tileInspecting;

    public virtual void EnableInspector(Tile t)
    {
        tileInspecting = t;
        positionText.text = $"Position: ({t.GridPosition.x},{t.GridPosition.y})";
        animator.Play("Fade-in");
        OnFadeIn?.Invoke();
    }

    public void Remove()
    {
        tilePlacer.Remove(tileInspecting);
        animator.Play("Fade-out");
    }
}