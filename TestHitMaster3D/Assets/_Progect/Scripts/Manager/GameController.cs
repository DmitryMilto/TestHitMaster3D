using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public enum GameState
    {
        Start, Play, Load, GameOver, Win
    }
    public System.Action<GameState> OnStateChange;
    [SerializeField] private GameState _state;

    public GameState State
    {
        get => _state;
        set
        {
            _state = value;
            OnStateChange?.Invoke(_state);
        }
    }

    public void StartGame()
    {
        State = GameState.Play;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
