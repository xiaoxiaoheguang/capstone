using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; }
    
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

    public void StartMiniGame(string miniGameName)
    {
        Debug.Log($"Starting mini-game: {miniGameName}");
        // Implement mini-game starting logic here

    }
}
