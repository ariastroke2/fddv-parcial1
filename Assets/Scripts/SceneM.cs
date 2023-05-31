using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneM : MonoBehaviour
{
    public void StartGame(){
        SceneManager.LoadScene(1);
    }

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
}
