using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static float DROP_SECONDS = 0.5f;
    public static float FAST_DROP_SECONDS = 0.05f;

    public List<GamePiece> pieces;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        GetComponent<GameBoard>().Init();
        SpawnPiece();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0;
        SceneManager.LoadScene("GameOverMenu");
    }

    public void SpawnPiece()
    {
        //choose a piece
        int r = Random.Range(0, pieces.Count);

        //calculate the position
        Vector3 pos = new Vector3(
            (int)(GameBoard.BOARD_WIDTH / 2) - 1,
            GameBoard.BOARD_HEIGHT - 2,
            0
        );

        //instantiate it
        Instantiate(pieces[r], pos, Quaternion.identity);
    }
}
