using UnityEngine;

public class MarkerScript : MonoBehaviour
{
    public GameObject markerPrefab; // Reference to the marker prefab
    private GameObject markerInstance; // Reference to the instantiated marker

    void Update()
    {
        // Ensure there is a valid AR session
        if (Camera.main == null || !Camera.main.gameObject.activeInHierarchy)
            return;

        // Cast a ray from the center of the screen
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if the marker instance exists
        if (markerInstance == null)
        {
            // Instantiate the marker prefab
            markerInstance = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);
        }

        // Perform the raycast
        if (Physics.Raycast(ray, out hit))
        {
            // Update the marker's position and rotation to match the hit point
            markerInstance.transform.position = hit.point;
            markerInstance.transform.rotation = Quaternion.LookRotation(hit.normal, Vector3.up);
        }
        else
        {
            // If no hit point found, place the marker at a default distance
            markerInstance.transform.position = ray.origin + ray.direction * 2.0f; // Change 2.0f to desired default distance
            markerInstance.transform.rotation = Quaternion.LookRotation(ray.direction, Vector3.up);
        }
    }
}
