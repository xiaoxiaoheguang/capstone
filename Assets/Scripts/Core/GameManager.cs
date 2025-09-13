using UnityEngine;
using Core.Events;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        // 确保EventBus实例被创建
        _ = EventBus.Instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //设置帧率为60
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
