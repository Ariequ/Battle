using UnityEngine;
using System.Collections.Generic;

public class TestLimitFunction : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
//        ScriptEnv.Instance.Reset(null);
//        ScriptEnv.Instance.LoadProjectBytes("ScriptSystemCSLEDll.bytes");

        LimitFunctionNode node = LimitFunctionTreeFactory.Instance.Create(1);
        node.Execute(new AbstractLimitFunctionContext(), null);
    }
}
