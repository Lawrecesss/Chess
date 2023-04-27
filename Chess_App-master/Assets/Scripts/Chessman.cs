using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    //References to objects in our Unity Scene
    public GameObject controller;
    public GameObject movePlate;

    //Position for this Chesspiece on the Board
    //The correct position will be set later
    private int xBoard = -1;
    private int yBoard = -1;

    private bool check = false;

    //Variable for keeping track of the player it belongs to "black" or "white"
    private string player;

    //References to all the possible Sprites that this Chesspiece could be
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    public void Activate()
    {
        //Get the game controller
        controller = GameObject.FindGameObjectWithTag("GameController");

        //Take the instantiated location and adjust transform
        SetCoords();

        //Choose correct sprite based on piece's name
        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        //Get the board value in order to convert to xy coords
        float x = xBoard;
        float y = yBoard;

        //Adjust by variable offset
        x *= 0.66f;
        y *= 0.66f;

        //Add constants (pos 0,0)
        x += -2.3f;
        y += -2.3f;

        //Set actual unity values
        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            //Remove all moveplates relating to previously selected piece
            DestroyMovePlates();

            //Create new MovePlates
            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        //Destroy old MovePlates
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]); //Be careful with this function "Destroy" it is asynchronous
        }
    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                if(yBoard == 6)
                {
                    PawnMovePlate(xBoard, yBoard, 1);
                    PawnMovePlate(xBoard, yBoard, 2);
                }
                else
                {
                    PawnMovePlate(xBoard, yBoard, 1);
                }
                break;
            case "white_pawn":
                if (yBoard == 1)
                {
                    PawnMovePlate(xBoard, yBoard, 1);
                    PawnMovePlate(xBoard, yBoard, 2);
                }
                else
                {
                    PawnMovePlate(xBoard, yBoard, 1);
                }
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 0);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard + 0);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }
    
    public void PawnMovePlate(int x, int y, int coord)
    {
        int yPosition = 0;
        Game sc = controller.GetComponent<Game>();
        switch (this.player)
        {
            case "black":
                yPosition = y - coord;
                break;
            case "white":
                yPosition = y + coord;
                break;
        }
        if (sc.PositionOnBoard(x, yPosition))
        {
            if (sc.GetPosition(x, yPosition) == null)
            {
                MovePlateSpawn(x, yPosition);
                
            }

            if (sc.PositionOnBoard(x + 1, yPosition) && sc.GetPosition(x + 1, yPosition) != null && sc.GetPosition(x + 1, yPosition).GetComponent<Chessman>().player != player && coord != 2)
            {
                MovePlateAttackSpawn(x + 1, yPosition);
                
            }

            if (sc.PositionOnBoard(x - 1, yPosition) && sc.GetPosition(x - 1, yPosition) != null && sc.GetPosition(x - 1, yPosition).GetComponent<Chessman>().player != player && coord != 2)
            {
                MovePlateAttackSpawn(x - 1, yPosition);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Adjust by variable offset
        x *= 0.66f;
        y *= 0.66f;

        //Add constants (pos 0,0)
        x += -2.3f;
        y += -2.3f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        //Get the board value in order to convert to xy coords
        float x = matrixX;
        float y = matrixY;

        //Adjust by variable offset
        x *= 0.66f;
        y *= 0.66f;

        //Add constants (pos 0,0)
        x += -2.3f;
        y += -2.3f;

        //Set actual unity values
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
    //public void  checkKing(int x, int y)
    //{
    //    checkSurround(x, y);
    //    checkLMove(x, y);
    //    CheckLineOrDiagonal(x, y);
    //    ChangeColorWhenCheck(x, y);
    //}
    //public void checkPointPlate(int x, int y)
    //{
    //    Game sc = controller.GetComponent<Game>();
    //    if (sc.PositionOnBoard(x, y))
    //    {
    //        GameObject cp = sc.GetPosition(x, y);

    //        if (cp != null && cp.GetComponent<Chessman>().player != player)
    //        {
    //            check = true;
    //        }
    //    }
    //}
    //public void checkSurround(int x, int y)
    //{
    //    checkPointPlate(x, y + 1);
    //    checkPointPlate(x, y - 1);
    //    checkPointPlate(x - 1, y + 0);
    //    checkPointPlate(x - 1, y - 1);
    //    checkPointPlate(x - 1, y + 1);
    //    checkPointPlate(x + 1, y + 0);
    //    checkPointPlate(x + 1, y - 1);
    //    checkPointPlate(x + 1, y + 1);
    //}
    //public void checkLMove(int x, int y)
    //{
    //    checkPointPlate(x + 1, y + 2);
    //    checkPointPlate(x - 1, y + 2);
    //    checkPointPlate(x + 2, y + 1);
    //    checkPointPlate(x + 2, y - 1);
    //    checkPointPlate(x + 1, y - 2);
    //    checkPointPlate(x - 1, y - 2);
    //    checkPointPlate(x - 2, y + 1);
    //    checkPointPlate(x - 2, y - 1);
    //}
    //public void LineMove(int xIncrement, int yIncrement)
    //{
    //    Game sc = controller.GetComponent<Game>();

    //    int x = xBoard + xIncrement;
    //    int y = yBoard + yIncrement;

    //    while (sc.PositionOnBoard(x, y))
    //    {
    //        if (sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
    //        {
    //            switch (this.player)
    //            {
    //                case "white":
    //                    if(sc.GetPosition(x, y).GetComponent<Chessman>().name == "black_bishop" ||
    //                        sc.GetPosition(x, y).GetComponent<Chessman>().name == "black_queen")
    //                    {
    //                        check = true;
    //                    }
    //                    break;
    //                case "black":
    //                    if (sc.GetPosition(x, y).GetComponent<Chessman>().name == "white_bishop" ||
    //                        sc.GetPosition(x, y).GetComponent<Chessman>().name == "white_queen")
    //                    {
    //                        check = true;
    //                    }
    //                    break;
    //            }

    //        }
    //        x += xIncrement;
    //        y += yIncrement;
    //    }
    //}
    //public void CheckLineOrDiagonal(int x, int y)
    //{
    //    LineMove(1, 0);
    //    LineMove(0, 1);
    //    LineMove(1, 1);
    //    LineMove(-1, 0);
    //    LineMove(0, -1);
    //    LineMove(-1, -1);
    //    LineMove(-1, 1);
    //    LineMove(1, -1);
    //}
    //public void ChangeColorWhenCheck(int x, int y)
    //{
    //    Game sc = controller.GetComponent<Game>();
    //    if (check == true)
    //    {
    //        sc.GetPosition(x, y).GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    //    }
    //}
    ////

}
