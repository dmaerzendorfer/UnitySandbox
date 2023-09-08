using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    private int _startHash;

    private void Awake()
    {
        _startHash = Animator.StringToHash("Start");
    }

    // Update is called once per frame
    void Update()
    {
        //demo code for testing purposes, can be removed
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
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
        yield return new WaitForSeconds(transition.GetCurrentAnimatorStateInfo(0).length);
        DOTween.KillAll();
        SceneManager.LoadScene(levelIndex);
    }
}