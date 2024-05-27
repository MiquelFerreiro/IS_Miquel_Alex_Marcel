using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;

    public static SoundController SOUND_CONTROLLER;
    private SpaceshipAnimation spaceship;

    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        SOUND_CONTROLLER = GameObject.Find("SoundController").GetComponent<SoundController>();
        spaceship = GameObject.Find("Objective").GetComponent<SpaceshipAnimation>(); // Asegúrate de tener el nombre correcto del GameObject de la nave espacial en tu escena
    }

    // Muestra el texto de "Game Over"
    public void ShowGameOver()
    {
        gameOverText.gameObject.SetActive(true);
        EnemySpawner.DestroyAllEnemies();

        SOUND_CONTROLLER.MuteAllSounds();
        SOUND_CONTROLLER.PlayYouLose();
        spaceship.FinishRotation();
    }
}
