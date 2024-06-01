using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        StartCoroutine(RestartGameWithDelay(10f));
    }

    IEnumerator RestartGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        RestartGame();
    }
    public void RestartGame()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);
    }
}
