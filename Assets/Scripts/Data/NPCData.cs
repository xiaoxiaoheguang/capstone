using UnityEngine;

[CreateAssetMenu(fileName = "NPCData", menuName = "ScriptableObjects/NPCData", order = 1)]
public class NPCData :ScriptableObject
{

    public string npcName;
    public int npcID;

    [TextArea]public string[] dialogues;
    public string miniGameName;

}
