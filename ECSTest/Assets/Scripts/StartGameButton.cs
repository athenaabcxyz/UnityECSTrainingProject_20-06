using CortexDeveloper.ECSMessages.Service;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Entities;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class StartGameButton : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] GameObject playingUI;
    [SerializeField] GameObject startUI;
    [SerializeField] GameObject endUI;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        background.gameObject.SetActive(true);
        startUI.SetActive(true);
        endUI.SetActive(false);
        playingUI.gameObject.SetActive(false);
        _button.onClick.AddListener(StartGame);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        startUI.SetActive(false);
        background.gameObject.SetActive(false);
        playingUI.SetActive(true);
        MessageBroadcaster
            .PrepareMessage()
            .AliveForOneFrame()
            .PostImmediate(entityManager,
                new GameStateChangeCommand
                {
                    currentState = 1
                });
    }
}
