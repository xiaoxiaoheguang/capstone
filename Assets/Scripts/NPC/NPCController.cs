using UnityEngine;

/// <summary>
/// 控制 NPC 的基本交互逻辑，
/// 当玩家进入/离开 NPC 范围时触发提示或交互。
/// </summary>
public class NPCController : MonoBehaviour
{
    /// <summary>
    /// 存储 NPC 的基本信息数据（如名字、台词等）。
    /// </summary>
    public NPCData data;

    /// <summary>
    /// NPC 的碰撞体（触发区域），用于检测玩家是否靠近。
    /// 通过 Inspector 指定。
    /// </summary>
    [SerializeField] private Collider npcCollider;

    /// <summary>
    /// 标记玩家是否在 NPC 的交互范围内。
    /// </summary>
    private bool isPlayerInRange = false;

    /// <summary>
    /// 当有物体进入 NPC 的触发范围时调用。
    /// </summary>
    /// <param name="other">进入触发器的碰撞体。</param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter called on NPC: {data.npcName}");
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log($"Player entered range of NPC: {data.npcName}");

            // TODO: 在这里调用 UI 提示逻辑
             WorldDialogueUI.Instance.ShowHint(data.npcName);
        }
    }

    /// <summary>
    /// 当有物体离开 NPC 的触发范围时调用。
    /// </summary>
    /// <param name="other">离开触发器的碰撞体。</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log($"Player exited range of NPC: {data.npcName}");

            // TODO: 在这里调用 UI 隐藏逻辑
             WorldDialogueUI.Instance.HideHint();
        }
    }

    /// <summary>
    /// 每帧调用，可在这里扩展 NPC 的交互逻辑。
    /// 例如检测玩家按键触发对话。
    /// </summary>
    private void Update()
    {
        // 示例：检测玩家是否在范围内并按下交互键
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log($"Start dialogue with NPC: {data.npcName}");
            // TODO: 启动对话系统
        }
    }
}
