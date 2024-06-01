using UnityEngine;
using UnityEngine.InputSystem;

public class SceneManager : MonoBehaviour
{
    private void Start()
    {
        InputManager.actions.Player.NextScene.performed += SwitchScene;
    }

    private void SwitchScene(InputAction.CallbackContext context)
    {
        // FIXME (Timon): Strings, DRY
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SampleScene")
            UnityEngine.SceneManagement.SceneManager.LoadScene("Scene2");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
