using UnityEngine;

public class GManager : MonoBehaviour
{
    [SerializeField] public PlayerData PlayerData;
    public static GManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
