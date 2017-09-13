using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    float lastfall = 0;
    public float fallspeed = 1;
    public bool allowrotation = true;
    public bool limitrotation = false;
    public int width;
    public int individualScore = 100;
    private float livedTime;
    public AudioClip moveSound;
    public AudioClip rotateSound;
    public AudioClip landSound;
    public static AudioSource audioSource;
    // Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckUserInput();
        UpdateIndividualScore();
    }

    void UpdateIndividualScore()
    {
        if (livedTime <1)
        {
            livedTime += Time.deltaTime;
        } else
        {
            livedTime = 0;
            individualScore = Mathf.Max(individualScore - 10, 0);
        }
    }

    void CheckUserInput() {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if(CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
                audioSource.PlayOneShot(moveSound);
            } else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
                audioSource.PlayOneShot(moveSound);
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        } else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowrotation)
            {
                if (limitrotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, 90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }
                }
                else {
                    transform.Rotate(0, 0, -90);
                }
                if (CheckIsValidPosition())
                {
                    FindObjectOfType<Game>().UpdateGrid(this);
                    audioSource.PlayOneShot(rotateSound);
                } else {
                    if (limitrotation)
                    {
                        if (transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, 90);
                        }
                        else
                        {
                            transform.Rotate(0, 0, -90);
                        }
                    }else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
            }
        } else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastfall >= fallspeed)
        {
            transform.position += new Vector3(0, -1, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
                if(Input.GetKey(KeyCode.DownArrow)) audioSource.PlayOneShot(moveSound);
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                FindObjectOfType<Game>().DeleteRow();
                if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
                {
                    FindObjectOfType<Game>().GameOver();
                }
                audioSource.PlayOneShot(landSound);
                enabled = false;
                Game.currentScore += individualScore;
                FindObjectOfType<Game>().SpawnNextTetromino();
            }
            lastfall = Time.time;
        } else if(Input.GetKeyDown(KeyCode.Space))
        {
            while (CheckIsValidPosition())
            {
                transform.position += new Vector3(0, -1, 0);
                if(CheckIsValidPosition()) FindObjectOfType<Game>().UpdateGrid(this);
            }
            transform.position += new Vector3(0, 1, 0);
            audioSource.PlayOneShot(moveSound);
            FindObjectOfType<Game>().DeleteRow();
            if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
            {
                FindObjectOfType<Game>().GameOver();
            }
            audioSource.PlayOneShot(landSound);
            enabled = false;
            Game.currentScore += individualScore;
            FindObjectOfType<Game>().SpawnNextTetromino();
            lastfall = Time.time;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Time.time - lastfall >= 0.05)
        {
            transform.position += new Vector3(0, -1, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
                if(Input.GetKey(KeyCode.DownArrow)) audioSource.PlayOneShot(moveSound);
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                FindObjectOfType<Game>().DeleteRow();
                if (FindObjectOfType<Game>().CheckIsAboveGrid(this))
                {
                    FindObjectOfType<Game>().GameOver();
                }
                audioSource.PlayOneShot(landSound);
                enabled = false;
                Game.currentScore += individualScore;
                FindObjectOfType<Game>().SpawnNextTetromino();
            }
            lastfall = Time.time;
        }
    }

    bool CheckIsValidPosition ()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if(FindObjectOfType<Game>().CheckInsideGrid(pos) == false)
            {
                return false;
            }
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
