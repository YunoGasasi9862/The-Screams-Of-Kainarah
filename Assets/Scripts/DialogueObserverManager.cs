using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static DialogueEntityScriptableObject;

public class DialogueObserverManager : MonoBehaviour, IObserver<DialogueEntity>
{
    [Header("Dialogue Trigger Entity Object")]
    [SerializeField] DialogueEntityScriptableObject dialogueEntityScriptableObject;

    Dictionary<string, Func<object, Task>> dialogueManagerActionDict; //Func -> the type of parameter it will take that is Dialogues,and the return type will be Task. It's c# delegation

    private void Start()
    {
        dialogueManagerActionDict = PrefilDictionaryFromTheScriptableObject(dialogueEntityScriptableObject);
    }

    private Dictionary<string, Func<object, Task>> PrefilDictionaryFromTheScriptableObject(DialogueEntityScriptableObject dialogueEntityScriptableObject)
    {
        var dictionary = new Dictionary<string, Func<object, Task>>();

        foreach (var item in dialogueEntityScriptableObject.entities)
        {
            dictionary.Add(item.entity.tag, dialogues => TriggerDialogue(dialogues));
        }
        return dictionary;
    }
    private async Task TriggerDialogue(object dialogues)
    {
        await Task.Delay(TimeSpan.FromSeconds(0.5));

        if(DialogueType(dialogues) == typeof(Dialogues))
            StartCoroutine(Interactable.TriggerDialogue((Dialogues)dialogues));
        else
            StartCoroutine(Interactable.TriggerDialogue((Dialogues[])dialogues));

    }
    private void OnEnable()
    {
        PlayerObserverListenerHelper.DialogueEntites.AddObserver(this);
    }

    private void OnDisable()
    {
        PlayerObserverListenerHelper.DialogueEntites.RemoveOberver(this); 
    }

    private Type DialogueType(object dialogues)
    {
        return dialogues.GetType();
    }
    public void OnNotify(ref DialogueEntity Data, params object[] optional)
    {
        if(dialogueManagerActionDict.TryGetValue(Data.entity.tag, out var func))
        {
            if (Data.shouldDialogueTrigger)
            {
                object dialogues = Interactable.GetDialoguesDict[Data.entity.tag] is Dialogues ? (Dialogues)(Interactable.GetDialoguesDict[Data.entity.tag]) :
                    (Dialogues[])(Interactable.GetDialoguesDict[Data.entity.tag]);

                func.Invoke(dialogues);  //solved it!! casting is needed to cast it to dialogues as in the dictionary its object
            }
        }
    }

}