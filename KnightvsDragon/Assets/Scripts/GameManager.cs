using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private bool startGame = false;
    private KnightController knightController;
    private DragonController dragonController;
    private UIManager uiManager;
    [SerializeField] private int coinsRequiredToWin;
    //Allows other classes to get these variables but not set it.
    //This is to prevent other classes from accidently changing the variable values
    public bool StartGame
    {
        get
        {
            return startGame;
        }
        private set
        {
            startGame = value;
        }
    }

    private bool pauseState = false;
    public bool PauseState
    {
        get
        {
            return pauseState;
        }
        private set
        {
            pauseState = value;
        }
    }


    private bool isGameOver = false;
    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }
        private set
        {
            isGameOver = value;
        }
    }

    [SerializeField] private bool hasWonGame = false;
    public bool HasWonGame
    {
        get
        {
            return hasWonGame;
        }
        private set
        {
            hasWonGame = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        knightController = FindObjectOfType<KnightController>();
        uiManager = FindObjectOfType<UIManager>();
        dragonController = FindObjectOfType<DragonController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(knightController.CoinsCollected >= coinsRequiredToWin)
        {
            hasWonGame = true;
        }

        if(knightController.Health <= 0)
        {
            hasWonGame = true;
        }

        if(hasWonGame)
        {
            uiManager.ToggleWinScreen();
        }

    }

    //This can be called by other classes to reload the game scene.
    public void ReloadGameScene()
    {
        TogglePauseState();
        SceneManager.LoadScene(2);

    }

    public void ReturnToMenuScene()
    {
        TogglePauseState();
        SceneManager.LoadScene(1);

    }

    public void TogglePauseState()
    {
        //If the game is paused and the player clicks the pause button, unpause the game.
        //Unpause any audio as well.
        //Then set pauseState to false.
        if (pauseState && !isGameOver && !hasWonGame)
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            pauseState = false;
        }
        //If the game is not paused and the player clicks the pause button, pause the game.
        //Pause any audio as well.
        //Then set pauseState to true.
        else if (!pauseState && !isGameOver && !hasWonGame)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            pauseState = true;
        }
    }
}
