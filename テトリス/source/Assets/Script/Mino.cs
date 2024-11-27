using UnityEngine;
using UnityEngine.UI;

public class Mino : MonoBehaviour
{
    private ScoreManager scoreManager;
    public float previousTime;
    public float fallTime = 1f;

    private static int width = 10;
    private static int height = 20;

    public Vector3 rotationPoint;

    public static Transform[,] grid = MoveOnlyMino.grid;

    private Vector3 originalScale;

    private bool HoldCount = false;

    public static bool hasOnlyTag = false;

    void Start()
    {
        originalScale = transform.localScale;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        MinoMovememt();
    }

    private void MinoMovememt()
    {
        //���ړ�
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            if (!ValidMovement())
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        //�E�ړ�
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (!ValidMovement())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        //���ړ��A�����~��
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - previousTime >= fallTime)
        {
            transform.position += new Vector3(0, -1, 0);

            if (!ValidMovement())
            {
                transform.position += new Vector3(0, +1, 0);

                AddToGrid();
                CheckLines();
                this.enabled = false;
                FindObjectOfType<SpawnMino>().NewMino();
            }

            previousTime = Time.time;
        }
        //�u�ԍ~��
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //���݂̈ʒu���牺�ɍ~����������
            while (ValidMovement())
            {
                transform.position += new Vector3(0, -1, 0);
            }

            transform.position += new Vector3(0, 1, 0);

            AddToGrid();
            CheckLines();
            this.enabled = false;
            FindObjectOfType<SpawnMino>().NewMino();
        }
        //�E90����]
        else if (Input.GetKeyDown(KeyCode.X))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

            if (!ValidMovement())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            }
        }
        //��90����]
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);

            if (!ValidMovement())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }
        //�z�[���h�@�\
        else if (Input.GetKeyDown(KeyCode.C) && !HoldCount)
        {
            HoldMino();
        }
    }

    void HoldMino()
    {
        SpawnMino spawnMino = FindObjectOfType<SpawnMino>();
        if (spawnMino == null) return;

        if (spawnMino.holdMino == null)
        {
            //�z�[���h���̃~�m���Ȃ��ꍇ�A���݂̃~�m���z�[���h
            HoldActive(spawnMino);
            FindObjectOfType<SpawnMino>().NewMino();
        }
        else
        {
            //�z�[���h���̃~�m�ƌ��݂̃~�m�����ւ���
            GameObject temp = spawnMino.holdMino;
            HoldActive(spawnMino);

            temp.SetActive(true);
            temp.transform.position = spawnMino.transform.position;
            temp.transform.localScale = originalScale;
            temp.GetComponent<Mino>().enabled = true;
        }

        //���݂̃~�m���z�[���h�G���A�Ɉړ�
        transform.localScale = originalScale;     //�X�P�[�������Z�b�g
        transform.rotation = Quaternion.identity; //��]�����Z�b�g
        this.enabled = false;
        HoldCount = true;
    }

    void HoldActive(SpawnMino spawnMino)
    {
        spawnMino.holdMino = this.gameObject;
        spawnMino.holdMino.SetActive(false);
        spawnMino.UpdateHoldMinoDisplay();
        spawnMino.holdMino.SetActive(true);
    }

    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            //������20�ȏ�Ȃ�Q�[���I�[�o�[���������s
            if (roundY >= height - 1)
            {
                GameOver();
                return;
            }

            grid[roundX, roundY] = children;
        }
    }

    void GameOver()
    {
        SpawnMino.isGameOver = true;

    }

    bool ValidMovement()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }

            if (grid[roundX, roundY] != null)
            {
                return false;
            }
        }
        return true;
    }

    public void CheckLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DeleteLine(i);
                RowDown(i);
                AddScore(50);
            }
        }
    }

    void AddScore(int points)
    {
        if (scoreManager != null)
        {
            scoreManager.AddScore(points);
        }
    }

    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] != null)
            {
                //�^�O��"Only"�ł���΃t���O�𗧂Ă�
                if (grid[j, i].CompareTag("Only"))
                {
                    hasOnlyTag = true;
                }

                //�Q�[���I�u�W�F�N�g���폜
                Destroy(grid[j, i].gameObject);
                grid[j, i] = null;
            }
        }

        if (hasOnlyTag)
        {
            SpawnMino.isGameOver = true;
        }
    }

    public void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].position += new Vector3(0, -1, 0);
                }
            }
        }
    }
}
