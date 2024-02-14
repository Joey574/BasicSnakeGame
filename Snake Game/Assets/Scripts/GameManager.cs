using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Objects")]
    public Snake snake;
    private GameObject food;
    private Vector2 foodPos;

    [Header("Prefabs")]
    public GameObject foodPrefab;
    public GameObject wallPrefab;
    public GameObject snakeHeadPrefab;

    [Header("Game Data")]
    public Vector2 gameboardSize;
    public float frequency;

    void Awake()
    {
        InitializeGameBoard();
        InitializeSnake();
        SpawnFood();
    }

    void Update()
    {
        CheckForCollisions();

        if (snake == null)
        {
            InitializeSnake();
        }
    }

    private void InitializeSnake()
    {
        GameObject SnakeHead = Instantiate(snakeHeadPrefab, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), GameObject.Find("Snake").transform);
        snake = SnakeHead.GetComponent<Snake>();
    }

    private void SpawnFood()
    {
        int x;
        int y;

        do
        {
            x = (int)Random.Range(-gameboardSize.x, gameboardSize.x);
            y = (int)Random.Range(-gameboardSize.y, gameboardSize.y);

        } while (snake.snakePos.Contains(new Vector2(x,y)));

        foodPos.x = x;
        foodPos.y = y;

        food = Instantiate(foodPrefab, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0));
    }

    private void CheckForCollisions()
    {
        if (snake.snakePos[0].Equals(foodPos))
        {
            Destroy(food);
            snake.increaseLength = true;
            SpawnFood();
        }

        if (snake.snakePos[0].x > gameboardSize.x || snake.snakePos[0].x < -gameboardSize.x ||
            snake.snakePos[0].y > gameboardSize.y || snake.snakePos[0].y < -gameboardSize.y)
        {
            Destroy(snake);
        }

        HashSet<Vector2> setPos = new HashSet<Vector2>();
        setPos.UnionWith(snake.snakePos);

        if (setPos.Count != snake.snakePos.Count)
        {
            Destroy(snake);
        }
    }

    private void InitializeGameBoard()
    {
        int y = (int) -gameboardSize.y - 1;
        for (int i = (int) -gameboardSize.x - 1; i <= gameboardSize.x + 1; i++)
        {
            Instantiate(wallPrefab, new Vector3(i, y, 0), new Quaternion(0, 0, 0, 0), GameObject.Find("Wall").transform);
        }

        y = (int) gameboardSize.y + 1;
        for (int i = (int) -gameboardSize.x - 1; i <= gameboardSize.x + 1; i++)
        {
            Instantiate(wallPrefab, new Vector3(i, y, 0), new Quaternion(0, 0, 0, 0), GameObject.Find("Wall").transform);
        }

        int x = (int) -gameboardSize.x - 1;
        for (int i = (int) -gameboardSize.y - 1; i <= gameboardSize.y + 1; i++)
        {
            Instantiate(wallPrefab, new Vector3(x, i, 0), new Quaternion(0, 0, 0, 0), GameObject.Find("Wall").transform);
        }

        x = (int) gameboardSize.x + 1;
        for (int i = (int)-gameboardSize.y - 1; i <= gameboardSize.y + 1; i++)
        {
            Instantiate(wallPrefab, new Vector3(x, i, 0), new Quaternion(0, 0, 0, 0), GameObject.Find("Wall").transform);
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 18;

        int score = snake.length - 3;

        GUI.Label(new Rect(5, 5, 80, 50), "Score: " + Mathf.Max(0, score).ToString(), style);
    }
}
