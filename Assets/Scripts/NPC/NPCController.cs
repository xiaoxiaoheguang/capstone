using UnityEngine;

public class NPCController : MonoBehaviour
{
    public NPCData data;

    [SerializeField] private Collider npcCollider;
    private bool isPlayerInRange = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log($"Player entered range of NPC: {data.npcName}");
            // Additional interaction logic can be added here
            // WorldDialogueUI.Instance.ShowHint(npcData.npcName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log($"Player exited range of NPC: {data.npcName}");
            // Additional interaction logic can be added here
            // WorldDialogueUI.Instance.HideHint();
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
