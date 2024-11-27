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
        //新しい位置をgridに追加
        if (Time.time - previousTime >= fallTime)
        {
            //現在位置をgridから削除
            RemoveFromGrid();


            //左に動く場合
            if (direction == Direction.left)
            {
                transform.position += new Vector3(-1, 0, 0);

                if (!ValidMovement())
                {
                    transform.position += new Vector3(1, 0, 0);
                }
                previousTime = Time.time;
            }

            //右に動く場合
            else if (direction == Direction.right)
            {
                transform.position += new Vector3(1, 0, 0);

                if (!ValidMovement())
                {
                    transform.position += new Vector3(-1, 0, 0);
                }
                previousTime = Time.time;
            }

            //上に動く場合
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
            //下に動く場合
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

    //過去の位置を grid から削除
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

    //グリッドへの追加
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

            //ステージの範囲外に出た場合
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                changedirection();
                return false;
            }

            //進行方向を記録
            if (direction == Direction.left)
            {
                keep = 0;
            }
            else if (direction == Direction.right)
            {
                keep = 1;
            }

            //下方向が空いている場合
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
            //進行先にMinoがあり、かつ上方向が空いている場合
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
            //進行先にMinoがあり、上方向も塞がれている場合
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
