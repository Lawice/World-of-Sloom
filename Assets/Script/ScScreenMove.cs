using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScScreenMove : MonoBehaviour {
    [SerializeField] Camera _mainCamera;
    [SerializeField] float _maxHorizontal;
    [SerializeField] Slider _moveSlider;
    [SerializeField] float _defaultPos;

    void Start() {
        _moveSlider.minValue = -1;
        _moveSlider.maxValue = 1;
        _moveSlider.value = _defaultPos;

        float newX = _moveSlider.value * _maxHorizontal;
        _mainCamera.transform.position = new Vector3(newX, 0f, _mainCamera.transform.position.z);
    }

    public void OnSliderValueChanged(float value) {
        float newX = value * _maxHorizontal;
        _mainCamera.transform.position = new Vector3(newX, 0f, _mainCamera.transform.position.z);
    }
}
