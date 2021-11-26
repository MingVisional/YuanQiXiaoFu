using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UiButton,PasueMenu;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void pasueEnter()
    {
        Time.timeScale = 0f;
        PasueMenu.SetActive(true);
        UiButton.SetActive(false);
    }
    public void pasueOut()
    {
        Time.timeScale = 1f;
        PasueMenu.SetActive(false);
        UiButton.SetActive(true);
    }
    public void toMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
