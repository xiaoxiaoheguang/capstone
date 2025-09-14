using UnityEngine;
using TMPro;  // ���� TextMeshPro �����ռ䣬���� UI �ı���ʾ

/// <summary>
/// ��������ռ��е� NPC �Ի���ʾ UI��
/// ��������ҿ��� NPC ʱ��ʾ���� E �Ի�������ʾ����ʼ�����������
/// </summary>
public class WorldDialogueUI : MonoBehaviour
{
    /// <summary>
    /// ��̬����ʵ�������������ű�ֱ�ӷ��� WorldDialogueUI��
    /// </summary>
    public static WorldDialogueUI Instance;

    /// <summary>
    /// �Ի����Ԥ���壨Prefab������Ҫ�� Inspector ��ָ����
    /// </summary>
    public GameObject dialogueBoxPrefab;

    /// <summary>
    /// ��ǰ�ڳ��������ɲ���ʾ�ĶԻ������
    /// </summary>
    private GameObject currentBox;

    /// <summary>
    /// �Ի����е��ı�����������޸���ʾ���֡�
    /// </summary>
    private TextMeshProUGUI text;

    /// <summary>
    /// �ű���ʼ��ʱ���ã�����ǰ��������Ϊ������
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ��ʾ�Ի���ʾ��
    /// </summary>
    /// <param name="npcName">NPC �����֣�����ʾ����ʾ�����С�</param>
    public void ShowHint(string npcName)
    {
        // �����ǰ��û�жԻ��򣬾�����һ��
        if (currentBox == null)
        {
            currentBox = Instantiate(dialogueBoxPrefab, transform);
            text = currentBox.GetComponentInChildren<TextMeshProUGUI>();
        }

        // ������ʾ���ݣ����硰�� E �� С�� �Ի���
        text.text = $"�� E �� {npcName} �Ի�";

        // ��ʾ�Ի���
        currentBox.SetActive(true);
    }

    /// <summary>
    /// ���ص�ǰ�Ի���ʾ��
    /// </summary>
    public void HideHint()
    {
        if (currentBox != null) currentBox.SetActive(false);
    }

    /// <summary>
    /// �� LateUpdate ���öԻ���ʼ���������������
    /// </summary>
    private void LateUpdate()
    {
        if (currentBox != null)
        {
            // �Ի��������
            currentBox.transform.LookAt(Camera.main.transform);

            // ��Ϊ LookAt Ĭ���Ƿ��泯��������ת 180 �ȵ�������
            currentBox.transform.Rotate(0, 180, 0);
        }
    }
}
