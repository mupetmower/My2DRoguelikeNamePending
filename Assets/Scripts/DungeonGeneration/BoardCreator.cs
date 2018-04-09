using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The type of tile that will be laid in a specific position.
public enum TileType
{
    Wall, Floor,
}


public class BoardCreator : MonoBehaviour
{

   


    public int columns = 100;                                 // The number of columns on the board (how wide it will be).
    public int rows = 100;                                    // The number of rows on the board (how tall it will be).
    public IntRange numRooms = new IntRange(15, 20);         // The range of the number of rooms there can be.
    public IntRange roomWidth = new IntRange(3, 10);         // The range of widths rooms can have.
    public IntRange roomHeight = new IntRange(3, 10);        // The range of heights rooms can have.
    public IntRange corridorLength = new IntRange(6, 10);    // The range of lengths corridors between rooms can have.
    public IntRange numEnemies = new IntRange(5, 10);         //Range of number of enemies
    public IntRange numWeapons = new IntRange(25, 33);


    public GameObject[] floorTiles;                           // An array of floor tile prefabs.
    public GameObject[] wallTiles;                            // An array of wall tile prefabs.
    public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.
    public GameObject player;
    public GameObject[] enemyTiles;                             //Array to hold enemy prefabs;
    public GameObject stairsDown;


    //private int floorTileCount = 0;
    private Node[][] nodes;
    private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
    private Room[] rooms;                                     // All the rooms that are created for this board.
    private Corridor[] corridors;                             // All the corridors that connect the rooms.

    private Enemy[] enemies;                                  //All enemys

    private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.
    private GameObject enemyHolder;
    private GameObject itemHolder;

    private Weapon[] weapons;


    private ItemHolder AllItems;

    List<Vector3> spawnVectors = new List<Vector3>();
    Vector3 playerPos;
    Vector3 stairPos;



    private void Awake()
    {
        AllItems = GameManager.instance.GetComponent<ItemHolder>();
    }


    public void InitBoard(int level)
    {
        // Create the board holder.
        boardHolder = new GameObject("BoardHolder");
        enemyHolder = new GameObject("EnemyHolder");
        itemHolder = new GameObject("ItemHolder");

        if (level > 1)
        {
            ClearForReset();
        }
        

        SetupTilesArray();
        SetupNodesArray();

        //For now, the player is created here, also
        CreateRoomsAndCorridors();



        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();

        InstantiateTiles();
        InstantiateOuterWalls();

        InstantiateStairsDown();

        CreateEnemies();

        CreateItems();


    }

    void ClearForReset()
    {
        spawnVectors.Clear();
        
    }

    void SetupTilesArray()
    {
        // Set the tiles jagged array to the correct width.
        tiles = new TileType[columns][];

        // Go through all the tiles array...
        for (int i = 0; i < tiles.Length; i++)
        {
            // ... and set each tile array to the correct height.
            tiles[i] = new TileType[rows];
        }

    }

