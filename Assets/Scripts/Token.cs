using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
    }
    public int column;
    public int row;
    public string toString()
    {
        return this.name;
    }
    public int getColumn()
    {
        return column;
    }
    public int getRow()
    {
        return row;
    }
    public void setColumn(int x)
    {
        column = x;
    }
    public void setRow(int y)
    {
        row = y;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
