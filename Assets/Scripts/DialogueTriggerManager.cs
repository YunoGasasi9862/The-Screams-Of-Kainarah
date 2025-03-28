using System.Collections;
using System.Threading;
using UnityEngine;

public class DialogueTriggerManager : MonoBehaviour
{
    private int DialogueCounter { get; set; } = 0;
    private SemaphoreSlim SemaphoreSlim { get; set;} =  new SemaphoreSlim(1);     // use this for dialogue, to make it not run fast/use ASYNC for each sentence

    [SerializeField]
    public DialogueTriggerEvent dialogueTriggerEvent;
    public DialogueTakingPlaceEvent dialogueTakingPlaceEvent;

    private void Start()
    {
        dialogueTriggerEvent.AddListener(TriggerCoroutine);
    }

    private IEnumerator TriggerDialogue(DialoguesAndOptions.DialogueSystem dialogueSystem)
    {
        dialogueTakingPlaceEvent.Invoke(true);

        foreach (Dialogues dialogue in dialogueSystem.Dialogues)
        {
        
            if (dialogueSystem.Dialogues.Count == DialogueCounter)
            {
                Debug.Log("Concluded");
                dialogueSystem.DialogueOptions.DialogueConcluded = true;

                DialogueCounter = 0;

                yield return null;
            }
            else
            {
                SceneSingleton.GetDialogueManager().PrepareDialoguesQueue(dialogue);

                SemaphoreSlim.WaitAsync();

                StartCoroutine(SceneSingleton.GetDialogueManager().StartDialogue(SemaphoreSlim));

                DialogueCounter++;

                Debug.Log(DialogueCounter);
            }
            

            yield return new WaitUntil(() => SemaphoreSlim.CurrentCount > 0);
        }

        dialogueTakingPlaceEvent.Invoke(false);

    }

    public void TriggerCoroutine(DialoguesAndOptions.DialogueSystem dialogueSystem)
    {
        if(SceneSingleton.IsDialogueTakingPlace == false && dialogueSystem.DialogueOptions.DialogueConcluded == false)
        {
            Debug.Log("INSIDE");
            Coroutine triggerDialogueCoroutine = StartCoroutine(TriggerDialogue(dialogueSystem));
        }
    }
}
