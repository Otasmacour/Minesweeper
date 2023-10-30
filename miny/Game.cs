using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miny
{
    class Game
    {
        public Game(int y, int x, int percentOfMines) 
        {
            GenerateMap(y, x, percentOfMines);
            twoDArrayHeight = y;
            twoDArrayWidth = x;
            this.percentOfMines = percentOfMines;
        }
        Random random = new Random();
        public Node[,] twoDArray;
        public bool someMineExpoloded;
        public int twoDArrayHeight;
        public int twoDArrayWidth;
        public int percentOfMines;
        public int minesLeft;
        public int numberOfExposed;
        int numberOfminesOnMap;
        public bool run = true;
        public List<Coordinates> GetAdjacentCoordinates(Node[,] twoDArray, Coordinates coordinates, int fourOrEight)
        {
            List<Coordinates> adjacentCoordinates = new List<Coordinates>();
            bool Left = false; bool Right = false; bool Up = false; bool Down = false;
            if (coordinates.x > 0)
            {
                Left = true;
                Coordinates left = new Coordinates();
                left.y = coordinates.y;
                left.x = coordinates.x - 1;
                adjacentCoordinates.Add(left);
            }
            if (coordinates.x <= twoDArray.GetLength(1) - 2)
            {
                Right = true;
                Coordinates right = new Coordinates();
                right.y = coordinates.y;
                right.x = coordinates.x + 1;
                adjacentCoordinates.Add(right);
            }
            if (coordinates.y > 0)
            {
                Up = true;
                Coordinates up = new Coordinates();
                up.y = coordinates.y - 1;
                up.x = coordinates.x;
                adjacentCoordinates.Add(up);
            }
            if (coordinates.y <= twoDArray.GetLength(0) - 2)
            {
                Down = true;
                Coordinates down = new Coordinates();
                down.y = coordinates.y + 1;
                down.x = coordinates.x;
                adjacentCoordinates.Add(down);
            }
            if (fourOrEight == 8)
            {
                if (Left && Up)
                {
                    Coordinates topLeft = new Coordinates();
                    topLeft.y = coordinates.y - 1;
                    topLeft.x = coordinates.x - 1;
                    adjacentCoordinates.Add(topLeft);
                }
                if (Right && Up)
                {
                    Coordinates topRight = new Coordinates();
                    topRight.y = coordinates.y - 1;
                    topRight.x = coordinates.x + 1;
                    adjacentCoordinates.Add(topRight);
                }
                if (Left && Down)
                {
                    Coordinates bottomLeft = new Coordinates();
                    bottomLeft.y = coordinates.y + 1;
                    bottomLeft.x = coordinates.x - 1;
                    adjacentCoordinates.Add(bottomLeft);
                }
                if (Right && Down)
                {
                    Coordinates bottomRight = new Coordinates();
                    bottomRight.y = coordinates.y + 1;
                    bottomRight.x = coordinates.x + 1;
                    adjacentCoordinates.Add(bottomRight);
                }
            }
            return adjacentCoordinates;
        }
        public (bool end, bool victory) VictoryCheck()
        {
            bool end = false;
            bool victory = false;
            if((numberOfExposed + numberOfminesOnMap == twoDArrayWidth * twoDArrayHeight) && minesLeft == 0)
            {
                end = true;
                run = false;
                victory = true;
            }
            else if(someMineExpoloded)
            {
                end = true;
                run = false;
                victory = false;
            }
            return(end, victory);
        }
        public void GenerateMap(int y, int x, int percentOfMines)
        {
            int numberOfMinesToSet = (x * y * percentOfMines) / 100;
            numberOfminesOnMap = numberOfMinesToSet;
            minesLeft = numberOfMinesToSet;
            twoDArray = new Node[y, x];
            //Creating nodes
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Node node = new Node();
                    Coordinates coordinates = new Coordinates();
                    coordinates.y = y;
                    coordinates.x = x;
                    node.coordinates = coordinates;
                    twoDArray[i, j] = node;
                }
            }
            //Seting mines
            while (numberOfMinesToSet > 0)
            {
                int X = random.Next(x);
                int Y = random.Next(y);
                if(twoDArray[Y, X].mine == false)
                {
                    twoDArray[Y, X].mine = true;
                    numberOfMinesToSet--;
                } 
            }
            //Updating the number of adjacent mines
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    Node node = twoDArray[i, j];
                    if(node.mine == false)
                    {
                        Coordinates coordinates = new Coordinates();
                        coordinates.y = i;
                        coordinates.x = j;
                        node.coordinates = coordinates;
                        List<Coordinates> adjacentCoordinates = GetAdjacentCoordinates(twoDArray, coordinates, 8);
                        foreach (Coordinates coordinates1 in adjacentCoordinates)
                        {
                            Node adjacentNode = twoDArray[coordinates1.y, coordinates1.x];
                            if (adjacentNode.mine)
                            {
                                node.numberOfMinesAround++;
                            }
                        }
                    } 
                }
            }
        }
        public void Wawe(Node node)
        {
            List<Node> result = new List<Node>();
            Queue<Node> queue = new Queue<Node>();
            result.Add(node);
            queue.Enqueue(node);
            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();
                List<Coordinates> adjacentCoordinates = GetAdjacentCoordinates(twoDArray, current.coordinates, 8);
                foreach (Coordinates adjacent in adjacentCoordinates)
                {
                    Node adjacentNode = twoDArray[adjacent.y, adjacent.x];
                    if(result.Contains(adjacentNode) == false && adjacentNode.mine == false)
                    {
                        if (adjacentNode.numberOfMinesAround == 0 )
                        {
                            result.Add(adjacentNode);
                            queue.Enqueue(adjacentNode);
                        }
                        else
                        {
                            result.Add(adjacentNode);
                        }
                    }
                    
                }
            }
            foreach (Node n in result)
            {
                n.label.Text = n.numberOfMinesAround.ToString();
                n.exposed = true;
                numberOfExposed++;
            }
        }
        public Node GetNode(int order)
        {
            int y = order / twoDArrayWidth;
            int x = order - y * twoDArrayWidth;
            return twoDArray[y, x];
        }
    }
}
