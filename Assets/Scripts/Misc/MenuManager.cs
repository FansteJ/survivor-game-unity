using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quitting the game...");

        Application.Quit();
    }
}