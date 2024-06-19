using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_manager : MonoBehaviour
{
    public float delayTime = 5f;
    public string nextSceneName;

    void Start()
    {
        // Invoke the LoadNextScene method after the specified delay time
        Invoke("LoadNextScene", delayTime);
    }

    void LoadNextScene()
    {
        Debug.Log("Loading next scene: " + nextSceneName);
        // Load the next scene by name
        SceneManager.LoadScene(nextSceneName);
        
    }
}
