using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScMainMenu : MonoBehaviour {
    [SerializeField] private GameObject _creditPanel;
    [SerializeField] private GameObject _mainMenu;

    private void Awake() {
        _creditPanel.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void OnStartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnOpenCredits() {
        _creditPanel.SetActive(true);
        _mainMenu.SetActive(false);
    }

    public void OnCloseCredits() {
        _creditPanel.SetActive(false);
        _mainMenu.SetActive(true);
    }
}