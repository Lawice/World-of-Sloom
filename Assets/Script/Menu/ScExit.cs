using UnityEngine;
using UnityEngine.SceneManagement;

public class ScExit : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.TryGetComponent(out ScSloom sloomComponent) && sloomComponent.SloomState == SLOOMSTATE.Placed)
        {
            OnNextLevel();
        }
    }

     public void OnNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
