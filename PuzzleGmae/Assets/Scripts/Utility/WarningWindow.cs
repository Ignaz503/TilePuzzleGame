using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningWindow : MonoBehaviour {

    public static WarningWindow Instance { get; protected set; }

    [SerializeField] Text header;
    [SerializeField] Text body;
    [SerializeField] Animator animator;

    struct Warning
    {
        public string Header;
        public string Body;
    }
    Queue<Warning> warningQueue;
    bool canDisplayNext = true;

    private void Awake()
    {

        if (Instance != null)
            throw new System.Exception("Only supporting one warning window right now. Already one in existance");
        Instance = this;

        warningQueue = new Queue<Warning>();

        StartCoroutine(WorkQueue());
    }


    public void SetNextPossible()
    {
        canDisplayNext = true;
    }

    public void GiveWarning(string header, string body)
    {
        warningQueue.Enqueue(new Warning() { Header = header, Body = body });
    }
    
    void DisplayWarning(Warning war)
    {
        this.header.text = war.Header;
        this.body.text = war.Body;
        canDisplayNext = false;
        animator.Play("Fade-in");
    }

    IEnumerator WorkQueue()
    {
        while (true)
        {
            if(warningQueue.Count > 0 && canDisplayNext)
            {
                DisplayWarning(warningQueue.Dequeue());
            }

            yield return null;
        }
    }
}
