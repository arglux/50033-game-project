using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    public Text dialogueText;

    public string[] dialogueData;

    int currentDialogueNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentDialogueNum = 0;
        UpdateDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            PushNextButton();
        }
    }

    public void PushNextButton()
    {
        dialogueText.text = "";
        currentDialogueNum++;
        StartCoroutine(WaitAndViewDialogue());
    }

    IEnumerator WaitAndViewDialogue()
    {
        dialogueText.text = "";
        yield return new WaitForSeconds(0.3f);
        UpdateDialogue();
    }

    void UpdateDialogue()
    {
        dialogueText.text = dialogueData[currentDialogueNum];
    }
}
