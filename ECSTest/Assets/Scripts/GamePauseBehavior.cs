using CortexDeveloper.ECSMessages.Service;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Entities;

public class GamePauseBehavior : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image pauseImage;
    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject playingUI;
    private int currentState;

    public void Start()
    {
        pauseUI.SetActive(false);
        currentState = 1;
        pauseImage.gameObject.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    private void PauseGame()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        if (currentState == 1)
        {
            playingUI.SetActive(false);
            pauseImage.gameObject.SetActive(true);
            pauseUI.SetActive(true);
            MessageBroadcaster
                .PrepareMessage()
                .AliveForOneFrame()
                .PostImmediate(entityManager,
                    new GameStateChangeCommand
                    {
                        currentState = 2
                    });
            currentState = 2;
        }
        else if (currentState == 2)
        {
            playingUI.SetActive(true);
            pauseImage.gameObject.SetActive(false);
            pauseUI.SetActive(false);
            MessageBroadcaster
                .PrepareMessage()
                .AliveForOneFrame()
                .PostImmediate(entityManager,
                    new GameStateChangeCommand
                    {
                        currentState = 1
                    });
            currentState = 1;
        }

    }
}
