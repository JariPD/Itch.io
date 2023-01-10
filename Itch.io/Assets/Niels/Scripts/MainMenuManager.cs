using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;

    public GameObject[] Objects;
    public GameObject cam;

    public Animator Play;
    public Animator Quit;

    public Animator Menu;

    private void Awake()
    {
        instance = this;
    }

    public void BlackJackStart()
    {
        int index = 0;
        int lodedScene = index;
        index = 1;
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(lodedScene);

        RenderSettings.fogDensity = 0.02f;

        for (int i = 0; i < Objects.Length; i++)
        {
            Objects[i].SetActive(false);
        }

        StartCoroutine(Cam());
    }

    public void PlayMenu()
    {
        Play.GetComponent<MainMenu>().off = true;
        Quit.GetComponent<MainMenu>().off = true;
        Play.Play("PlayCardDissolve");
        Quit.Play("QuitCardDissolve");

        Menu.Play("MainMenu");
    }

    private IEnumerator Cam()
    {
        yield return new WaitForSeconds(2);
        cam.SetActive(false);
        Destroy(this.gameObject);
    }
}
