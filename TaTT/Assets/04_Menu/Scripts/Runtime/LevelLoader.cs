using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    private static LevelLoader _instance = null;

    public static LevelLoader Instance
    {
        get { return _instance; }
    }

    private int _startHash;

    private void Awake()
    {
        _startHash = Animator.StringToHash("Start");
        _instance = this;
    }

    public void LoadLevelIndex(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        if (levelIndex >= SceneManager.sceneCountInBuildSettings)
            levelIndex = 0;

        transition.SetTrigger(_startHash);
        //use realtime wait to wait even if timescale is 0
        yield return new WaitForSecondsRealtime(transition.GetCurrentAnimatorStateInfo(0).length);
        DOTween.KillAll();
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}