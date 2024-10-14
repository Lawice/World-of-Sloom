using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScSloomManager : MonoBehaviour {
    [Header("~~~~~Sloom Generator~~~~")]
    [SerializeField] GameObject _sloom;
    [SerializeField] Transform _sloomGenerate;
    [SerializeField] Transform _sloomFolder;

    [Header("~~~~Count~~~~")]
    [SerializeField] int _nbActual;
    [SerializeField] int _nbMax;
    [SerializeField] TextMeshProUGUI _displayNb;

    ScDragDrop _dragDropSys => ScDragDrop.Instance;

    private void Start() {
        _displayNb.text = $"{_nbActual} / {_nbMax}";
    }

    public void CreateSloom() {
        if (_nbActual < _nbMax) {
            Instantiate(_sloom, _sloomGenerate.position, Quaternion.identity, _sloomFolder);
            _nbActual++;
            _displayNb.text = $"{_nbActual} / {_nbMax}";
        }
    }
}
