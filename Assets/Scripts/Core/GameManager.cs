using UnityEngine;
using Core.Events;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        // ȷ��EventBusʵ��������
        _ = EventBus.Instance;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //����֡��Ϊ60
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
