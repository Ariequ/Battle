using UnityEngine;
using System.Collections;
using RVO;
using System.IO;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Threading;

using Battle;

public class RvoAgent : MonoBehaviour
{
	private const int RAND_MAX = 0x7fff;

	[Range(3.0f, 8.0f)]
	public float commonSpeed = 5.0f;

	private Dictionary<string,RVO.Simulator> simulatorDic = new Dictionary<string, RVO.Simulator> ();
	private Dictionary<string,List<GameObject>> gameObjectsDic = new Dictionary<string,List<GameObject>> ();
	private Dictionary<string,List<RVO.Vector2>> goalsDic = new Dictionary<string, List<RVO.Vector2>> ();

	private List<Simulator> paddingSimulators = new List<Simulator>();
	private Dictionary<int, int> obstacleIndex = new Dictionary<int, int>();

	void Start ()
	{
		Loom.Initialize();

		ThreadPool.QueueUserWorkItem(DoStepAsync);
	}

	void Update ()
	{
		setPreferredVelocities ();
		
		foreach (KeyValuePair<string, RVO.Simulator> dic in simulatorDic)
		{
			Simulator simulator = dic.Value;
			simulator.UpdateSpeedStep(BattleSimulator.deltaTime);

			if (simulator.getNumAgents() > 0)
			{
				lock (paddingSimulators)
				{
					if (!paddingSimulators.Contains(simulator))
					{
						paddingSimulators.Add(simulator);
					}
				}
			}
		}
	}

	void OnDestroy ()
	{
		lock (paddingSimulators)
		{
			paddingSimulators = null;
		}
	}

	private void DoStepAsync (object o)
	{
		RVO.Simulator simulator;

		while (paddingSimulators != null)
		{
			lock (paddingSimulators)
			{
				if (paddingSimulators.Count > 0)
				{
					simulator = paddingSimulators[0];
					paddingSimulators.RemoveAt(0);
				}
				else
				{
					simulator = null;
				}
			}
			
			if (simulator != null)
			{
				Loom.RunAsync(simulator.doStep);
			}
			else
			{
				Thread.Sleep(5);
			}
		}

		Debug.Log("RVO Agnent Tread End");
	}

	public void addObstacle (int tombStoneID, Transform transform, string type)
	{
		Bounds bounds = transform.collider.bounds;

		float x = bounds.center.x + transform.position.x;
		float y = bounds.center.y + transform.position.y;
		float halfWidth = bounds.size.x * 0.5f;
		float halfHeight = bounds.size.z * 0.5f;

		IList<RVO.Vector2> vertices = new List<RVO.Vector2>();
		vertices.Add(new RVO.Vector2(x - halfWidth, y - halfHeight));
		vertices.Add(new RVO.Vector2(x + halfWidth, y - halfHeight));
		vertices.Add(new RVO.Vector2(x - halfWidth, y + halfHeight));
		vertices.Add(new RVO.Vector2(x + halfWidth, y + halfHeight));

		Simulator simulator = null;
		simulatorDic.TryGetValue(type, out simulator);

		int obstacleNo = simulator.addObstacle(vertices);
		obstacleIndex.Add(tombStoneID, obstacleNo);
	}

	public void removeObstacle (int tombStoneID, string type)
	{
		Simulator simulator = null;
		simulatorDic.TryGetValue(type, out simulator);

		int obstacleNo = obstacleIndex[tombStoneID];
		obstacleIndex.Remove(tombStoneID);
		simulator.removeObstacle(obstacleNo);
	}

