using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogues And Options", menuName = "Dialogue And Options")]
[Serializable]
public class DialoguesAndOptions: ScriptableObject
{
    [Serializable]
    public class DialogueSystem
    {
        [SerializeField]
        private List<Dialogues> _dialogues;
        [SerializeField]
        private DialogueOptions _dialogueOptions;
        [SerializeField]
        private DialogueTriggeringEntity _dialogueTriggeringEntity;

        public List<Dialogues> Dialogues { get => _dialogues; }
        public DialogueOptions DialogueOptions { get => _dialogueOptions;  }
        public DialogueTriggeringEntity DialogueTriggeringEntity { get => _dialogueTriggeringEntity; }
    }

    public List<DialogueSystem> exchange;
}