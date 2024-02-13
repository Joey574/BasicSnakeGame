using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Snake : MonoBehaviour
{
    // Inputs
    public bool Up { set => direction.z = true; }
    public bool Down { set => direction.w = true; }
    public bool Left { set => direction.y = true; }
    public bool Right { set => direction.x = true; }
    public bool Reset { set => direction = false; }

    [Header("Snake info")]
    public List<Vector2> snakePos = new List<Vector2>();
    private List<GameObject> snakeBody = new List<GameObject>();

    private bool4 direction;
    public bool increaseLength = false;
    public int length = 1;

    [Header("Movement")]
    public float frequency = 0.5f;
    private float time;

    [Header("Prefabs")]
    public GameObject snakeBodyPrefab;

    void Awake()
    {
        direction = false;
        direction.w = true;

        snakePos.Add(transform.position);
        snakeBody.Add(gameObject);

        StartCoroutine(SetStartingLength());
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

    private IEnumerator SetStartingLength()
    {
        increaseLength = true;

        while (increaseLength == true)
        {
            yield return null;
        }

        increaseLength = true;
    }

    private void OnDestroy()
    {
        foreach (GameObject snake in snakeBody)
        {
            Destroy(snake);
        }
        Debug.Log("Snake Killed");
    }
}
