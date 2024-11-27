using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnlyMino : MonoBehaviour
{
    public float previousTime;
    public float fallTime = 1f;
    public bool shift = false;
    private static int width = 10;
    private static int height = 20;

    public static Transform[,] grid = new Transform[width, height];

    private int keep = 0;

    public static bool isClear = false;


    public enum Direction
    {
        left,
        right,
        up,
        down
    }

    public Direction direction;

    void Start()
    {
        direction = Direction.left;
    }

    void Update()
    {
        if (!SpawnMino.isGameOver&&!SpawnMino.isClear || !isClear)
        {
            MinoMovememt();
        }
        AddToGrid();
    }

    private void MinoMovememt()
    {
        //�V�����ʒu��grid�ɒǉ�
        if (Time.time - previousTime >= fallTime)
        {
            //���݈ʒu��grid����폜
            RemoveFromGrid();


            //���ɓ����ꍇ
            if (direction == Direction.left)
            {
                transform.position += new Vector3(-1, 0, 0);

                if (!ValidMovement())
                {
                    transform.position += new Vector3(1, 0, 0);
                }
                previousTime = Time.time;
            }

            //�E�ɓ����ꍇ
            else if (direction == Direction.right)
            {
                transform.position += new Vector3(1, 0, 0);

                if (!ValidMovement())
                {
                    transform.position += new Vector3(-1, 0, 0);
                }
                previousTime = Time.time;
            }

            //��ɓ����ꍇ
            else if (direction == Direction.up)
            {
                transform.position += new Vector3(0, 1, 0);

                if (!ValidMovement())
                {
                    transform.position += new Vector3(0, 1, 0);
                }
                previousTime = Time.time;

                keepdirection();

            }
            //���ɓ����ꍇ
            else if (direction == Direction.down)
            {
                transform.position += new Vector3(0, -1, 0);
                if (!ValidMovement())
                {
                    transform.position += new Vector3(0, -1, 0);
                }
                previousTime = Time.time;

                keepdirection();

            }
        }
    }

    //�ߋ��̈ʒu�� grid ����폜
    void RemoveFromGrid()
    {
        foreach (Transform child in transform)
        {
            int roundX = Mathf.RoundToInt(child.position.x);
            int roundY = Mathf.RoundToInt(child.position.y);

            if (roundX >= 0 && roundX < width && roundY >= 0 && roundY < height)
            {
                grid[roundX, roundY] = null;
            }
        }
    }

    //�O���b�h�ւ̒ǉ�
    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            if (roundX >= 0 && roundX < width && roundY >= 0 && roundY < height)
            {
                grid[roundX, roundY] = children;
            }

            if(roundY >= 9)
            {
                isClear = true;
            }
        }

    }

    bool ValidMovement()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            //�X�e�[�W�͈̔͊O�ɏo���ꍇ
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                changedirection();
                return false;
            }

            //�i�s�������L�^
            if (direction == Direction.left)
            {
                keep = 0;
            }
            else if (direction == Direction.right)
            {
                keep = 1;
            }

            //���������󂢂Ă���ꍇ
            if (roundY > 0 && grid[roundX, roundY - 1] == null && grid[roundX, roundY] == null)
            {
                if (roundY >= 2 && grid[roundX, roundY - 2] == null)
                {
                    changedirection();
                    return false;
                }
                else
                {
                    direction = Direction.down;
                }
            }
            //�i�s���Mino������A����������󂢂Ă���ꍇ
            else if (roundY + 1 < height && roundX >= 0 && roundX < width && grid[roundX, roundY] != null && grid[roundX, roundY + 1] == null)
            {
                if (direction == Direction.left && grid[roundX + 1, roundY + 1] == null)
                {
                    direction = Direction.up;
                }
                else if (direction == Direction.right && grid[roundX - 1, roundY + 1] == null)
                {
                    direction = Direction.up;
                }
                return false;
            }
            //�i�s���Mino������A��������ǂ���Ă���ꍇ
            else if (grid[roundX, roundY] != null)
            {
                changedirection();
                return false;
            }
        }
        
        return true;
    }


    void keepdirection()
    {
        if (keep == 0)
        {
            direction = Direction.left;
        }
        else if (keep == 1)
        {
            direction = Direction.right;
        }
    }

    void changedirection()
    {
        if (direction == Direction.left)
        {
            direction = Direction.right;
        }
        else if (direction == Direction.right)
        {
            direction = Direction.left;
        }
    }
}
