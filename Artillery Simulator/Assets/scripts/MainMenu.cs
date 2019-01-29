using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    // Use this for initialization
    public AudioSource music;
    void Start () {
        if(!music.isPlaying)music.Play();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void play()
    {
        //Application.LoadLevel("Test");
        music.Stop();
        SceneManager.LoadScene("Test");
        
    }
    public void exit()
    {
        Application.Quit();
    }
    public void credits()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Additive);
       // music.Stop();
    }
    public void controls()
    {
        SceneManager.LoadScene("controls", LoadSceneMode.Additive);
        // music.Stop();
    }
    public void mainMenu()
    {
        //Application.LoadLevel("MainMenu");
        SceneManager.LoadScene("MainMenu");
    }
}