    void SetupNodesArray()
    {
        nodes = new Node[columns][];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Node[rows];
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            for (int j = 0; j < nodes[i].Length; j++)
            {
                nodes[i][j] = new Node(i, j, TileType.Wall);
                //nodes[i][j].Type = TileType.Wall;
                //nodes[i][j].X = i;
                //nodes[i][j].Y = j;
            }
        }

    }


    void CreateRoomsAndCorridors()
    {
        // Create the rooms array with a random size.
        rooms = new Room[numRooms.Random];

        // There should be one less corridor than there is rooms.
        corridors = new Corridor[rooms.Length - 1];

        // Create the first room and corridor.
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        // Setup the first room, there is no previous corridor so we do not use one.
        rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

        // Setup the first corridor using the first room.
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            // Create a room.
            rooms[i] = new Room();

            // Setup the room based on the previous corridor.
            rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

            // If we haven't reached the end of the corridors array...
            if (i < corridors.Length)
            {
                // ... create a corridor.
                corridors[i] = new Corridor();

                // Setup the corridor based on the room that was just created.
                corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
            }


            //This is where player is instantiated, in a room about halfway through the array
            if (i == (int)(rooms.Length * .5f))
            {
                playerPos = new Vector3(rooms[i].xPos, rooms[i].yPos, 0);
                Instantiate(player, playerPos, Quaternion.identity);
            }
        }

    }



    //create stairs to next level in random position in random room.
    void InstantiateStairsDown()
    {
        //Pick a random room
        Room r = rooms[Random.Range(0, rooms.Length)];

        int x = Random.Range(0, r.roomWidth);
        int y = Random.Range(0, r.roomHeight);

        stairPos = new Vector3(r.xPos + x, r.yPos + y, 0);
        Instantiate(stairsDown, stairPos, Quaternion.identity);
    }



    //Create and instantiate enemies at random positions.
    void CreateEnemies()
    {
        enemies = new Enemy[numEnemies.Random];
 
        
        for (int x = 0; x < tiles.Length; x++)
        {
            for (int y = 0; y < tiles[x].Length; y++)
            {
                if (tiles[x][y] == TileType.Floor)
                {
                    spawnVectors.Add(new Vector3(x, y, 0));

                }
            }
        }

        spawnVectors.Remove(playerPos);
        spawnVectors.Remove(stairPos);

        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 tempVector = new Vector3();
            tempVector = spawnVectors[Random.Range(0, spawnVectors.Count)];
            InstantiateFromArray(enemyTiles, tempVector.x, tempVector.y, enemyHolder);

            spawnVectors.Remove(tempVector);

        }

    }




    void CreateItems()
    {
        weapons = new Weapon[numWeapons.Random];
        

        for (int i = 0; i < weapons.Length; i++)
        {
            Vector3 tempVector = new Vector3();
            tempVector = spawnVectors[Random.Range(0, spawnVectors.Count)];
            InstantiateFromArray(AllItems.Weapons, tempVector.x, tempVector.y, itemHolder);

            spawnVectors.Remove(tempVector);

        }
    }






    void SetTilesValuesForRooms()
    {
        // Go through all the rooms...
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            // ... and for each room go through it's width.
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                // For each horizontal tile, go up vertically through the room's height.
                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;

                    // The coordinates in the jagged array are based on the room's position and it's width and height.
                    tiles[xCoord][yCoord] = TileType.Floor;
                    nodes[xCoord][yCoord].Type = TileType.Floor;
                }
            }
        }
    }


    void SetTilesValuesForCorridors()
    {
        // Go through every corridor...
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            // and go through it's length.
            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                // Start the coordinates at the start of the corridor.
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                // Depending on the direction, add or subtract from the appropriate
                // coordinate based on how far through the length the loop is.
                switch (currentCorridor.direction)
                {
                    case Direction.North:
                        yCoord += j;
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j;
                        break;
                    case Direction.West:
                        xCoord -= j;
                        break;
                }

                // Set the tile at these coordinates to Floor.
                tiles[xCoord][yCoord] = TileType.Floor;
                nodes[xCoord][yCoord].Type = TileType.Floor;
            }
        }
    }





    //For this, instead of instantiating all of the tiles in the entire grid to be floor prefabs, you can iterate through the tiles[][] array with the for loops and
    //use ifs to set all of them that are not TileType.Floor or TileType.Wall to be black space or something, all of the TileType.Floor to be floor prefabs, and all of
    //TileType.Wall to be wall prefabs. Or just simply if floor put floor, if wall put wall.

    //They did this one this way because the sprites for the wall prefabs were made to be laid on top of the sprites for the floor prefabs.
    //Also because in this game, the player is able to destroy the walls, so you need a floor under there still.

    void InstantiateTiles()
    {
        // Go through all the tiles in the jagged array...
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                //If tile is floor..
                if (tiles[i][j] == TileType.Floor)
                {

                    //floorTileCount++;

                    // ...instantiate a floor tile for it.
                    InstantiateFromArray(floorTiles, i, j, boardHolder);

                }
                // If the tile type is Wall...                  //I think the TileType enum defauls to be Wall(0) unless you specify otherwise, since it was declared first in enum
                if (tiles[i][j] == TileType.Wall)               //So, you could also say if (tiles[i][j] != TileType.Floor)...
                {
                    // ... instantiate a wall.
                    InstantiateFromArray(wallTiles, i, j, boardHolder);
                }
            }
        }
    }


    void InstantiateOuterWalls()
    {
        // The outer walls are one unit left, right, up and down from the board.
        float leftEdgeX = -1f;
        float rightEdgeX = columns + 0f;
        float bottomEdgeY = -1f;
        float topEdgeY = rows + 0f;

        // Instantiate both vertical walls (one on each side).
        InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
        InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

        // Instantiate both horizontal walls, these are one in left and right from the outer walls.
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
        InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
    }


    void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
    {
        // Start the loop at the starting value for Y.
        float currentY = startingY;

        // While the value for Y is less than the end value...
        while (currentY <= endingY)
        {
            // ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
            InstantiateFromArray(outerWallTiles, xCoord, currentY, boardHolder);

            currentY++;
        }
    }


    void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
    {
        // Start the loop at the starting value for X.
        float currentX = startingX;

        // While the value for X is less than the end value...
        while (currentX <= endingX)
        {
            // ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
            InstantiateFromArray(outerWallTiles, currentX, yCoord, boardHolder);

            currentX++;
        }
    }


    void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord, GameObject parent)
    {

        // Create a random index for the array.
        int randomIndex = Random.Range(0, prefabs.Length);

        // The position to be instantiated at is based on the coordinates.
        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        // Create an instance of the prefab from the random index of the array.
        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        // Set the tile's parent to the board holder.
        tileInstance.transform.parent = parent.transform;
    }



    public Node[][] GetNodes()
    {
        return nodes;
    }
}
