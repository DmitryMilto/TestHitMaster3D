using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private LoaderManagerScene loader;
    [SerializeField] private Text number;
    [SerializeField] private Image _image;

    [SerializeField] private GameObject levelMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject winMenu;
    private void Start()
    {
        ServiceLocator.GetService<EnemyManager>().OnStateChange += UpdateEnemy;
        GameController.Instance.OnStateChange += UpdateUI;
        loader.OnStateChange += UpdateLevel;
    }

    private void UpdateUI(GameController.GameState state)
    {
        levelMenu.SetActive(state == GameController.GameState.Play || state == GameController.GameState.Start);
        gameOverMenu.SetActive(state == GameController.GameState.GameOver);
        winMenu.SetActive(state == GameController.GameState.Win);
    }
    private void UpdateEnemy(int country, int dead)
    {
        _image.fillAmount = (float) dead / country;
    }

    private void UpdateLevel(int level)
    {
        number.text = level.ToString();
    }
    private void OnDestroy()
    {
        //ServiceLocator.GetService<EnemyManager>().OnStateChange -= UpdateEnemy;
        GameController.Instance.OnStateChange -= UpdateUI;
        loader.OnStateChange -= UpdateLevel;
    }
}
