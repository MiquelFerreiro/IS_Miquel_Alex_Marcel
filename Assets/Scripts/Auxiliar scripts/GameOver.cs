using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;

    public static SoundController SOUND_CONTROLLER;
    private GameObject objective;

    public bool isGameOver = false;

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        SOUND_CONTROLLER = GameObject.Find("SoundController").GetComponent<SoundController>();
        objective = GameObject.Find("Objective"); // Asegúrate de tener el nombre correcto del GameObject de la nave espacial en tu escena
    }

    // Muestra el texto de "Game Over"
    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);

        EnemySpawner.DestroyAllEnemies();

        Destroy(objective);

        SOUND_CONTROLLER.MuteAllSounds();
        SOUND_CONTROLLER.PlayYouLose();

        isGameOver = true;

        


    }
}
