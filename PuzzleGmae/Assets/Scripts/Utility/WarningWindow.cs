using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningWindow : MonoBehaviour {

    public static WarningWindow Instance { get; protected set; }

    [SerializeField] Text header;
    [SerializeField] Text body;
    [SerializeField] Animator animator;

    private void Awake()
    {
        if (Instance != null)
            throw new System.Exception("Only supporting one warning window right now. Already one in existance");
        Instance = this;

        animator.Play("Fade-out");
    }

    public void GiveWarning(string header, string body)
    {
        this.header.text = header;
        this.body.text = body;
        animator.Play("Fade-in");
    }
}
