using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public string SceneName = "GameS";

    public static Title instance;

    private SaveNLoad theSaveNLoad;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void ClickStart()
    {
        Debug.Log("����");
        SceneManager.LoadScene(SceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("�ε�");

        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);

        while (!operation.isDone)
        {
            yield return null;
        }

        theSaveNLoad = FindObjectOfType<SaveNLoad>();
        theSaveNLoad.LoadData();
        Destroy(gameObject);
    }

    public void ClickExit()
    {
        Debug.Log("����");
        Application.Quit();
    }
}
