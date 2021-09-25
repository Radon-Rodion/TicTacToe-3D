using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoChange : MonoBehaviour
{
	private static GameObject circleInfo;
	private static GameObject crossInfo;
	
	public GameObject circleInfo1;
	public GameObject crossInfo1;
    // Start is called before the first frame update
    void Start()
    {
        circleInfo = circleInfo1;
		crossInfo = crossInfo1;
		
		setCircleInfo();
		setCrossInfo();
    }
	
	public static void setCircleInfo(){
		circleInfo.GetComponent<Text>().text=""+FieldControl.circleWonSides;
	}
	
	public static void setCrossInfo(){
		crossInfo.GetComponent<Text>().text=""+FieldControl.crossWonSides;
	}
}
