using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Threading.Tasks;
using static UnityEditor.PlayerSettings;

public class Snake : MonoBehaviour
{
    [Header("Snake info")]
    public List<Vector2> snakePos = new List<Vector2>();
    public List<GameObject> snakeBody = new List<GameObject>();
    public int length = 1;

    public bool increaseLength = false;

    public float frequency = 0.5f;
    private float time;

    public GameObject snakeBodyPrefab;

    private bool4 direction;

    void Awake()
    {
        direction = false;
        direction.w = true;

        snakePos.Add(transform.position);
        snakeBody.Add(gameObject);
    }

    void Update()
    {
        time += Time.deltaTime;

        InputHandler();

        if (time > frequency) 
        {
            time = 0;

            Vector2 pos = transform.position;

            if (direction.x)
            {
                pos.x++;
            }
            else if (direction.y)
            {
                pos.x--;
            }
            else if (direction.z)
            {
                pos.y++;
            }
            else if (direction.w)
            {
                pos.y--;
            }

            gameObject.transform.position = pos;

            if (increaseLength)
            {
                IncreaseLength();
            }

            UpdateLocations();
        }
    }

    private void IncreaseLength()
    {
        length++;
        increaseLength = false;

        int x = (int)snakePos[snakePos.Count - 1].x;
        int y = (int)snakePos[snakePos.Count - 1].y;

        snakeBody.Add(Instantiate(snakeBodyPrefab, new Vector3(x, y, 0), new Quaternion(0, 0, 0, 0), gameObject.transform.parent));
        snakePos.Add(snakeBody[snakeBody.Count - 1].transform.position);
    }

    private void InputHandler()
    {
        if(Input.GetKey(KeyCode.W))
        {
            direction = false;
            direction.z = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            direction = false;
            direction.y = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = false;
            direction.w = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = false;
            direction.x = true;
        }
    }

    private void UpdateLocations()
    {
        for (int i = 1; i < length; i++)
        {
            snakeBody[i].transform.position = new Vector3(snakePos[i - 1].x, snakePos[i - 1].y, 0);
        }

        for(int i = 0; i < length; i++)
        {
            snakePos[i] = snakeBody[i].transform.position;
        }
    }

    private void OnDestroy()
    {
        foreach (GameObject snake in snakeBody)
        {
            Destroy(snake);
        }
    }
}
