using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FieldControl : MonoBehaviour
{
	public GameObject fieldCell;
	public Material cross1;
	public Material circle1;
	
	public static int sideLength = 3;
	public static int reqToWin = 3;
	
	public static int crossWonSides = 0;
	public static int circleWonSides = 0;
	private static int freeSides = 6;
	
	public static GameObject[,,] cells;
	private static Quaternion[] cubeSideRotations = {Quaternion.Euler(-90,0,0), Quaternion.Euler(-180,0,0), Quaternion.Euler(0,0,-90), Quaternion.Euler(0,0,90), Quaternion.Euler(0,0,0), Quaternion.Euler(90,0,0)}; //суммы номеров всех противоположных граней равны 5
	private static GameObject fieldCell1;
	public static bool ai = true;
    // Start is called before the first frame update
    void Start()
    {
		crossWonSides = 0;
		circleWonSides = 0;
		freeSides = 6;
		InfoChange.setCircleInfo();
		InfoChange.setCrossInfo();
		
		cells = new GameObject[6, sideLength, sideLength];
		cross = cross1;
		circle = circle1;
		int k = sideLength/2;
		
		fieldCell1 = fieldCell;
		//for(int i=0; i<6; i++) repaintSide(i); 
        //for(int i=0;i<sideLength;i++)     Quaternion.Euler(90*(i&1),0,90*(i&2)/2)
			for(int j=0;j<(sideLength*sideLength);j++)
			{
				for(int i=0; i<6; i++) setCell (i, j, defineVector(i,j,k), cubeSideRotations[i]);
				/*setCell (0, j, new Vector3(j%sideLength-k,j/sideLength-k,-k-0.5f), cubeSideRotations[0]);
				setCell (5, j, new Vector3(j%sideLength-k,j/sideLength-k,k+0.5f), cubeSideRotations[5]);
				
				setCell (1, j, new Vector3(j%sideLength-k,-k-0.5f,j/sideLength-k), cubeSideRotations[1]);
				setCell (4, j, new Vector3(j%sideLength-k,k+0.5f,j/sideLength-k), cubeSideRotations[4]);
				
				setCell (2, j, new Vector3(k+0.5f,j%sideLength-k,j/sideLength-k), cubeSideRotations[2]);
				setCell (3, j, new Vector3(-k-0.5f,j%sideLength-k,j/sideLength-k), cubeSideRotations[3]);/**/
			}
				
    }
	
	private Vector3 defineVector(int i, int j, int k){
		float x = (i&6)==2 ? (k+0.5f)*(5-2*i) : j%sideLength-k;
		float y = i%3==1 ? (k+0.5f)*(i/2-1) : ((i&6)==2 ? j%sideLength-k : j/sideLength-k);
		float z = i%5==0 ? (k+0.5f)*(i/2-1) : j/sideLength-k;
		return new Vector3(x,y,z);
	}
	
	private static Vector3 defineVector(int i, int k){
		float x = (i&6)==2 ? (k+0.5001f)*(5-2*i) : 0;
		float y = i%3==1 ? (k+0.5001f)*(i/2-1) : 0;
		float z = i%5==0 ? (k+0.5001f)*(i/2-1) : 0;
		return new Vector3(x,y,z);
	}
	
	private void setCell(int cubeSide, int cellNumber, Vector3 vector, Quaternion quat){
		GameObject cell; CellClick ck;
		cell = Instantiate (fieldCell, vector, quat);
		ck = cell.GetComponent<CellClick>();
		ck.cubeSide = cubeSide;
		ck.cellNumber = cellNumber;
		cells[cubeSide, cellNumber/sideLength, cellNumber%sideLength] = cell;
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public static Material cross;
	public static Material circle;
	public static bool crossTakt = false;
	
	public static void cellClicked(GameObject cell){
		Debug.Log("Cell clicked");
		CellClick ck = cell.GetComponent<CellClick>();
		repaintCell(cell, ck);
		AIController.cubeSidesStatus[AIController.currentSide]=1; 
		AIController.currentSide = ck.cubeSide;
		AIController.cubeSidesStatus[ck.cubeSide]=2;
		paramsChange(ck);
		
		//ответ ИИ
		if(ai){
			int[] aiAnswer = AIController.aiAnswer();
			GameObject aiCell = cells[aiAnswer[0], aiAnswer[1], aiAnswer[2]];
			Debug.Log(""+aiAnswer[0]+" "+aiAnswer[1]+" "+aiAnswer[2]);
			CellClick aiCk = aiCell.GetComponent<CellClick>();
			repaintCell(aiCell, aiCk);
			paramsChange(aiCk);
			Debug.Log("AI answered");
		}
	}
	
	private static void repaintCell(GameObject cell, CellClick ck){
		if(crossTakt) {
			cell.GetComponent<Renderer>().material = cross;
			ck.type = 1;
		}
		else {
			cell.GetComponent<Renderer>().material = circle;
			ck.type = -1;
		}
	}
	
	private static void paramsChange(CellClick ck){
		
		if(lineFilled(ck.cubeSide, ck.cellNumber/sideLength, ck.cellNumber%sideLength, ck.type)) {
			AIController.cubeSidesStatus[ck.cubeSide]=-1;
			repaintSide(ck.cubeSide);
			if(crossTakt) {
				crossWonSides++;
				InfoChange.setCrossInfo();
			}
			else {
				circleWonSides++;
				InfoChange.setCircleInfo();
			}
			freeSides--;
		} else if(defineDrawOnSide(ck.cubeSide)) {
			AIController.cubeSidesStatus[ck.cubeSide]=-1;
			freeSides--;
		}
		if(freeSides==0)
			SceneManager.LoadScene(3); //gameOver
		crossTakt = !crossTakt;
	}
	
	private static void gameOver(){
		SceneManager.LoadScene(3);
	}
	
	public static bool lineFilled(int cubeSide, int cellX, int cellY, int type){
		bool res = true;
		for(int i = cellX-reqToWin; i<=cellX; i++){ //перебор горизонтали
			if(i>=0 && i<sideLength && cells[cubeSide, i, cellY].GetComponent<CellClick>().type==type){
				for(int j=i+1; j<i+reqToWin; j++)
					if(j>=sideLength || cells[cubeSide, j, cellY].GetComponent<CellClick>().type!=type) {
						res = false;
						break;
					}
				if(res) return res;
				else res = true;
			} else continue;
		}
		
		for(int i = cellY-reqToWin; i<=cellY; i++){ //перебор вертикали
			if(i>=0 && i<sideLength && cells[cubeSide, cellX, i].GetComponent<CellClick>().type==type){
				for(int j=i+1; j<i+reqToWin; j++)
					if(j>=sideLength || cells[cubeSide, cellX, j].GetComponent<CellClick>().type!=type) {
						res = false;
						break;
					}
				if(res) return res;
				else res = true;
			} else continue;
		}
		
		int i2 = cellX-reqToWin;
		for(int i = cellY-reqToWin; i<=cellY; i++){ //перебор главной диагонали
			if(i>=0 && i<sideLength && i2>=0 && i2<sideLength && cells[cubeSide, i2, i].GetComponent<CellClick>().type==type){
				int j2 = i2+1;
				for(int j=i+1; j<i+reqToWin; j++){
					if(j>=sideLength || j2>=sideLength || cells[cubeSide, j2, j].GetComponent<CellClick>().type!=type) {
						res = false;
						break;
					}
					j2++;
				}
				if(res) return res;
				else res = true;
			}
			i2++;
		}
		
		i2 = cellX+reqToWin;
		for(int i = cellY-reqToWin; i<=cellY; i++){ //перебор побочной диагонали
			if(i>=0 && i<sideLength && i2>=0 && i2<sideLength && cells[cubeSide, i2, i].GetComponent<CellClick>().type==type){
				int j2 = i2-1;
				for(int j=i+1; j<i+reqToWin; j++){
					if(j>=sideLength || j2<0 || cells[cubeSide, j2, j].GetComponent<CellClick>().type!=type) {
						res = false;
						break;
					}
					j2--;
				}
				if(res) return res;
				else res = true;
			}
			i2--;
		}
		
		return false;
	}
	
	private static void repaintSide(int cubeSide){
		GameObject side;
		side = Instantiate (fieldCell1,defineVector(cubeSide, sideLength/2), cubeSideRotations[cubeSide]);
		side.transform.localScale = new Vector3(0.1f*sideLength, 1, 0.1f*sideLength);
		Material mat;
		if(crossTakt) mat = cross;
		 else mat = circle;
		side.GetComponent<Renderer>().material = mat;
		for(int i=0; i<sideLength; i++)
			for(int j=0; j<sideLength; j++)
				cells[cubeSide, i, j].GetComponent<CellClick>().type = crossTakt ? 1 : -1;
	}
	
	private static bool defineDrawOnSide(int cubeSide){
		for(int i=0; i<sideLength; i++)
			for(int j=0; j<sideLength; j++)
				if(cells[cubeSide, i, j].GetComponent<CellClick>().type == 0) 
					return false;
		return true;
	}
}
