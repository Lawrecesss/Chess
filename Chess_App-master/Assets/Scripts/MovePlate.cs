using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePlate : MonoBehaviour
{
    //Some functions will need reference to the controller
    public GameObject controller;

    //The Chesspiece that was tapped to create this MovePlate
    GameObject reference = null;
    private bool printCheckBlack = false;
    private bool printCheckWhite = false;

    //Location on the board
    int matrixX;
    int matrixY;

    //false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //Set to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        removePrint();
        

        //Destroy the victim Chesspiece
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
            if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

            Destroy(cp);
        }

        //Set the Chesspiece's original location to be empty
        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().GetXBoard(),
            reference.GetComponent<Chessman>().GetYBoard());

        //Move reference chess piece to this position
        reference.GetComponent<Chessman>().SetXBoard(matrixX);
        reference.GetComponent<Chessman>().SetYBoard(matrixY);
        reference.GetComponent<Chessman>().SetCoords();

        //Update the matrix
        controller.GetComponent<Game>().SetPosition(reference);
        PawnPromotion();


        //Switch Current Player
        controller.GetComponent<Game>().NextTurn();
        

        //Destroy the move plates including self
        reference.GetComponent<Chessman>().DestroyMovePlates();
        Check();

    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

    public void Check()
    {
        switch (reference.name)
        {
            case "white_bishop":
            case "black_bishop":
                LineMove(1, 1);
                LineMove(1, -1);
                LineMove(-1, 1);
                LineMove(-1, -1);
                break;
            case "white_queen":
            case "black_queen":
                LineMove(1, 0);
                LineMove(0, 1);
                LineMove(1, 1);
                LineMove(-1, 0);
                LineMove(0, -1);
                LineMove(-1, -1);
                LineMove(-1, 1);
                LineMove(1, -1);
                break;
            case "white_rook":
            case "black_rook":
                LineMove(1, 0);
                LineMove(0, 1);
                LineMove(-1, 0);
                LineMove(0, -1);
                break;
            case "white_knght":
            case "black_knight":
                LMove();
                break;
            case "white_pawn":
            case "black_pawn":
                LineMove(1, 1);
                LineMove(1, -1);
                break;
        }
    }
    public void LineMove(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = matrixX + xIncrement;
        int y = matrixY + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            x += xIncrement;
            y += yIncrement;
        }
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<Chessman>().player != reference.GetComponent<Chessman>().player &&
            sc.GetPosition(x, y).GetComponent<Chessman>().name == "black_king")
        {
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "Black king is checked";
            printCheckBlack = true;
            
        }
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<Chessman>().player != reference.GetComponent<Chessman>().player &&
            sc.GetPosition(x, y).GetComponent<Chessman>().name == "white_king")
        {
            GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().text = "White king is checked";
            printCheckWhite = true;
        }
    }
    public void PointMove(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {

            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<Chessman>().player != reference.GetComponent<Chessman>().player &&
            sc.GetPosition(x, y).GetComponent<Chessman>().name == "black_king")
            {
                GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
                GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = "Black king is checked";
                printCheckBlack = true;

            }
            if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) != null && sc.GetPosition(x, y).GetComponent<Chessman>().player != reference.GetComponent<Chessman>().player &&
                sc.GetPosition(x, y).GetComponent<Chessman>().name == "white_king")
            {
                GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
                GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().text = "White king is checked";
                printCheckBlack = true;

            }
        }
    }
    public void LMove()
    {
        PointMove(matrixX + 1, matrixY + 2);
        PointMove(matrixX - 1, matrixY + 2);
        PointMove(matrixX + 2, matrixY + 1);
        PointMove(matrixX + 2, matrixY - 1);
        PointMove(matrixX + 1, matrixY - 2);
        PointMove(matrixX - 1, matrixY - 2);
        PointMove(matrixX - 2, matrixY + 1);
        PointMove(matrixX - 2, matrixY - 1);
    }
    public void removePrint()
    {
        if (printCheckBlack)
        {
            GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = false;
        }
        if (printCheckWhite)
        {
            GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = false;
        }

    }
    public void PawnPromotion()
    {
        if (reference.name == "white_pawn" && matrixY == 7)
        {
            reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().white_bishop;
            reference.name = "white_bishop";
        }
        if (reference.name == "black_pawn" && matrixY == 0)
        {
            reference.GetComponent<SpriteRenderer>().sprite = reference.GetComponent<Chessman>().black_bishop;
            reference.name = "black_bishop";
        }

    }
}
