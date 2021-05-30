using System;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        [Range(0,100)]
        public int RandomFillPercent;
        [Range(0, 100)]
        public int Speed;
        public int Height;
        public int Width;
        public String Seed;
        public bool UseRandomSeed;
        public GameObject Node;
        public float Padding = 1f;

        public bool HasStarted = false;
        public enum Types { GameOfLife, CaveGeneration };
        public Types Algorithm; // This will create the dropdown in the inspector

        public int Generation = 0;

        private GameObject[,] _nodes;
        private Boolean[,] _nodeValues;
        private Boolean[,] _nodeValuesCopy;
        // Start is called before the first frame update
        void Start()
        {
            _nodes = new GameObject[Width, Height];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    var position = new Vector3(i * Padding, 0, j * Padding);
                    _nodes[i, j] = Instantiate(Node, position, Quaternion.identity) as GameObject;
                    _nodes[i, j].GetComponent<Node>().x = i;
                    _nodes[i, j].GetComponent<Node>().y = j;
                }
            }

            GenerateBoard();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GenerateBoard();
            }
        }

        private void GenerateBoard()
        {
            Generation = 0;
            _nodeValues = new bool[Width, Height];
            _nodeValuesCopy = new bool[Width, Height];

            if (UseRandomSeed)
            {
                Seed = UnityEngine.Random.value.ToString(CultureInfo.CurrentCulture);
            }
            System.Random randomNumberGenerator = new System.Random(Seed.GetHashCode());

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _nodeValues[i, j] = randomNumberGenerator.Next(0, 100) < RandomFillPercent ? true : false;
                    if (_nodeValues[i, j] == true)
                    {
                        _nodes[i, j].GetComponent<Node>().SetState(true);
                    }
                    else
                    {
                        _nodes[i, j].GetComponent<Node>().SetState(false);
                    }
                }
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            UnityEngine.Time.timeScale = Speed / 100f;
            if (!HasStarted)
            {
                return;
            }

            _nodeValuesCopy = _nodeValues.Clone() as bool[,];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    CalculateNode(i, j);
                }
            }

            _nodeValues = _nodeValuesCopy;
            Generation++;
        }


        private void CalculateNode(int x, int y)
        {
            int aliveNeighborCount = ActiveNeighborCount(x, y);

            switch (Algorithm)
            {
                case Types.GameOfLife:
                    GameOfLifeAlgorithm(x, y, aliveNeighborCount);
                    break;
                case Types.CaveGeneration:
                    CaveGenerationAlgorithm(x, y, aliveNeighborCount);
                    break;
                default:
                    GameOfLifeAlgorithm(x, y, aliveNeighborCount);
                    break;
            }
        }

        private void GameOfLifeAlgorithm(int x, int y, int aliveNeighborCount)
        {
            if (_nodeValues[x, y] == true)
            {
                if (aliveNeighborCount < 2)
                {
                    // Dies
                    _nodeValuesCopy[x, y] = false;
                    _nodes[x, y].GetComponent<Node>().SetState(false);
                }
                else if (aliveNeighborCount > 3)
                {
                    // Dies
                    _nodeValuesCopy[x, y] = false;
                    _nodes[x, y].GetComponent<Node>().SetState(false);
                }
                // Lives
            }
            // Dead cell checks
            else
            {
                if (aliveNeighborCount == 3)
                {
                    // Born
                    _nodeValuesCopy[x, y] = true;
                    _nodes[x, y].GetComponent<Node>().SetState(true);
                }
            }
        }

        private void CaveGenerationAlgorithm(int x, int y, int aliveNeighborCount)
        {
            if (aliveNeighborCount > 4)
            {
                // Born
                _nodeValuesCopy[x, y] = true;
                _nodes[x, y].GetComponent<Node>().SetState(true);
            }
            else if (aliveNeighborCount < 4)
            {
                // Dies
                _nodeValuesCopy[x, y] = false;
                _nodes[x, y].GetComponent<Node>().SetState(false);
            }
        }

        private int ActiveNeighborCount(int x, int y)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if ((x + i) >= 0 && (x + i) < Width && (y + j) >= 0 && (y + j) < Height && _nodeValues[x + i, y + j] == true) count++;
                }
            }
            if (_nodeValues[x, y] == true) count--; // Remove the cell we are looking at, since it's not its own neighbor
            return count;
        }

        public void ToggleState(int x, int y)
        {
            _nodeValues[x, y] = !_nodeValues[x, y];
            _nodes[x, y].GetComponent<Node>().SetState(_nodeValues[x, y]);
        }
    }
}
