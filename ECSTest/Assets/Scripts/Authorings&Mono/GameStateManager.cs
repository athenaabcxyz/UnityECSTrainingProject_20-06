using CortexDeveloper.ECSMessages.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{

    public Image background;
    public Image backgroundPause;
    public GameObject playingUI;
    public GameObject startUI;
    public GameObject endUI;
    public GameObject pauseUI;
    public Button startButton;

    public GameBaseState currentState;
    public GameStartState startState = new GameStartState();
    public GamePlayingState playingState = new GamePlayingState();
    public GameEndState endState = new GameEndState();
    public GamePauseState pauseState = new GamePauseState();


    private void OnEnable()
    {
        startButton.onClick.AddListener(StartOnClickListener);
    }
    private void OnDisable()
    {
        startButton.onClick.RemoveListener(StartOnClickListener);
    }
    // Start is called before the first frame update
    void Start()
    {
        currentState = startState;
        currentState.Enter(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update(this);
    }
    public void SwitchState(GameBaseState state)
    {
        currentState.Exit(this);
        currentState = state;
        currentState.Enter(this);
    }

    public void StartOnClickListener()
    {
        SwitchState(playingState);
    }
}
