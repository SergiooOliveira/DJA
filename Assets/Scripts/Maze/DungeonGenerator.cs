using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool isVisited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public Vector2Int minPosition;
        public Vector2Int maxPosition;

        public bool obligatory;

        public int ProbabilityOfSpawning(int x, int y)
        {
            // return 0 - Cannot Spawn
            // return 1 - Can Spawn
            // return 2 - Has to Spawn

            if (x >= minPosition.x && x <= maxPosition.x && y >= minPosition.y && y <= maxPosition.y)
            {
                return obligatory ? 2 : 1;
            }

            return 0;
        }
    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;

    List<Cell> board;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateDungeon()
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                Cell currentCell = board[i + j * size.x];
                if (currentCell.isVisited)
                {
                    int randomRoom = -1;
                    List<int> availableRooms = new List<int>();

                    for (int k = 0; k < rooms.Length; k++)
                    {
                        int probability = rooms[k].ProbabilityOfSpawning(i, j);

                        if (probability == 2)
                        {
                            randomRoom = k;
                            break;
                        }
                        else if (probability == 1)
                        {
                            availableRooms.Add(k);
                        }
                    }

                    if (randomRoom == -1)
                    {
                        if (availableRooms.Count > 0)
                        {
                            randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                        }
                        else
                        {
                            randomRoom = 0;
                        }
                    }

                    var newRoom = Instantiate(rooms[randomRoom].room, new Vector3(i * offset.x, 0, -j * offset.y), Quaternion.identity, transform).GetComponent<RoomBehaviour>();
                    newRoom.UpdateRoom(currentCell.status);

                    newRoom.name += " " + i + "-" + j;
                }
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;

        Stack<int> path = new Stack<int>();

        int k = 0;

        while (k < 1000)
        {
            k++;
            board[currentCell].isVisited = true;

            if (currentCell == board.Count - 1) break;

            // Check the cell neighbors
            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    // Down or Right
                    if (newCell - 1 == currentCell)
                    {
                        // Goes Right
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        // Goes Down
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    // Up or Left
                    if (newCell + 1 == currentCell)
                    {
                        // Goes Left
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        // Goes Up
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }
        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        // Check up neighbor
        if (cell - size.x >= 0 && !board[cell - size.x].isVisited)
        {
            neighbors.Add(cell - size.x);
        }

        // Check down neighbor
        if (cell + size.x < board.Count && !board[cell + size.x].isVisited)
        {
            neighbors.Add(cell + size.x);
        }

        // Check right neighbor
        if ((cell + 1) % size.x != 0 && !board[cell + 1].isVisited)
        {
            neighbors.Add(cell + 1);
        }

        // Check left neighbor
        if (cell % size.x != 0 && !board[cell - 1].isVisited)
        {
            neighbors.Add(cell - 1);
        }

        return neighbors;
    }
}