using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarScout {

    //private int startX;
    //private int startY;

    private List<Node> tilesToSearch = new List<Node>();
    private List<Node> searchedTiles = new List<Node>();


    private bool allowDiag = false;




    public AStarScout(int _startX, int _startY)
    {
        //startX = _startX;
        //startY = _startY;

        tilesToSearch.Clear();
        searchedTiles.Clear();
    }

    public AStarScout(int _startX, int _startY, bool diag)
    {
        //startX = _startX;
        //startY = _startY;

        allowDiag = diag;

        tilesToSearch.Clear();
        searchedTiles.Clear();
    }


    public void AssignNewPositions(int _startX, int _startY, int targetX, int targetY)
    {
        //startX = _startX;
        //startY = _startY;

        //allowDiag = diag;

        tilesToSearch.Clear();
        searchedTiles.Clear();
    }


    public Path AStarSearch(int startX, int startY, int targetX, int targetY)
    {

        Node target = null;

        tilesToSearch.Clear();
        searchedTiles.Clear();

        GameManager.instance.gridNodes[startX][startY].Cost = 0;
        GameManager.instance.gridNodes[startX][startY].PosInPath = 0;

        AddToTilesToSearch(startX, startY);

        //GameManager.instance.gridNodes[targetX][targetY].SetParent(null);


        while (tilesToSearch.Count != 0)
        {

            Node current = tilesToSearch[0];

            if (GameManager.instance.gridNodes[current.X][current.Y] == GameManager.instance.gridNodes[targetX][targetY])
            {
                target = current;
                tilesToSearch.Clear();
                continue;
            }

            tilesToSearch.Remove(current);
            searchedTiles.Add(current);

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {

                    if ((x == 0) && (y == 0))
                    {
                        continue;
                    }

                    if (!allowDiag)
                    {
                        if ((x != 0) && (y != 0))
                        {
                            continue;
                        }
                    }



                    int currentNeighborX = current.X + x;
                    int currentNeighborY = current.Y + y;



                    bool isOffBoard = (currentNeighborX < 0 || currentNeighborY < 0 || currentNeighborX >= GameManager.instance.gridNodes.Length || currentNeighborY >= GameManager.instance.gridNodes[currentNeighborX].Length);

                    if (!isOffBoard && GameManager.instance.gridNodes[currentNeighborX][currentNeighborY].Type != TileType.Wall)
                    {

                        Node currentNeighbor = GameManager.instance.gridNodes[currentNeighborX][currentNeighborY];

                        if (GameManager.instance.gridNodes[currentNeighbor.X][currentNeighbor.Y] == GameManager.instance.gridNodes[targetX][targetY])
                        {
                            target = currentNeighbor;
                            tilesToSearch.Clear();
                        }


                        if (currentNeighbor.X == startX && currentNeighbor.Y == startY)
                            continue;

                        float nextStepCost = current.Cost + 1;

                        if (nextStepCost < currentNeighbor.Cost)
                        {
                            if (tilesToSearch.Contains(currentNeighbor))
                            {
                                tilesToSearch.Remove(currentNeighbor);
                            }
                            if (searchedTiles.Contains(currentNeighbor))
                            {
                                searchedTiles.Remove(currentNeighbor);
                            }
                        }

                        if (!(tilesToSearch.Contains(currentNeighbor)) && !(searchedTiles.Contains(currentNeighbor)))
                        {
                            currentNeighbor.Cost = nextStepCost;
                            currentNeighbor.Heuristic = 1;
                            currentNeighbor.SetParent(current);
                            AddToTilesToSearch(currentNeighbor);
                        }
                        else
                        {
                            continue;
                        }
                        
                    }
                    else
                    {
                        continue;
                    }


                }
            }
            

        }

        if (target == null)
            throw new System.Exception("No Target Found...");

        Path path = new Path();
        while (target != GameManager.instance.gridNodes[startX][startY])
        {
            path.PrependStep(target.X, target.Y);
            target = target.GetParent();
        }

        path.PrependStep(startX, startY);

        return path;

    }




    public void AddToTilesToSearch(Node node)
    {
        tilesToSearch.Add(node);
        tilesToSearch.Sort();
    }

    public void AddToTilesToSearch(int x, int y)
    {
        AddToTilesToSearch(GameManager.instance.gridNodes[x][y]);
    }

    public void AddToSearched(int x, int y)
    {
        searchedTiles.Add(GameManager.instance.gridNodes[x][y]);
        GameManager.instance.gridNodes[x][y].IsSearched = true;
    }



  
    private void MarkTileSearched(int x, int y)
    {
        GameManager.instance.gridNodes[x][y].IsSearched = true;
    }

    


}
