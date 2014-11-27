using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Battle;

public class AIContext : IAIContext
{
    private Dictionary<string,object> m_Param;

    public void setParam(Dictionary<string,object> param)
    {
        m_Param = param;
    }

    public object getValueOfKeyWithDefault(string key, object value = null)
    {
        if (m_Param != null)
        {
            object result = null;
            m_Param.TryGetValue(key, out result);

            return result != null ? result : value;
        }
        else
        {
            return value;
        }
    }

    public void ModifyOrAddValueOfNotExsitKey(string key, object value)
    {
        if (m_Param == null)
        {
            m_Param = new Dictionary<string, object>();
        }

        m_Param[key] = value;
    }

    public void DeletParamOfKey(string key)
    {
        m_Param[key] = null;
    }

    public object Agent
    {
        get
        {
            return this;
        }
    }
}
