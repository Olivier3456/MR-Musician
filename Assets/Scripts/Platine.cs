using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Platine : MonoBehaviour
{
    private Vector3 _lastRotationEulerAngles;

    [SerializeField] private float _speedFactor = 1;

    [SerializeField] private TextMeshProUGUI _debugText;

    [SerializeField] private Transform _platine;

    [SerializeField] private AudioSource _platineAudioSource;

    private float _defaultPitch = 1;


    private float updateInterval = 0.1f;
    private float timeSinceLastUpdate = 0f;



    void Update()
    {        
        Quaternion currentRotation = Quaternion.AngleAxis(_platine.rotation.eulerAngles.y, Vector3.up);
        
        Quaternion previousRotation = Quaternion.AngleAxis(_lastRotationEulerAngles.y, Vector3.up);
        
        Quaternion relativeRotation = Quaternion.Inverse(previousRotation) * currentRotation;
        
        float angle = relativeRotation.eulerAngles.y;
        
        if (angle > 180f) angle -= 360f;
        
        float speed = angle / Time.deltaTime;
        
        _lastRotationEulerAngles = _platine.rotation.eulerAngles;


        float desiredPitch = Mathf.Clamp(_defaultPitch + (speed * _speedFactor), -3, 3);

        _platineAudioSource.pitch = desiredPitch;
        DisplayPitch();
    }

    private void DisplayPitch()
    {
        if (timeSinceLastUpdate >= updateInterval)
        {
            _debugText.text = "Pitch: " + _platineAudioSource.pitch.ToString("0.00");
            timeSinceLastUpdate = 0f;
        }
        else
        {
            timeSinceLastUpdate += Time.deltaTime;
        }
    }



    public void SetDefaultPitch(float pitch)
    {
        _defaultPitch = pitch + 0.5f;
    }

    public void TogglePlayPause()
    {
        if (_platineAudioSource.isPlaying) _platineAudioSource.Pause();
        else _platineAudioSource.Play();
    }

    public void SetStereoPan(float value)
    {
        _platineAudioSource.panStereo = value;
    }
}
