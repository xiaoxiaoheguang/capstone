using UnityEngine;

/// <summary>
/// ���� NPC �Ļ��������߼���
/// ����ҽ���/�뿪 NPC ��Χʱ������ʾ�򽻻���
/// </summary>
public class NPCController : MonoBehaviour
{
    /// <summary>
    /// �洢 NPC �Ļ�����Ϣ���ݣ������֡�̨�ʵȣ���
    /// </summary>
    public NPCData data;

    /// <summary>
    /// NPC ����ײ�壨�������򣩣����ڼ������Ƿ񿿽���
    /// ͨ�� Inspector ָ����
    /// </summary>
    [SerializeField] private Collider npcCollider;

    /// <summary>
    /// �������Ƿ��� NPC �Ľ�����Χ�ڡ�
    /// </summary>
    private bool isPlayerInRange = false;

    /// <summary>
    /// ����������� NPC �Ĵ�����Χʱ���á�
    /// </summary>
    /// <param name="other">���봥��������ײ�塣</param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called on NPC: {data.npcName}");
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log($"Player entered range of NPC: {data.npcName}");

            // TODO: ��������� UI ��ʾ�߼�
             WorldDialogueUI.Instance.ShowHint(data.npcName);
        }
    }

    /// <summary>
    /// ���������뿪 NPC �Ĵ�����Χʱ���á�
    /// </summary>
    /// <param name="other">�뿪����������ײ�塣</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log($"Player exited range of NPC: {data.npcName}");

            // TODO: ��������� UI �����߼�
             WorldDialogueUI.Instance.HideHint();
        }
    }

    /// <summary>
    /// ÿ֡���ã�����������չ NPC �Ľ����߼���
    /// ��������Ұ��������Ի���
    /// </summary>
    private void Update()
    {
        // ʾ�����������Ƿ��ڷ�Χ�ڲ����½�����
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Start dialogue with NPC: {data.npcName}");
            // TODO: �����Ի�ϵͳ
        }
    }
}
