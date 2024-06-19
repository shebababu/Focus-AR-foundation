using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public string ColliderNameNext; // Name of the collider that triggers the scene change
    public string ColliderNamePrevious;
    public float maxDistance = 2f; // Adjust as needed
    public string nextSceneName;
    public string previousSceneName;

    public float thresholdDistance = 5f;
    public Text instructionText;
    public GameObject panel;
    public GameObject image;

    public float displayDuration = 5f; // Duration to display the panel and text (in seconds)

    private Vector3 initialCameraPosition;
    private bool displayed = false;
    private string text_one= "In this scene you can see two 3D models of jet engine and one 3d model of industrial robot, Go closer to the 3d model to get more information.If you want to go to previous scene, walk towards left from the left Jet engine. If you want to go to next scene, walk towards right from the right 3d objects.";
    private string text_two = "In this scene you can see an animation playing where is shows each part of jet engine seperately. You can focus on each component to view its description.";


    void Start()
    {
        initialCameraPosition = Camera.main.transform.position;
        // Set initial instruction text when the scene is loaded
        SetInstructionText("In this scene, you can see two 3D models of the jet engine and one 3D model of an industrial robot. Go closer to the 3D model to get more information. If you want to go to the previous scene, walk towards the left from the Jet engine. If you want to go to the next scene, walk towards the right from the right 3D objects.");

        // Enable the panel and text
        SetPanelAndTextActive(true);

        // Start coroutine to disable the panel and text after displayDuration
        StartCoroutine(DisablePanelAndText());
    }

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

    

    // Function to set instruction text
    void SetInstructionText(string text)
    {
        if (instructionText != null)
        {
            instructionText.text = text;
        }
    }

    // Function to enable or disable the panel and text
    void SetPanelAndTextActive(bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }

        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(active);
        }

        if (image != null)
        {
            image.gameObject.SetActive(active);
        }
    }

    // Coroutine to disable the panel and text after displayDuration
    IEnumerator DisablePanelAndText()
    {
        yield return new WaitForSeconds(displayDuration);
        SetPanelAndTextActive(false);
    }
}
