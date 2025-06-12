using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] ParticleSystem speedupParticleSystem;
    [SerializeField] float minFOV = 20f;
    [SerializeField] float maxFOV = 120f;
    [SerializeField] float zoomDuration = 1f;
    [SerializeField] float zoomSpeedModifier = 5f;
    CinemachineVirtualCamera cinemachineCamera;

    void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ChangeCameraFOV(float speedAmount)
    {
        StopAllCoroutines(); // Stop any ongoing FOV changes to prevent overlap
        StartCoroutine(ChangeFOVRoutine(speedAmount));

        if (speedAmount > 0)
        {
            speedupParticleSystem.Play(); // Play particle system when speed increases
        }
    }

    IEnumerator ChangeFOVRoutine(float speedAmount)
    {
        float startFOV = cinemachineCamera.m_Lens.FieldOfView;
        float targetFOV = Mathf.Clamp(startFOV + speedAmount * zoomSpeedModifier, minFOV, maxFOV);

        float elapsedTime = 0f;
        while (elapsedTime < zoomDuration)
        {
            float t = elapsedTime / zoomDuration;
            elapsedTime += Time.deltaTime;
            float newFOV = Mathf.Lerp(startFOV, targetFOV, t);
            cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsedTime / zoomDuration);

            yield return null; // Wait for the next frame
        }
        cinemachineCamera.m_Lens.FieldOfView = targetFOV;
    }
}
