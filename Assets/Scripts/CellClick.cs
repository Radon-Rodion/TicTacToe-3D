using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellClick : MonoBehaviour
{
	//public FieldControl fieldControl;
	public int cubeSide;
	public int cellNumber;
	public int type = 0;  //0 - free, 1 - cross, -1 - circle
	
    void OnMouseDown(){
		if(type==0)
			Debug.Log(""+cubeSide+" "+cellNumber/3+" "+cellNumber%3);
			FieldControl.cellClicked(gameObject);
	}
}
