using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{
    private GameBoard board;
    private float fallSpeed = GameManager.DROP_SECONDS; // Initial fall speed
    private float fastFallSpeed = 0.05f; // Speed for faster falling
    private float fallTimer = 0f; // Timer to track fall time

    // Start is called before the first frame update
    void Start()
    {
        board = Camera.main.GetComponent<GameBoard>();
        if (CheckClash())
        {
            Camera.main.GetComponent<GameManager>().GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Continuously update the fall timer
        fallTimer += Time.deltaTime;

        // Check if it's time for the piece to fall based on fallSpeed
        if (fallTimer >= fallSpeed)
        {
            Fall();
            fallTimer = 0f; // Reset the timer
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Rotate(-1);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!CheckLeft())
            {
                transform.position += Vector3.left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!CheckRight())
            {
                transform.position += Vector3.right;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            fallSpeed = fastFallSpeed; // Increase fall speed when "S" is pressed
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            fallSpeed = GameManager.DROP_SECONDS; // Reset fall speed when "S" is released
        }
    }

    private void Fall()
    {
        if (CheckStop())
        {
            // Done, lock it in
            board.ConvertPiece(this);
            return;
        }

        // Otherwise, move it down one
        transform.position += Vector3.down;
    }

    private bool CheckClash()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = child.position;

            if (Mathf.RoundToInt(pos.y) < GameBoard.BOARD_HEIGHT &&
                board.CheckSquare(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)))
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckLeft()
    {
        // Return true if there's a collision (wall or piece)
        // Loop through all individual squares
        foreach (Transform child in transform)
        {
            Vector3 pos = child.position;
            if (pos.x <= 0 ||
                board.CheckSquare(
                    Mathf.RoundToInt(pos.x - 1),
                    Mathf.RoundToInt(pos.y)
                ))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckRight()
    {
        // Return true if there's a collision (wall or piece)
        // Loop through all individual squares
        foreach (Transform child in transform)
        {
            Vector3 pos = child.position;
            if (pos.x >= GameBoard.BOARD_WIDTH - 1 ||
                board.CheckSquare(
                    Mathf.RoundToInt(pos.x + 1),
                    Mathf.RoundToInt(pos.y)
                ))
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckStop()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = child.position;
            if (pos.y <= 0 ||
                board.CheckSquare(
                    Mathf.RoundToInt(pos.x),
                    Mathf.RoundToInt(pos.y) - 1)
                )
            {
                return true;
            }
        }

        return false;
    }

    private bool IsPositionInvalid(int x, int y)
    {
        if (x < 0 || x >= GameBoard.BOARD_WIDTH)
        {
            return true;
        }

        if (y < 0 || y >= GameBoard.BOARD_HEIGHT)
        {
            return true;
        }

        return false;
    }

    private void Rotate(int dir)
    {
        // Rotate to check
        transform.Rotate(dir * Vector3.back * 90);

        // Check if position is valid &
        // if there's a collision

        foreach (Transform child in transform)
        {
            Vector3 pos = child.position;
            if (IsPositionInvalid(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)) ||
                board.CheckSquare(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)))
            {
                transform.Rotate(dir * Vector3.back * -90);
                return;
            }
        }
    }
}
