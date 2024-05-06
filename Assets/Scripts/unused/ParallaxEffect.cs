using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform[] backgrounds; // Array of the background layers
    public float[] parallaxScales; // The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;    // How smooth the parallax effect will be

    private Transform cam;          // Reference to the main camera's transform
    private Vector3 previousCamPos; // The position of the camera in the previous frame

    void Awake()
    {
        cam = Camera.main.transform; // Assign reference to the main camera
    }

    void Start()
    {
        previousCamPos = cam.position; // Set the previous frame's camera position
    }

    void Update()
    {
        // For each background layer
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // The parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallaxX = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            float parallaxY = (previousCamPos.y - cam.position.y) * parallaxScales[i];

            // Set a target position which is the background's current position plus the parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallaxX;
            float backgroundTargetPosY = backgrounds[i].position.y + parallaxY;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

            // Fade between current position and target position using Lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        // Set the previousCamPos to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}
