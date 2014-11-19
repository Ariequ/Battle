using UnityEngine;
using System.Collections;
using Battle;

public class TestBattleSimulator : MonoBehaviour
{
//    BattleSimulator simulator;

    void Start()
    {
//        Application.targetFrameRate = 2000;
//        simulator = new BattleSimulator();
//        BattleAgentManager manager = new BattleAgentManager(new Battle.GameMessageRoute(),new BattleValueCalculator());
//
//        for (int i = 0; i < 100; i++)
//        {
//            BattleAgent agent = new BattleAgent();
//            agent.position_ = new Battle.Vector2(Random.Range(0, 100f), Random.Range(0, 100f));
//            agent.simulator = simulator;
//
//            UnitController unitController = new UnitController(null);
//            unitController.HP = 100;
//
//            agent.UnitController = unitController;
//
//            agent.BattleAgentManager = manager;
//
//            simulator.AddAgent(agent);
//        }
    }

    void Update()
    {
//        simulator.Tick();
    }

//    void OnDrawGizmos()
//    {    
//        // 设置矩阵
//        Matrix4x4 defaultMatrix = Gizmos.matrix;
//        
//        // 设置颜色
//        Color defaultColor = Gizmos.color;
//        
//        foreach (BattleAgent agent in simulator.agents)
//        {
//            Gizmos.color = agent.GizmosColor;
//            Vector3 center = new Vector3(agent.position_.x(), 0, agent.position_.y());
//            drawCircle(center, 4.0f);
//        }
//        
//        // 恢复默认颜色
//        Gizmos.color = defaultColor;
//        
//        // 恢复默认矩阵
//        Gizmos.matrix = defaultMatrix;
//    }

    private void drawCircle(Vector3 center, float radius)
    {
        // 绘制圆环
        Vector3 beginPoint = center;
        Vector3 firstPoint = center;
        float m_Radius = radius;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += 0.1f)
        {
            float x = m_Radius * Mathf.Cos(theta) + center.x;
            float z = m_Radius * Mathf.Sin(theta) + center.z;
            Vector3 endPoint = new Vector3(x, 0, z);
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Gizmos.DrawLine(beginPoint, endPoint);
            }
            beginPoint = endPoint;
        }
        
        // 绘制最后一条线段
        Gizmos.DrawLine(firstPoint, beginPoint);
    }
}
