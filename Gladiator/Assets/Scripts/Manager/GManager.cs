using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager Instance;

    public PlayerData PlayerData;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}