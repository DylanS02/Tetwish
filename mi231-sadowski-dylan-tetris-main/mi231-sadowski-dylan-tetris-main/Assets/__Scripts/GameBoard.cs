using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public static int BOARD_WIDTH = 10;
    public static int BOARD_HEIGHT = 20;
    public GameObject blackSquare;

    private IndividualSquare[,] board;
    private AudioSource audioSource; 
    public AudioClip yeahClip; 

    public void PlayYeahSound()
    {
        audioSource.PlayOneShot(yeahClip);
    }

    // Start is called before the first frame update
    public void Init()
    {
        Camera cam = GetComponent<Camera>();
        cam.transform.position = new Vector3(BOARD_WIDTH / 2, BOARD_HEIGHT / 2, -1);

        audioSource = GetComponent<AudioSource>();

        // Initialize the game board
        board = new IndividualSquare[BOARD_WIDTH, BOARD_HEIGHT];
        FillGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void AdvanceLines(int startY)
    {
        for (int y = startY; y < BOARD_HEIGHT - 1; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                board[x, y] = board[x, y + 1];
                if (board[x, y] != null)
                {
                    board[x, y].transform.position = new Vector3(x, y, 0);
                }
            }
        }

        for (int x = 0; x < BOARD_WIDTH; x++)
        {
            board[x, BOARD_HEIGHT - 1] = null;
        }

        if (yeahClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(yeahClip);
        }
    }

    private void CheckLines()
    {
        int linesCleared = 0;
        //loop through the board bottom up
        for (int y = 0; y < BOARD_HEIGHT; y++)
        {
            bool rowIsFull = true;

            //loop through the columns (x)
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                //if it finds one that's null it'll go to the next row
                if (board[x, y] == null)
                {
                    rowIsFull = false;
                    break;
                }
            }

            //otherwise clear line
            if (rowIsFull)
            {
                ClearLine(y);
                AdvanceLines(y);

                //don't move up the board because we need to consider the line that just
                // moved into this row
                y--;
                linesCleared++;
            }
        }
    }

    public bool CheckSquare(int x, int y)
    {
        if (x < 0 || x >= BOARD_WIDTH || y < 0 || y >= BOARD_HEIGHT)
        {
            return false; // Return false if indices are out of bounds
        }
        //return true if there's something there, otherwise false
        return board[x, y] != null;
    }

    private void ClearLine(int y)
    {
        for (int x = 0; x < BOARD_WIDTH; x++)
        {
            Destroy(board[x, y].gameObject);
            //clear the space in array
            board[x, y] = null;
        }
    }

    public void ConvertPiece(GamePiece piece)
    {
        for (int i = piece.transform.childCount - 1; i >= 0; i--)
        {
            //get the square component
            IndividualSquare sq = piece.transform.GetChild(i).GetComponent<IndividualSquare>(); ;
            Vector3 position = sq.transform.position;

            //store the individual squares in my array
            if (Mathf.RoundToInt(position.x) >= 0 && Mathf.RoundToInt(position.x) < BOARD_WIDTH &&
                Mathf.RoundToInt(position.y) >= 0 && Mathf.RoundToInt(position.y) < BOARD_HEIGHT)
            {
                board[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)]
                    = sq;

                //re-parent the individual squares
                sq.transform.parent = this.transform;
            }
        }

        //destroy the gameObject
        Destroy(piece.gameObject);

        //call CheckLines()
        CheckLines();

        //start the next piece
        GetComponent<GameManager>().SpawnPiece();
    }

    private void FillGrid()
    {
        for (int x = 0; x < BOARD_WIDTH; x++)
        {
            for (int y = 0; y < BOARD_HEIGHT; y++)
            {
                Instantiate(blackSquare, new Vector3(x, y, .1f), Quaternion.identity);
            }
        }
    }
}
