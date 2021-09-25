using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static int[] cubeSidesStatus = {0,0,0,0,0,0};
	public static int currentSide = 0; //prevSide - ?
	public static int maxDepth = 4;
	
	public static void defineCurrent(){
		if(cubeSidesStatus[currentSide]==-1){
			for(int i=0; i<6; i++){
				if(cubeSidesStatus[i]==1){
					currentSide = i;
					return;
				}
			}
			
			for(int i=0; i<6; i++){
				if(cubeSidesStatus[i]==0){
					currentSide = i;
					return;
				}
			}
			
			//свободных мест нет - конец игры
			currentSide = -1;
		}
	}
	
	public static int[] aiAnswer(){
		int[] res = {0,0,0};
		defineCurrent();
		int[] marks = markPosition(FieldControl.crossTakt, 0);
		string s = "";
		for(int i =0; i<marks.Length; i++)
			s+=marks[i]+" ";
		Debug.Log(s);
		int[] bestSteps = largestIndexes(marks);
		int bestStepsAmount = bestSteps[bestSteps.Length-1];
		int stepIndex = -1;
		while(stepIndex == -1)
			stepIndex = bestSteps[Random.Range(0,bestStepsAmount)];
		
		res[0] = currentSide;
		res[1] = stepIndex/FieldControl.sideLength;
		res[2] = stepIndex%FieldControl.sideLength;
		
		return res;
	}
	
	public static int[] markPosition(bool crossTakt, int depth){//рекурсивная оценка всех ходов в позиции
		int type = crossTakt ? 1 : -1;
		int[] res = {0,0,0, 0,0,0, 0,0,0}; //оценки каждого хода
		if(depth >= maxDepth) 
			return res;
		for(int i=0; i<FieldControl.sideLength*FieldControl.sideLength; i++){
			if(FieldControl.cells[currentSide, i/FieldControl.sideLength, i%FieldControl.sideLength].GetComponent<CellClick>().type == 0){
				FieldControl.cells[currentSide, i/FieldControl.sideLength, i%FieldControl.sideLength].GetComponent<CellClick>().type = type;
				if(FieldControl.lineFilled(currentSide, i/FieldControl.sideLength, i%FieldControl.sideLength, type)) {
					res[i] = type*100;
					FieldControl.cells[currentSide, i/FieldControl.sideLength, i%FieldControl.sideLength].GetComponent<CellClick>().type = 0;
					continue;
				}
				res[i] = averageMark(markPosition(!crossTakt, depth+1));
				FieldControl.cells[currentSide, i/FieldControl.sideLength, i%FieldControl.sideLength].GetComponent<CellClick>().type = 0;
			} else res[i]=type*(-100000);
		}
		return res;
	}
	
	private static int averageMark(int[] stepMarks){//оценка позиции по итогам всех возможных ходов
		int res = 0; int nMarks = 0;
		foreach (int mark in stepMarks){
			if(mark!=-100000 && mark!= 100000){
				res+= mark;
				nMarks++;
			}
		}
		if(nMarks==0)
			return -10000;
		else res /= nMarks;
		return res;
	}
	
	private static int[] largestIndexes(int[] array){//индексы всех наибольших элементов
		int maxValue = array[0];
		foreach(int val in array){
			if(maxValue < val)
				maxValue = val;
		}
		
		int[] res = new int[array.Length+1]; int j=0;
		for(int i=0; i<array.Length; i++)
			if(array[i]==maxValue){
				res[j]=i;
				j++;
			}
		res[array.Length]=j-1;
		for(;j<array.Length;j++)
			res[j]=-1;
		return res;
	}
}
