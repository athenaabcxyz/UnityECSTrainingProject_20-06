using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameReset : MonoBehaviour
{
    [SerializeField] GameObject EndUI;
    public void ResetTheScene()
    {
        EndUI.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
