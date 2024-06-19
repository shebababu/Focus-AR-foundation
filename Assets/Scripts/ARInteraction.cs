using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ARInteraction : MonoBehaviour
{
    public LayerMask interactableLayer;
    public Text instructionText;
    public GameObject panel;
    public GameObject image;
    public GameObject reticle;
    public float maxDistance = 5f;
    public float cooldownDuration = 1f;
    public float focusDurationThreshold = 3f;
    public float displayDuration = 10f;
    public float reticleDistance = 2f;

    private float lastInteractionTime;
    private GameObject lastFocusedObject;
    private bool isNewFocus;
    private float focusStartTime;
    private bool isDisplaying;
    private float lastDisplayStartTime;
    private bool interactionTriggered;
    private Dictionary<string, string> colliderTextMap;
    private Coroutine stopAudioCoroutine;
    private AudioSource currentlyPlayingAudio;
    

    void Start()
    {
        panel.SetActive(false);
        image.SetActive(false);
        reticle.SetActive(true);
        lastInteractionTime = -cooldownDuration;
        InitializeColliderTextMap();
    }

    void Update()
    {
        UpdateReticlePosition();

        if (Time.time - lastInteractionTime < cooldownDuration)
            return;

        if (isDisplaying && Time.time - lastDisplayStartTime >= displayDuration)
        {
            ResetFocus(lastFocusedObject);
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, interactableLayer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != lastFocusedObject)
            {
                ResetFocus(lastFocusedObject); 
                isNewFocus = true;
                if (currentlyPlayingAudio != null)
                {
                    currentlyPlayingAudio.Stop();
                    currentlyPlayingAudio = null;
                }
                if (stopAudioCoroutine != null)
                {
                    StopCoroutine(stopAudioCoroutine);
                }
                stopAudioCoroutine = StartCoroutine(StopAudioAfterDelay(hitObject, 2));  // Passing the new object
                lastFocusedObject = hitObject;
                focusStartTime = Time.time;
                interactionTriggered = false;
            }

            if (!interactionTriggered && Time.time - focusStartTime >= focusDurationThreshold)
            {
                HandleFocusInteraction(hitObject);
                lastInteractionTime = Time.time;
                isDisplaying = true;
                lastDisplayStartTime = Time.time;
                interactionTriggered = true;
            }
        }
        else if (isNewFocus || isDisplaying)
        {
            ResetFocus(lastFocusedObject);
        }
    }

    void UpdateReticlePosition()
    {
        if (reticle != null)
        {
            reticle.transform.position = Camera.main.transform.position + Camera.main.transform.forward * reticleDistance;
        }
    }

    IEnumerator StopAudioAfterDelay(GameObject focusedObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        AudioSource audioSource = focusedObject.GetComponent<AudioSource>();
        if (audioSource != null && audioSource.isPlaying && audioSource == currentlyPlayingAudio)
        {
            audioSource.Stop();
        }
    }

    void HandleFocusInteraction(GameObject focusedObject)
    {
        

        AudioSource audioSource = focusedObject.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
            currentlyPlayingAudio = audioSource;
        }

        if (colliderTextMap.TryGetValue(focusedObject.name, out string description))
            instructionText.text = $"Focused on: {focusedObject.name}\n{description}";
        else
            instructionText.text = $"Focused on: {focusedObject.name}";

        EnablePanel();
        EnableImage();
    }

    void ResetFocus(GameObject lastFocusedObject)
    {
        
        ClearInstructions();
        DisablePanel();
        DisableImage();
        isNewFocus = false;
        isDisplaying = false;
        interactionTriggered = false;
        lastFocusedObject = null;  // Reset focus when no object is hit
    }

    void ClearInstructions()
    {
        instructionText.text = string.Empty;
    }

    void EnablePanel()
    {
        panel.SetActive(true);
    }

    void DisablePanel()
    {
        panel.SetActive(false);
    }

    void EnableImage()
    {
        image.SetActive(true);
    }

    void DisableImage()
    {
        image.SetActive(false);
    }

    void InitializeColliderTextMap()
    {
        colliderTextMap = new Dictionary<string, string>();
        colliderTextMap.Add("Inner_Nozzle", "The inner nozzle guides and shapes the flow of exhaust gases exiting the combustion chamber in a jet engine.");
        colliderTextMap.Add("Outer_Nozzle", "The outer nozzle surrounds the inner components of the jet engine and helps to further direct and accelerate the exhaust gases for efficient propulsion.");
        colliderTextMap.Add("Turbine_Shaft", "The turbine shaft connects the turbine to the compressor, transmitting power generated by the combustion process to drive the engine's components.");
        colliderTextMap.Add("Core_Shell", "The core shell encases the engine's central components, such as the compressor, combustion chamber, and turbine, providing structural support and housing for these vital parts. ");
        colliderTextMap.Add("Fan_Rim", "The fan rim is the outermost structure of the engine's fan assembly, supporting the fan blades and aiding in the intake and compression of air for propulsion.");
        colliderTextMap.Add("Turbofan", "The turbofan is a type of jet engine that combines a traditional turbojet engine with a large fan at the front, which provides additional thrust by accelerating air around the engine core, resulting in enhanced fuel efficiency and performance.");
        colliderTextMap.Add("Hi_pressure_blades", "High-pressure blades are located in the core of a jet engine and are responsible for extracting energy from the high-pressure gas flow produced by the combustion process, driving the engine's compressor and turbine stages.");
        colliderTextMap.Add("Hi_pressure_blades2", "High-pressure blades are located in the core of a jet engine and are responsible for extracting energy from the high-pressure gas flow produced by the combustion process, driving the engine's compressor and turbine stages.");

        colliderTextMap.Add("Low_PC_Body", "The low-pressure compressor body houses the components responsible for compressing incoming air at lower pressure levels before it reaches the high-pressure compressor, contributing to the overall compression process within the jet engine.");
        colliderTextMap.Add("LP_blades", "The low-pressure blades are situated within the low-pressure compressor and are responsible for further compressing incoming air at lower pressure levels before it enters the combustion chamber, aiding in the overall compression process of the jet engine.");
        colliderTextMap.Add("Turbine_Blades", "Turbine blades are mounted on the turbine shaft and are responsible for extracting energy from the high-pressure and high-velocity gases produced during combustion, converting this energy into rotational motion to drive the engine's compressor and other components.");
        colliderTextMap.Add("Low_presure_ribs_and_shell", "The low-pressure ribs and shell provide structural support and enclosure for the low-pressure components of the jet engine, such as the low-pressure compressor and associated blades, contributing to the overall integrity and efficiency of the engine's operation.");
        colliderTextMap.Add("Support_Ribs", "Support ribs are structural components within the engine's casing or housing that reinforce and support various parts, such as compressor and turbine sections, ensuring structural integrity and proper alignment during operation.");
        colliderTextMap.Add("High_Pressure_compresor", "The high-pressure compressor is a crucial component of a jet engine's core, responsible for compressing incoming air before it enters the combustion chamber, thereby increasing its pressure and facilitating efficient combustion.");
        colliderTextMap.Add("arm", "The arm of an industrial robot typically consists of multiple joints and links, allowing it to move in a coordinated manner to perform various tasks with precision and flexibility.");

        colliderTextMap.Add("link arm", "The link arm in an industrial robot connects different joints together, providing structural support and facilitating movement in a coordinated manner to execute specific tasks efficiently.");
        colliderTextMap.Add("Base low", "The base_low of an industrial robot serves as the foundation, providing stability and support to the entire robotic system while allowing for precise positioning and movement during operation.");
        colliderTextMap.Add("Gripper", "The gripper of an industrial robot is responsible for grasping and manipulating objects, employing various mechanisms such as jaws, vacuum suction, or magnetic attraction to securely hold items during tasks.");
        colliderTextMap.Add("foundation_low", "Foundation low: The foundation low of an industrial robot typically refers to the lower base structure, providing stability and support to the robot, ensuring proper alignment and minimizing vibrations during operation to maintain accuracy and safety.");
    }
}
