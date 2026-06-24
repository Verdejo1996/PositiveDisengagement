using System;
using UnityEngine;

[Serializable]
public class QuestData
{
    [Header("Quest Info")]
    public string questTitle;

    [TextArea]
    public string questDescription;

    [Header("Objectives")]
    public QuestObjective[] objectives;
}