using UnityEngine;
using System.Collections.Generic;

public class LimitFactory
{
    private static LimitFactory _instance;

    public static LimitFactory Instance {
        get {
            if (_instance == null) {
                _instance = new LimitFactory ();
            }

            return _instance;
        }
    }

    public ILimit Create (int limitID)
    {
        return new AbstractLimit (limitID);
    }
}

//class ScriptLimit : ILimit
//{
//    CSLE.ICLS_Type type;
//    CSLE.SInstance inst;//脚本实例
//    CSLE.CLS_Content content;//操作上下文
//
//    private int _limitID;
//
//    public ScriptLimit (int limitID)
//    {
//        _limitID = limitID;
//        string scriptTypeName = "Limit" + limitID;
//        type = ScriptEnv.Instance.scriptEnv.GetTypeByKeywordQuiet (scriptTypeName);
//        if (type == null) {
//            Debug.LogError ("Type:" + scriptTypeName + "不存在于脚本项目中");
//            return;
//        }
//        content = ScriptEnv.Instance.scriptEnv.CreateContent ();
//        List<CSLE.CLS_Content.Value> param = new List<CSLE.CLS_Content.Value> ();
//        param.Add (new CSLE.CLS_Content.Value ());
//        param [0].type = typeof(int);
//        param [0].value = limitID;
//        inst = type.function.New (content, param).value as CSLE.SInstance;
//        content.CallType = inst.type;
//        content.CallThis = inst;
//        Debug.Log ("inst=" + inst);
//    }
//
//    public int getLimitID ()
//    {
//        return _limitID;
//    }
//    
//    public bool IsLimitConfirm (ILimitFuncitonContext context)
//    {
//        List<CSLE.CLS_Content.Value> param = new List<CSLE.CLS_Content.Value> ();
//        param.Add (new CSLE.CLS_Content.Value ());
//        param [0].type = typeof(ILimitFuncitonContext);
//        param [0].value = context;
//        CLS_Content.Value returnValue = type.function.MemberCall (content, inst, "IsLimitConfirm", param);
//        return (bool)returnValue.value;
//    }
//}
