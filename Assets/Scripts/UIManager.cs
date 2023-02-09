using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Image _sceneChanger;

    private void Awake()
    {
        GameManager.onLevelChange += ChangeScene;
    }

    private void OnDisable()
    {
        GameManager.onLevelChange -= ChangeScene;
    }

    private void ChangeScene()
    {
        StartCoroutine(ChangeSceneRoutine());
    }

    private IEnumerator ChangeSceneRoutine()
    {
        _sceneChanger.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _sceneChanger.gameObject.SetActive(false);
    }
}
