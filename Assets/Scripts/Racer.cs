using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Racer : Token {
    public bool hasCarrot;
    public bool readyToMove;
    public bool hasMoved;
    private System.Random r = new System.Random();

    // Use this for initialization
    void Start () {
        hasCarrot = false;
        readyToMove = false;
        hasMoved = false;
        column = 0;
        row = 0;
    }
	// Update is called once per frame
	void Update () {
        if (!readyToMove)
        {
            row = r.Next(0, 3) - 1;
            column = r.Next(0, 3) - 1;
            while (GetComponentInParent<Transform>().GetComponentInParent<BoardManager>().checkMove(this.gameObject, row, column))
            {
                row = r.Next(0, 3) - 1;
                column = r.Next(0, 3) - 1;
            }
            readyToMove = true;
        }
	}

    public bool doYouHaveCarrot()
    {
        return hasCarrot;
    }
    public void updateCarrot()
    {
        hasCarrot = true;
    }
}
