using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogues
{
    public string playername;

    [TextArea(3, 10)]
    public string[] sentences;
}