using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Battle;
using System;

public class TestMath : MonoBehaviour {


    public GameObject cube1;
    public GameObject cube2;
	// Use this for initialization


    float start;
    BattleAgent b;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        
	}
    
    private Battle.Vector2 getAtackingPoint(Battle.Vector2 center, Battle.Vector2 target)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3 (center.x_, 0, center.y_);

        cube = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        cube.transform.position = new Vector3 (target.x_, 0, target.y_);

//        float startAngle = Battle.Vector2.Angle(Battle.Vector2.right, target - center);
//        Debug.Log(startAngle);
        
        float startAngle = Quaternion.LookRotation( new Vector3(target.x_, 0, target.y_) - new Vector3(center.x_, 0, center.y_), Vector3.up).eulerAngles.y;
        int clockStep = 12;
        double startDegree = Math.PI * startAngle / 180;
        double degreeStep = Math.PI * 2 / clockStep;
        
        int clockForward = 1;//Random.value > 0.5 ? 1 : -1;
        

        float outRange = 10;

        Battle.Vector2 tmpVector;
        for (int i = 0; i < 5; i++)
        {
            double degree = startDegree + clockForward * degreeStep * i;
            tmpVector.x_ = (float)(outRange * Math.Sin(degree));
            tmpVector.y_ = (float)(outRange * Math.Cos(degree));
            Battle.Vector2 testPoint = center + tmpVector;
            
            var cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube1.transform.position = new Vector3 (testPoint.x_, 0, testPoint.y_);
        }
        
        return Battle.Vector2.zero;
    }
}
