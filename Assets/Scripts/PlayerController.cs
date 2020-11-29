﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{

    [Tooltip("in ms^-1")] [SerializeField] float speed = 20f;
    [Tooltip("in ms")] [SerializeField] float xRange = 5f;
    [Tooltip("in ms")] [SerializeField] float yRange = 3f;

    [SerializeField] float positionPitchFactor = -5f;
    [SerializeField] float controlPitchFactor = -30f;
    [SerializeField] float positionYawFactor = 5f;
    [SerializeField] float controlRollFactor = -20f;
   
    [Header("Henk's abilities")]
    [Tooltip("FOV when braking")] public float brakeFOV = 50f;
    [Tooltip("FOV when boosting")] public float boostFOV = 90f;
    [Header("Effects")]
    public GameObject boostTrail;
    public GameObject boostPop;

    float xThrow, yThrow;
    bool isControlEnabled = true;
    private bool _boosting;

    void Update()
    {
        // Boost
        if (Input.GetButton("Fire3"))
        {
            _boosting = true; ;
        }

        if (Input.GetButtonUp("Fire3"))
        {
            _boosting = false;
        }

        // Brake
        if (Input.GetButton("Fire1"))
        {
            Brake(true);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            Brake(false);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (isControlEnabled)
        {
            ProcessTranslation();
            ProcessRotation();
            Boost();
        }
    }

    void OnPlayerDeath() // Uit CollisionHandler. Zorgt ervoor dat de controls niet meer bewegen. 
    {
        isControlEnabled = false;
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;
        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * positionYawFactor;

        float roll = xThrow * controlRollFactor;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    public float Speed => speed;

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        float xOffset = xThrow * speed * Time.deltaTime;
        float yOffset = yThrow * speed * Time.deltaTime;

        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void Boost()
    {
        if (_boosting)
        {
            boostPop.SetActive(true);
            boostTrail.SetActive(true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, boostFOV, 1f * Time.deltaTime);
            transform.parent.GetComponent<BetterWaypointFollower>().routeSpeed = 75f;

        }
        else if(Camera.main.fieldOfView > 60f)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60f, 1f * Time.deltaTime);
            transform.parent.GetComponent<BetterWaypointFollower>().routeSpeed = 30f;
            boostPop.SetActive(false);
            boostTrail.SetActive(false);
        }
    }

    void Brake(bool braking)
    {
        if (braking)
        {
            //boostPop.SetActive(true);
            //boostTrail.SetActive(true);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, brakeFOV, 1f * Time.deltaTime);
            transform.parent.GetComponent<BetterWaypointFollower>().routeSpeed = 10f; 
        }
        else if (Camera.main.fieldOfView < 60f)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60f, 1f);
            transform.parent.GetComponent<BetterWaypointFollower>().routeSpeed = 30f;
            //boostPop.SetActive(false);
            //boostTrail.SetActive(false);
        }
    }
}