using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponADS : MonoBehaviour
{ 
    [Header("Positions")]
    [SerializeField]
    private Transform hipPosition;
    [SerializeField]
    private Transform adsPosition;

    [Header("Settings")]
    public float adsSpeed = 10f;

    private bool isActive = false;
    private bool isAiming = false;

    public void SetIsActive(bool active)
    {
        isActive = active;
    }
    public void SetAiming(bool aiming)
    {

        isAiming = aiming;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;

        Transform target = isAiming ? adsPosition : hipPosition;

        // Smooth movement
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            target.localPosition,
            adsSpeed * Time.deltaTime
        );

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            target.localRotation,
            adsSpeed * Time.deltaTime
        );
    }
}
