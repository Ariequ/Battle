using UnityEngine;
using System.Collections.Generic;
using System;

public class FunctionFactory 
{
    private static FunctionFactory _instance;

    public static FunctionFactory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FunctionFactory();
            }

            return _instance;
        }
    }

    public IFunction Create(int fucntionID)
    {
		return new AbastractFunction(fucntionID);
    }
}

//class ScriptFunction : IFunction
//{
//    CSLE.ICLS_Type type;
//    CSLE.SInstance inst;//脚本实例
//    CSLE.CLS_Content content;//操作上下文
//    
//    private int _functionID;
//    
//    public ScriptFunction (int functionID)
//    {
//        _functionID = functionID;
//        string scriptTypeName = "Function" + functionID;
//        type = ScriptEnv.Instance.scriptEnv.GetTypeByKeywordQuiet (scriptTypeName);
//        if (type == null) {
//            Debug.LogError ("Type:" + scriptTypeName + "不存在于脚本项目中");
//            return;
//        }
//        content = ScriptEnv.Instance.scriptEnv.CreateContent ();
//        List<CSLE.CLS_Content.Value> param = new List<CSLE.CLS_Content.Value> ();
//        param.Add (new CSLE.CLS_Content.Value ());
//        param [0].type = typeof(int);
//        param [0].value = functionID;
//        inst = type.function.New (content, param).value as CSLE.SInstance;
//        content.CallType = inst.type;
//        content.CallThis = inst;
//        Debug.Log ("inst=" + inst);
//    }
//
//    public int getLimitID()
//    {
//        return _functionID;
//    }
//    
//    public void Execute(ILimitFuncitonContext context, Action<int,ILimitFuncitonContext> callBack)
//    {
//        List<CSLE.CLS_Content.Value> param = new List<CSLE.CLS_Content.Value> ();
//        param.Add (new CSLE.CLS_Content.Value ());
//        param [0].type = typeof(ILimitFuncitonContext);
//        param [0].value = context;
//
//        param.Add (new CSLE.CLS_Content.Value ());
//        param [1].type = typeof(Action<int,ILimitFuncitonContext>);
//        param [1].value = callBack;
//
//        type.function.MemberCall (content, inst, "Execute", param);
//    }
//}
