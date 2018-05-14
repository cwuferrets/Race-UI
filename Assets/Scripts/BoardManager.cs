using UnityEngine;
using System;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour {

    private Transform boardHolder;
    public List<GameObject> racersTemplate;
    public List<GameObject> racers;
    public GameObject mountain;
    public List<GameObject> carrots;
    public List<GameObject> goals;
    public GameObject floorTile;
    private GameObject spawn;
    private System.Random r = new System.Random();
    private List<List<string>> mapData = new List<List<string>>();
    private GameObject winner = null;
    public GameObject winnerText;
    public int turnSpeed;

    void BoardSetup()
    {
        Destroy(boardHolder);
        boardHolder = new GameObject("Board").transform;
        boardHolder.transform.SetParent(this.transform);
        SetTable();
        SetObjects();

    }

    private void SetTable()
    {
        List<String> row;
        for (int i = 0; i < 5; i++)
        {
            row = new List<String>();
            mapData.Add(row);
            for (int j = 0; j < 5; j++)
            {
                mapData[i].Add("");
                floorTile.GetComponent<Token>().setColumn(i);
                floorTile.GetComponent<Token>().setRow(j);
                spawn = Instantiate(floorTile, new Vector3((floorTile.GetComponent<Token>().getColumn() - 2) * 2, (floorTile.GetComponent<Token>().getRow() - 2) * 2, 0f), Quaternion.identity) as GameObject;
                spawn.transform.SetParent(boardHolder);
            }
        }
    }

    private void SetObjects()
    {
        int column;
        int row;
        for (int i = 0; i < racersTemplate.Count; i++)
        {
            row = r.Next(0, 5);
            column = r.Next(0, 5);
            while (mapData[row][column] != "")
            {
                row = r.Next(0, 5);
                column = r.Next(0, 5);
            }
            racersTemplate[i].GetComponent<Racer>().setColumn(column);
            racersTemplate[i].GetComponent<Racer>().setRow(row);
            initalize(racersTemplate[i]);
            mapData[row][column] = racersTemplate[i].name;
        }
        for (int i = 0; i < goals.Count; i++)
        {
            row = r.Next(0, 5);
            column = r.Next(0, 5);
            while (mapData[row][column] != "")
            {
                row = r.Next(0, 5);
                column = r.Next(0, 5);
            }
            goals[i].GetComponent<Token>().setColumn(column);
            goals[i].GetComponent<Token>().setRow(row);
            initalize(goals[i]);
            mapData[row][column] = goals[i].name;
            
        }
    }

    private void teleportMountain()
    {
        int column = r.Next(0, 5);
        int row = r.Next(0, 5);
        while (mapData[row][column] != "")
        {
            row = r.Next(0, 5);
            column = r.Next(0, 5);
        }
        mapData[row][column] = "F";
        row = row - 2;
        column = column - 2;
        mapData[(int)(mountain.transform.position.y / 2) + 2][(int)(mountain.transform.position.x / 2) + 2] = "";
        mountain.transform.position = new Vector3((column * 2), (row * 2), 0);
    }

    private void initalize(GameObject token)
    {
        spawn = Instantiate(token, new Vector3((token.GetComponent<Token>().getColumn() - 2) * 2, (token.GetComponent<Token>().getRow() - 2) * 2, 0f), Quaternion.identity) as GameObject;
        spawn.transform.SetParent(boardHolder);
        if(token.name == "C")
        {
            carrots.Add(spawn);
        }
        else if(token.name == "F")
        {
            mountain = spawn;
        }
        else
        {
            racers.Add(spawn);
        }
    }

    public bool checkMove(GameObject token, int row, int column)
    {
        if (((int)token.transform.position.x + (column * 2) > 4 || (int)token.transform.position.x + (column * 2) < -4))
        {
            return true;
        }
        if (((int)token.transform.position.y + (row * 2) > 4 || (int)token.transform.position.y + (row * 2) < -4))
        {
            return true;
        }
        if (token.GetComponent<Racer>().doYouHaveCarrot())
        {
            if (token.name.StartsWith("M"))
            {
                return false;
            }
            if (mapData[(((int)token.transform.position.y / 2) + row + 2)][(((int)token.transform.position.x / 2) + column + 2)] != "" && mapData[(((int)token.transform.position.y / 2) + row + 2)][(((int)token.transform.position.x / 2) + column + 2)] != "C" && mapData[(((int)token.transform.position.y / 2) + row + 2)][(((int)token.transform.position.x / 2) + column + 2)] != "F")
            {
                return true;
            }
        }
        else
        {
            if (token.name.StartsWith("M"))
            {
                if (mapData[(((int)token.transform.position.y / 2) + row + 2)][(((int)token.transform.position.x / 2) + column + 2)] == "F")
                {
                    return true;
                }
            }
            else
            {
                if (mapData[(((int)token.transform.position.y / 2) + row + 2)][(((int)token.transform.position.x / 2) + column + 2)] != "" && mapData[(((int)token.transform.position.y / 2) + row + 2)][(((int)token.transform.position.x / 2) + column + 2)] != "C")
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void makeMove(GameObject token)
    {
        string name = "";
        name = name + token.name[0];
        if(mapData[(((int)token.transform.position.y / 2) + token.GetComponent<Racer>().row + 2)][(((int)token.transform.position.x / 2) + token.GetComponent<Racer>().column + 2)] != "")
        {
            if (mapData[(((int)token.transform.position.y / 2) + token.GetComponent<Racer>().row + 2)][(((int)token.transform.position.x / 2) + token.GetComponent<Racer>().column + 2)] == "F")
            {
                weHaveAWinner(token);
            }
        }
        mapData[(((int)token.transform.position.y / 2) + 2)][(((int)token.transform.position.x / 2) + 2)] = "";
        mapData[(((int)token.transform.position.y / 2) + token.GetComponent<Racer>().row + 2)][(((int)token.transform.position.x / 2) + token.GetComponent<Racer>().column + 2)] = name;
        token.transform.Translate(new Vector3(token.GetComponent<Racer>().getColumn() * 2, token.GetComponent<Racer>().getRow() * 2, 0));
        for (int i = 0; i < carrots.Count; i++)
        {
            if (carrots[i].transform.position == token.transform.position)
            {
                token.GetComponent<Racer>().hasCarrot = true;
                Destroy(carrots[i]);
                carrots.Remove(carrots[i]);
            }
        }
        if (token.name.StartsWith("M"))
        {
            for(int i = 0; i < racers.Count; i++)
            {
                if (racers[i].transform.position == token.transform.position && racers[i].name != token.name)
                {
                    if (racers[i].GetComponent<Racer>().hasCarrot)
                    {
                        token.GetComponent<Racer>().hasCarrot = true;
                    }
                    Destroy(racers[i]);
                    racers.Remove(racers[i]);
                }
            }
        }
    }

    private void weHaveAWinner(GameObject token)
    {
        winner = token;
        if (winner.name.StartsWith("M"))
        {
            winnerText.GetComponent<UnityEngine.UI.Text>().text = "Marvin is the winner";
        }
        else if (winner.name.StartsWith("T"))
        {
            winnerText.GetComponent<UnityEngine.UI.Text>().text = "Tweety is the winner";
        }
        else if (winner.name.StartsWith("B"))
        {
            winnerText.GetComponent<UnityEngine.UI.Text>().text = "B. Bunny is the winner";
        }
        else if (winner.name.StartsWith("D"))
        {
            winnerText.GetComponent<UnityEngine.UI.Text>().text = "T. Devil is the winner";
        }
    }

    public void SetupScene()
    {
        //startingGameBoard();
        BoardSetup();
    }

    void Start () {

    }
    private int frame = 0;
    private int racerIndex = 0;
    private int turnCycle = 0;
    // Update is called once per frame
    void Update () {
        if (winner == null)
        {
            if (frame < turnSpeed)
            {
                frame++;
            }
            else
            {
                if (racerIndex < racers.Count)
                {
                    if (racers[racerIndex].GetComponent<Racer>().readyToMove && !racers[racerIndex].GetComponent<Racer>().hasMoved)
                    {
                        if (!checkMove(racers[racerIndex], racers[racerIndex].GetComponent<Racer>().getRow(), racers[racerIndex].GetComponent<Racer>().getColumn()))
                        {
                            racers[racerIndex].GetComponent<Racer>().hasMoved = true;
                            racers[racerIndex].GetComponent<Racer>().readyToMove = false;
                            makeMove(racers[racerIndex]);
                            frame = 0;
                        }
                        else
                        {
                            racers[racerIndex].GetComponent<Racer>().readyToMove = false;
                            racerIndex--;
                        }
                    }
                    racerIndex++;
                }
                else
                {
                    for (int i = 0; i < racers.Count; i++)
                    {
                        racers[i].GetComponent<Racer>().hasMoved = false;
                    }
                    racerIndex = 0;
                    turnCycle++;
                    if (turnCycle > 2)
                    {
                        teleportMountain();
                        turnCycle = 0;
                    }

                    frame = 0;
                }
            }
        }
    }
}
