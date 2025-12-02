using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField]
    private Camera m_playerCamera;

    private float m_XRotation = 0f;
    [SerializeField]
    private  float m_XSensitivity = 100f;
    [SerializeField]
    private float m_YSensitivity = 100f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x * Time.deltaTime * m_XSensitivity;
        float mouseY = input.y * Time.deltaTime * m_YSensitivity;
        //calculate the rotation for looking up or down
        m_XRotation -= mouseY;
        m_XRotation = Mathf.Clamp(m_XRotation, -80f, 80f);
        //apply it to the camera transform for te up or down look
        m_playerCamera.transform.localRotation = Quaternion.Euler(m_XRotation, 0f, 0f);
        //move the player body left to right because the camera is a child of the player object
        transform.Rotate(Vector3.up * mouseX);
    }
}
