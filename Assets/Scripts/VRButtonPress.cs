using UnityEngine;
using UnityEngine.XR;
using System.Collections;
using System.Collections.Generic;

public class VRButtonPress : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels; // List of panels in the desired order
    private int currentPanelIndex = 0; // Track the index of the current visible panel
    private bool isTransitioning = false; // Prevent multiple transitions at once
    private float buttonPressCooldown = 0.5f;

    private List<InputDevice> rightHandDevices = new List<InputDevice>();

    void Start()
    {
        // Get the right hand controller devices
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);

        // Ensure the initial state is correct
        if (panels.Count > 0)
        {
            // Set the first panel active
            for (int i = 0; i < panels.Count; i++)
            {
                panels[i].SetActive(i == currentPanelIndex); // Only the first panel is active initially
            }
        }
    }

    void Update()
    {
        // Don't process input if we're already transitioning
        if (isTransitioning) return;

        // Check for trigger button press
        if (rightHandDevices.Count > 0)
        {
            InputDevice rightHandDevice = rightHandDevices[0];

            if (rightHandDevice.isValid)
            {
                if (rightHandDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isPressed) && isPressed)
                {
                    // Set transition flag to prevent multiple triggers
                    isTransitioning = true;

                    // Switch to the next panel
                    SwitchToNextPanel();

                    // Start a coroutine to reset the transition flag
                    StartCoroutine(ResetTransition());
                }
            }
        }
    }

    void SwitchToNextPanel()
    {
        if (panels.Count == 0) return;

        // Deactivate the current panel
        panels[currentPanelIndex].SetActive(false);

        // Increment the panel index
        currentPanelIndex = (currentPanelIndex + 1) % panels.Count; // Wrap around when the end is reached

        // Activate the next panel
        panels[currentPanelIndex].SetActive(true);

        Debug.Log("Switching to panel: " + panels[currentPanelIndex].name);
    }

    // Coroutine to reset the transition flag after a cooldown
    IEnumerator ResetTransition()
    {
        yield return new WaitForSeconds(buttonPressCooldown);
        isTransitioning = false;
    }
}
