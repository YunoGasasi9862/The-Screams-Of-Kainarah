using System.Collections;
using System.Threading;
using UnityEngine;

public class DialogueTriggerManager : MonoBehaviour, IObserver<GameState>
{
    private int DialogueCounter { get; set; } = 0;
    private GameState GameState { get; set; }
    private SemaphoreSlim SemaphoreSlim { get; set;} =  new SemaphoreSlim(1);

    [SerializeField]
    public DialogueTriggerEvent dialogueTriggerEvent;
    [SerializeField]
    public GameStateEvent gameStateEvent;
    [SerializeField]
    GlobalGameStateDelegator globalGameStateDelegator;

    private void Start()
    {
        dialogueTriggerEvent.AddListener(TriggerCoroutine);

        globalGameStateDelegator.NotifySubjectWrapper(this, new NotificationContext()
        {
            ObserverName = this.name,
            ObserverTag = this.name,
            SubjectType = typeof(GlobalGameStateManager).ToString()

        }, CancellationToken.None);
    }

    private IEnumerator TriggerDialogue(DialoguesAndOptions.DialogueSystem dialogueSystem)
    {
        BroadcastGameState(GameState.DIALOGUE_TAKING_PLACE);

        foreach (DialogueSetup dialogue in dialogueSystem.DialogueSetup)
        {
            SceneSingleton.GetDialogueManager().PrepareDialoguesQueue(dialogue);

            SemaphoreSlim.Wait();

            StartCoroutine(SceneSingleton.GetDialogueManager().StartDialogue(SemaphoreSlim));

            DialogueCounter++;

            yield return new WaitUntil(() => SemaphoreSlim.CurrentCount > 0);

            if (dialogueSystem.DialogueSetup.Count == DialogueCounter)
            {
                dialogueSystem.DialogueSettings.DialogueConcluded = true;

                DialogueCounter = 0;

                GameState = GameState.FREE_MOVEMENT;

                BroadcastGameState(GameState.FREE_MOVEMENT);

                yield return null;
            }
        }
    }

    public void TriggerCoroutine(DialoguesAndOptions.DialogueSystem dialogueSystem)
    {
        if (GameState == GameState.DIALOGUE_TAKING_PLACE || dialogueSystem.DialogueSettings.DialogueConcluded)
        {
            return;
        }

        Coroutine triggerDialogueCoroutine = StartCoroutine(TriggerDialogue(dialogueSystem));
    }

    private async void BroadcastGameState(GameState value)
    {
        GameState = value;

        await gameStateEvent.Invoke(value);
    }

    public void OnNotify(GameState data, NotificationContext notificationContext, SemaphoreSlim semaphoreSlim, CancellationToken cancellationToken, params object[] optional)
    {
        GameState = data;
    }
}
