using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioManager audioManager;

    public GameObject tutorial;
    public GameObject main;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
    }


    public void PlayButton()
    {
        audioManager.Play("CubeRoll");
        SceneManager.LoadScene(1);
    }

    public void TutorialButton()
    {
        audioManager.Play("CubeRoll");

        tutorial.SetActive(true);
        main.SetActive(false);

    }

    public void QuitButton()
    {
        audioManager.Play("CubeRoll");
        Application.Quit();
    }

}
