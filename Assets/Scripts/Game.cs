using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private Platform platform;
    [SerializeField] private Ball ball;

    [SerializeField] private List<BoxObject> boxObjects; // 0- left border, 1- right border
    [SerializeField] private List<BoxObject> targets ;
    

    public static Game game;


    private bool isPaused;
    private bool isStartGame;
    private bool isGameOver;
    private bool isWin;
    private void Awake()
    {
        game = this;
        
        Reset();
        targets = new List<BoxObject>();
    }
    private void Update()
    {
        InputControl();
        if (isStartGame)
        {
            GameOver();
            Win();
        }
        
    }

    private void Win()
    {
        
        if (isWin)
        {
            Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void GameOver()
    {
        
            isGameOver = ball.transform.position.y <= platform.transform.position.y - 2f;
        
        if (isGameOver)
        {
               
            Restart();

        }
    }

    private void Restart()
    {
        Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Reset()
    {
        
        isStartGame = false;
        isPaused = false;
        isGameOver = false;
        isWin = false;
    }
    private void InputControl()
    {
        if (Input.GetKeyDown(KeyCode.Space)) isStartGame = true;
        
        if (Input.GetKeyDown(KeyCode.R)) Restart();

        if (Input.GetKeyDown(KeyCode.Q)) Quit();

        //if (Input.GetKeyDown(KeyCode.N)) isWin = true;


        if (isStartGame)
        {
            if (Input.GetKeyDown(KeyCode.P)) isPaused = !isPaused;
           

            if(!isPaused)
            {
                platform.MoveHorizontal(Input.GetAxis("Horizontal"), boxObjects[0].transform.localPosition.x, boxObjects[1].transform.localPosition.x);
                platform.Rotate(Input.GetAxis("Vertical"));
            }
        }
        
    }

    private void Quit()
    {
        Application.Quit();
    }

    private void FixedUpdate()
    {
        if (isStartGame && !isPaused)
        {
            ball.Move();
            CheckCollisions();
        }
    }

    private void CheckCollisions()
    {
        foreach (BoxObject item in boxObjects)
        {

            item.CheckCollision(ball);
        }
        foreach (BoxObject item in targets)
        {

            item.CheckCollision(ball);

        }
    }

    public void AddTarget(BoxObject _target)
    {
        targets.Add(_target);
    }
    public void RemoveTarget(BoxObject _target)
    {
        targets.Remove(_target);
        if(isStartGame && targets.Count == 0)
        {
            isWin = true;
        }
    }
}
