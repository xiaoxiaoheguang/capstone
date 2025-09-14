using UnityEngine;
using TMPro;  // 引入 TextMeshPro 命名空间，用于 UI 文本显示

/// <summary>
/// 控制世界空间中的 NPC 对话提示 UI，
/// 用于在玩家靠近 NPC 时显示“按 E 对话”的提示，并始终面向相机。
/// </summary>
public class WorldDialogueUI : MonoBehaviour
{
    /// <summary>
    /// 静态单例实例，方便其他脚本直接访问 WorldDialogueUI。
    /// </summary>
    public static WorldDialogueUI Instance;

    /// <summary>
    /// 对话框的预制体（Prefab），需要在 Inspector 中指定。
    /// </summary>
    public GameObject dialogueBoxPrefab;

    /// <summary>
    /// 当前在场景中生成并显示的对话框对象。
    /// </summary>
    private GameObject currentBox;

    /// <summary>
    /// 对话框中的文本组件，用于修改提示文字。
    /// </summary>
    private TextMeshProUGUI text;

    /// <summary>
    /// 脚本初始化时调用，将当前对象设置为单例。
    /// </summary>
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 显示对话提示。
    /// </summary>
    /// <param name="npcName">NPC 的名字，将显示在提示文字中。</param>
    public void ShowHint(string npcName)
    {
        // 如果当前还没有对话框，就生成一个
        if (currentBox == null)
        {
            currentBox = Instantiate(dialogueBoxPrefab, transform);
            text = currentBox.GetComponentInChildren<TextMeshProUGUI>();
        }

        // 设置提示内容，例如“按 E 与 小明 对话”
        text.text = $"按 E 与 {npcName} 对话";

        // 显示对话框
        currentBox.SetActive(true);
    }

    /// <summary>
    /// 隐藏当前对话提示。
    /// </summary>
    public void HideHint()
    {
        if (currentBox != null) currentBox.SetActive(false);
    }

    /// <summary>
    /// 在 LateUpdate 中让对话框始终面向主摄像机。
    /// </summary>
    private void LateUpdate()
    {
        if (currentBox != null)
        {
            // 对话框朝向相机
            currentBox.transform.LookAt(Camera.main.transform);

            // 因为 LookAt 默认是反面朝向，所以旋转 180 度调整方向
            currentBox.transform.Rotate(0, 180, 0);
        }
    }
}
