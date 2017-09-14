using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public static int gridWidth = 10;
    public static int gridHeight = 23; //23
    public static Transform[,] grid = new Transform[gridWidth, gridHeight];
    public static int scoreOneLine = 100;
    public static int scoreTwoLine = 200;
    public static int scoreThreeLine = 300;
    public static int scoreFourLine = 800;
    public static int scoreBTB = 1200;
    public int clearedFourCounter = 0;
    private int numberOfRowsThisTurn = 0;
    public Text hud_score;
    public static int currentScore = 0;
    public AudioSource audioSource;
    public AudioClip clearRows;
    public static bool Mute = false;

	// Use this for initialization
	void Start () {
        SpawnNextTetromino();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateScore();
        UpdateUI();
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            if(Mute)
            {
                Mute = false;
                audioSource.mute = false;
                Tetromino.audioSource.mute = false;
            } else
            {
                Mute = true;
                audioSource.mute = true;
                Tetromino.audioSource.mute = true;
            }
        }
        
	}

    public void UpdateUI()
    {
        hud_score.text = currentScore.ToString();
    }

    public bool CheckIsAboveGrid(Tetromino tetromino)
    {
        for (int x=0; x < gridWidth; x++)
        {
            foreach(Transform mino in tetromino.transform)
            {
                Vector2 pos = Round(mino.position);
                if(pos.y > gridHeight -1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool IsFullRowAt (int row)
    {
        for(int x=0; x<gridWidth; x++)
        {
            if (grid[x, row] == null)
            {
                return false;
            }
        }
        numberOfRowsThisTurn++;
        return true;
    }

    public void DeleteMinoAt(int y)
    {
        for(int x=0; x<gridWidth; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = 0; x<gridWidth; x++)
        {
            if(grid[x,y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; i++)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow()
    {
        for (int y=0; y<gridHeight; y++)
        {
            if(IsFullRowAt(y))
            {
                DeleteMinoAt(y);
                MoveAllRowsDown(y + 1);
                y--;
            }
        }
    }

    public void UpdateGrid(Tetromino tetromino)
    {
        for (int y=0; y<gridHeight; y++)
        {
            for (int x=0; x<gridWidth; x++)
            {
                if (grid[x,y] != null)
                {
                    if (grid[x, y].parent == tetromino.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }
        foreach(Transform mino in tetromino.transform)
        {
            Vector2 pos = Round(mino.position);
            if(pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }

    public Transform GetTransformAtGridPosition(Vector2 pos)
    {
        if (pos.y > gridHeight-1)
        {
            return null;
        } else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }
    public void SpawnNextTetromino()
    {

        GameObject nextTetromino = (GameObject)Instantiate(Resources.Load(GetRandomTetromino(), typeof(GameObject)), new Vector2(5.0f, 22.0f), Quaternion.identity);
    }

    public bool CheckInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    public Vector2 Round (Vector2 pos)
    {
        Vector2 positions = new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    private string GetRandomTetromino()
    {
        int rnd = Random.Range(1, 8);
        string randomTetrominoName = "Prefabs/Itetromino";

        switch (rnd)
        {
            case 1:
                randomTetrominoName = "Prefabs/Itetromino";
                break;
            case 2:
                randomTetrominoName = "Prefabs/Jtetromino";
                break;
            case 3:
                randomTetrominoName = "Prefabs/Ltetromino";
                break;
            case 4:
                randomTetrominoName = "Prefabs/Otetromino";
                break;
            case 5:
                randomTetrominoName = "Prefabs/Stetromino";
                break;
            case 6:
                randomTetrominoName = "Prefabs/Ttetromino";
                break;
            case 7:
                randomTetrominoName = "Prefabs/Ztetromino";
                break;
        }
        return randomTetrominoName;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOVer");
    }

    public void UpdateScore()
    {
        switch(numberOfRowsThisTurn)
        {
            case 0:
                break;
            case 1:
                ClearedOneLine();
                audioSource.PlayOneShot(clearRows);
                break;
            case 2:
                ClearedTwoLines();
                audioSource.PlayOneShot(clearRows);
                break;
            case 3:
                ClearedThreeLine();
                audioSource.PlayOneShot(clearRows);
                break;
            case 4:
                ClearedFourLine();
                audioSource.PlayOneShot(clearRows);
                break;
        }
        numberOfRowsThisTurn = 0;
    }

    public void ClearedOneLine()
    {
        clearedFourCounter = 0;
        currentScore += scoreOneLine;
    }
    public void ClearedTwoLines()
    {
        clearedFourCounter = 0;
        currentScore += scoreTwoLine;
    }
    public void ClearedThreeLine()
    {
        clearedFourCounter = 0;
        currentScore += scoreThreeLine;
    }
    public void ClearedFourLine()
    {
        if(clearedFourCounter != 0)
        {
            currentScore += scoreBTB; 
        } else
        {
            currentScore += scoreFourLine;
            clearedFourCounter++;
        }
    }
}
