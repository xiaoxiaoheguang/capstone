using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartDialogue(NPCData npcData)
    {
        Debug.Log($"Starting dialogue with NPC: {npcData.npcName}");
        // Implement dialogue UI logic here
        foreach (var line in npcData.dialogues)
        {
            Debug.Log(line);
        }

        //MiniGameManager.Instance.StartMiniGame(npcData.miniGameName);
    }
}
