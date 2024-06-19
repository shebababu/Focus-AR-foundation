using UnityEngine;

public class marker_script : MonoBehaviour
{
    public GameObject markerPrefab; // Reference to the marker prefab
    private GameObject markerInstance; // Reference to the instantiated marker

    void Update()
    {
        // Check if the marker instance exists
        if (markerInstance == null)
        {
            // Instantiate the marker prefab
            markerInstance = Instantiate(markerPrefab, Vector3.zero, Quaternion.identity);
        }

        // Update the marker's position and rotation to match the AR camera
        if (Camera.main != null)
        {
            markerInstance.transform.position = Camera.main.transform.position;
            markerInstance.transform.rotation = Camera.main.transform.rotation;
        }
    }
}
