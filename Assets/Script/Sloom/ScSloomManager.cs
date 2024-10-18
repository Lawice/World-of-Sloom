using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScSloomManager : MonoBehaviour { 
    [Header("~~~~~Sloom Generator~~~~")]
    [SerializeField] GameObject _sloom;
    [SerializeField] Transform _sloomGenerate;
    [SerializeField] Transform _sloomFolder;
    
    [Header("~~~~~Sloom Generator Image~~~~")]
    [SerializeField] Image _sloomImage;
    [SerializeField] Sprite _defaultSprite;
    [SerializeField] Sprite _resetSprite;
    
    [Header("~~~~Count~~~~")]
    [SerializeField] int _nbActual;
    [SerializeField] int _nbMax;
    [SerializeField] TextMeshProUGUI _displayNb;

    
    private void Start() {
        _displayNb.text = $"{_nbActual} / {_nbMax}";
    }

    public void CreateSloom() {
        if (_nbActual < _nbMax) {
            Instantiate(_sloom, _sloomGenerate.position, Quaternion.identity, _sloomFolder);
            _nbActual++;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }

    void Update()
    {
        _displayNb.text = $"{_nbActual} / {_nbMax}";

        if (_nbActual < _nbMax)
        {
            _sloomImage.sprite = _defaultSprite;
        }
        else
        {
            _sloomImage.sprite = _resetSprite;
        }
    }
}
