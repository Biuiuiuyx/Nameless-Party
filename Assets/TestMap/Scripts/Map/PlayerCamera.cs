using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour
{
    private const int NumPlayers = 2;
    private List<CinemachineVirtualCamera> playerCameras;
    private int currentCameraIndex;
    private Camera defaultCamera;
    private List<GameObject> players;

    void Start()
    {
        playerCameras = new List<CinemachineVirtualCamera>();
        currentCameraIndex = 0;
        defaultCamera = Camera.main;
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

        // Get the position and rotation of the main camera
        Vector3 cameraPosition = defaultCamera.transform.position;
        Quaternion cameraRotation = defaultCamera.transform.rotation;

        // Create a new virtual camera and set its position and rotation to match the main camera
        GameObject cameraObj = new GameObject("defaultcamera");
        CinemachineVirtualCamera virtualCamera = cameraObj.AddComponent<CinemachineVirtualCamera>();
        virtualCamera.transform.position = cameraPosition;
        virtualCamera.transform.rotation = cameraRotation;
        playerCameras.Add(virtualCamera);

        for (int i = 0; i < NumPlayers; i++)
        {
            GameObject cameraObj2 = new GameObject("playercamera" + i);
            CinemachineVirtualCamera virtualCamera2 = cameraObj2.AddComponent<CinemachineVirtualCamera>();
            virtualCamera2.m_Lens.FieldOfView = 50;
            virtualCamera2.AddCinemachineComponent<CinemachineTransposer>();
            virtualCamera2.Follow = players[i].transform;
            playerCameras.Add(virtualCamera2);
        }

        ActivateCamera(currentCameraIndex);
        currentCameraIndex += 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateCamera(currentCameraIndex);
            currentCameraIndex += 1;
            if (currentCameraIndex > NumPlayers)
            {
                currentCameraIndex = 0;
            }
        }
    }

    private void ActivateCamera(int index)
    {
        for (int i = 0; i < playerCameras.Count; i++)
        {
            if (i == index)
            {
                playerCameras[i].gameObject.SetActive(true);
            }
            else {
                playerCameras[i].gameObject.SetActive(false);
            }
        }
    }
}
