using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNigtCycle : MonoBehaviour
{
    private Material _skybox;
    [SerializeField] private Material _cover;
    [SerializeField] private Light _sun;

    //Skybox colors
    [SerializeField] private Color _dayGradientTop;
    [SerializeField] private Color _dayGradientBottom;

    [SerializeField] private Color _nightGradientTop;
    [SerializeField] private Color _nightGradientBottom;

    //Ambient Light Colors
    [SerializeField] private Color _dayAmbLightTop;
    [SerializeField] private Color _dayAmbLightMiddle;
    [SerializeField] private Color _dayAmbLightBottom;
    [SerializeField] private Color _dayShadowColor;
    [SerializeField] private Color _lightColorDay;

    [SerializeField] private Color _nightAmbLightTop;
    [SerializeField] private Color _nightAmbLightMiddle;
    [SerializeField] private Color _nightAmbLightBottom;
    [SerializeField] private Color _nightShadowColor;
    [SerializeField] private Color _lightColorNight;

    [SerializeField] private float _sunIntensityDay;
    [SerializeField] private float _sunIntensityNight;

    //Times
    [SerializeField, Range(0, 24)] private float _timeOfDay;
    [SerializeField, Range(0, 24)] private float _sunRise;
    [SerializeField, Range(0, 24)] private float _sunSet;
    [SerializeField, Range(0, 24)] private float _sunRiseLength;
    [SerializeField, Range(0, 24)] private float _sunSetLength;

    [SerializeField] private float _dayLengthSeconds;
    private float _secondsSinceLastDay;

    // Start is called before the first frame update
    void Start()
    {
        _skybox = RenderSettings.skybox;
    }

    // Update is called once per frame
    void Update()
    {
        _secondsSinceLastDay += Time.deltaTime;

        if (_secondsSinceLastDay > _dayLengthSeconds)
        {
            _secondsSinceLastDay = 0;
        }

        _timeOfDay = (_secondsSinceLastDay/_dayLengthSeconds) * 24f;
        
        UpdateLighting();
    }

    private void UpdateLighting()
    {
        if (_timeOfDay > _sunRise && _timeOfDay < _sunRise + _sunRiseLength)
        {
            float percent = 1 - ((_timeOfDay - _sunRise) / _sunRiseLength);
            UpdateColors(percent);
        }

        else if (_timeOfDay > _sunSet && _timeOfDay < _sunSet + _sunSetLength)
        {
            float percent = (_timeOfDay - _sunSet) / _sunSetLength;
            Debug.Log(percent);
            UpdateColors(percent);
        }

        else if (_timeOfDay < _sunRise || _timeOfDay > _sunSet)
        {
            UpdateColors(1);
        }

        else
        {
            UpdateColors(0);
        }

        if (_sun != null)
        {
            float percent = _timeOfDay / 24f;


            MoveSun(percent);
            
        }
    }

    private void UpdateColors(float nightPercent)
    {

        //Adjust skybox colors
        _skybox.SetColor("_gradientTop", Color.Lerp(_dayGradientTop, _nightGradientTop, nightPercent));
        _skybox.SetColor("_gradientBottom", Color.Lerp(_dayGradientBottom, _nightGradientBottom, nightPercent));

        //Adjust cover colors
        _cover.SetColor("_gradientTop", Color.Lerp(_dayGradientTop, _nightGradientTop, nightPercent));
        _cover.SetColor("_gradientBottom", Color.Lerp(_dayGradientBottom, _nightGradientBottom, nightPercent));

        //Adjust ambient light
        RenderSettings.ambientSkyColor = Color.Lerp(_dayAmbLightTop, _nightAmbLightTop, nightPercent);
        RenderSettings.ambientEquatorColor = Color.Lerp(_dayAmbLightMiddle, _nightAmbLightMiddle, nightPercent);
        RenderSettings.ambientGroundColor = Color.Lerp(_dayAmbLightBottom, _nightAmbLightBottom, nightPercent);

        _sun.intensity = Mathf.Lerp(_sunIntensityDay, _sunIntensityNight, nightPercent);
    }

    private void MoveSun(float percent)
    {
        _sun.transform.localRotation = Quaternion.Euler(new Vector3((percent * 360f) - 90f, 170f, 0));
    }
}
