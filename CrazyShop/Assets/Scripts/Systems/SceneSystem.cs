using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public void SceneLoader()
    {
        SceneManager.LoadScene("Main");
    }
}
