using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneOnFocus : MonoBehaviour
{
    public string ColliderNameNext; // Name of the collider that triggers the scene change
    public string ColliderNamePrevious;
    public float maxDistance = 2f; // Adjust as needed
    public string nextSceneName;
    public string previousSceneName;

    void Update()
    {
        // Cast a ray from the camera's viewport
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Check if the hit collider has the target name
            if (hit.collider.name == ColliderNameNext)
            {
                // Load the next scene
                SceneManager.LoadScene(nextSceneName);
            }
        }

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // Check if the hit collider has the target name
            if (hit.collider.name == ColliderNamePrevious)
            {
                // Load the next scene
                SceneManager.LoadScene(previousSceneName);
            }
        }
    }
}
