using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathSceneControl : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            SceneManager.LoadScene("Battle Arena");
        else if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Credits");
    }
}