	public Simulator addGameObject (GameObject gameObject, String identity, string type, float radius, RVO.Vector2 goal)
	{
		List<GameObject> gameObjects = null;
		gameObjectsDic.TryGetValue(type, out gameObjects);
		
		if (gameObjects == null)
		{
			gameObjects = new List<GameObject>();
			gameObjectsDic.Add(type, gameObjects);
		}
		
		Simulator simulator = null;
		simulatorDic.TryGetValue(type, out simulator);
		
//		if (simulator == null)
//		{
//			simulator = new RVO.Simulator();
//			simulator.name = type;
//			simulator.setTimeStep (0.25f);
//			/* Specify the default parameters for agents that are subsequently added. */
//			float defautSpeed = 100.0f / FPS * this.commonSpeed;
//			simulator.setAgentDefaults (15.0f, 10, 5.0f, 5.0f, 1f, defautSpeed, new RVO.Vector2 (),type);
//			simulator.SetNumWorkers (1);
//			simulatorDic.Add(type, simulator);
//		}

		if (simulator == null)
		{
			simulator = new RVO.Simulator();
			simulator.name = type;
			simulator.setCommonSpeed (this.commonSpeed);
			simulator.setAgentDefaults (15.0f, 10, 5.0f, 5.0f, 1.0f, this.commonSpeed, new RVO.Vector2 (), type);
			simulator.SetNumWorkers (1);
			simulatorDic.Add(type, simulator);
		}

		List<RVO.Vector2> goals = null;
		goalsDic.TryGetValue(type, out goals);
		
		if (goals == null)
		{
			goals = new List<RVO.Vector2>();
			goalsDic.Add(type, goals);
		}
		
		int index = gameObjects.IndexOf(gameObject);
		
		if (index == -1) 
		{
			simulator.addAgent (identity, new RVO.Vector2 (gameObject.transform.position.x, gameObject.transform.position.z), radius * 1.1f);
			goals.Add (goal);
			gameObjects.Add (gameObject);
		}
		else
		{
			goals[index] = goal;
		}

		return simulator;
	}

	public void modifyGoal (GameObject gameObject, String identity, string type, RVO.Vector2 goal)
	{
		List<GameObject> gameObjects = gameObjectsDic[type];
		int index = gameObjects.IndexOf(gameObject);

		if (index != -1)
		{
			goalsDic[type][index] = goal;
		}
	}

	private System.Random _random = new System.Random ();

	public void setPreferredVelocities ()
	{
		/* 
         * Set the preferred velocity to be a vector of unit magnitude (speed) in the
         * direction of the goal.
         */
		foreach (KeyValuePair<string, RVO.Simulator> dic in simulatorDic) 
		{
			Simulator simulator = dic.Value;
			for (int i = 0; i < simulator.getNumAgents(); ++i) {
				RVO.Vector2 goalVector = goalsDic [dic.Key] [i] - simulator.getAgentPosition (i);
					
				//				AIUtil.Log ("goal" + goals [i].ToString ());
					
				if (RVO.RVOMath.absSq (goalVector) > 1.0f) {
					goalVector = RVO.RVOMath.normalize (goalVector);
				}

				simulator.setAgentPrefVelocity (i, goalVector);
					
				/*
             * Perturb a little to avoid deadlocks due to perfect symmetry.
             */
				float angle = _random.Next (RAND_MAX) * 2.0f * (float)Math.PI / RAND_MAX;
				float dist = _random.Next (RAND_MAX) * 0.0001f / RAND_MAX;
                    
				simulator.setAgentPrefVelocity (i, simulator.getAgentPrefVelocity (i) +
				                                dist * new RVO.Vector2 ((float)Math.Cos (angle), (float)Math.Sin (angle)));
			}
		}
	}

	public void removeGameObject (GameObject gameObject, String identity, string type)
	{
		List<GameObject> gameObjects = gameObjectsDic[type];
		int targetIndex = gameObjects.IndexOf(gameObject);

		if (targetIndex != -1) {
			gameObjects.RemoveAt(targetIndex);
			goalsDic[type].RemoveAt(targetIndex);
			simulatorDic[type].removeAgent(identity, targetIndex);
		}
	}

	public void pauseMove (GameObject gameObject, string type)
	{
		int index = gameObjectsDic [type].IndexOf (gameObject);
		simulatorDic [type].pauseAgent (index);
	}

	public void resumeMove (GameObject gameObject, string type)
	{
		int index = gameObjectsDic [type].IndexOf (gameObject);
		simulatorDic [type].resumeAgent (index);
	}

	private Simulator getSimulator (string type)
	{
		Simulator simulator;
		simulatorDic.TryGetValue (type, out simulator);
		return simulator;
	}

	private List<GameObject> getGameObjects (string type)
	{
		List<GameObject> list;
		gameObjectsDic.TryGetValue (type, out list);
		return list;
	}

	private List<RVO.Vector2> getGoals (string type)
	{
		List<RVO.Vector2> goals;
		goalsDic.TryGetValue (type, out goals);
		return goals;
	}
}
