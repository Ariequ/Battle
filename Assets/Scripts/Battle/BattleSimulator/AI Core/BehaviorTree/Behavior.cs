using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class Behavior
{
	public enum Status
	{
		BH_INVALID,
		BH_SUCCESS,
		BH_FAILURE,
		BH_RUNNING,
		BH_ABORTED
    }
	;

	protected Status m_eStatus;
	private Dictionary<string,string> m_param;

	public Behavior ()
	{
		m_eStatus = Status.BH_INVALID;
	}

	public Behavior (IBehaviorCondition condition)
	{
		m_eStatus = Status.BH_INVALID;
		setCondition (condition);
	}
    
    virtual public void onInitialize (IAIContext context)
	{
	}

	virtual public void onTerminate (Status status)
	{

	}

    public Status tick (IAIContext context)
    {
        if (context == null)
        {
            Debug.Log("behavior tick context is null");
        }

        if (m_eStatus == Status.BH_INVALID) 
        {
            if (CheckCondition(context))
            {
                onInitialize (context);
            }
            else
            {
                return Status.BH_FAILURE;
            }

        }

        Status status = update(context);

        if (status != Status.BH_RUNNING) 
        {
            onTerminate (status);

            m_eStatus = Status.BH_INVALID;
        }
        else
        {
            m_eStatus = status;
        }

        return status;
        
//        m_eStatus = update (context);
//
//        if (m_eStatus != Status.BH_RUNNING) 
//        {
//            onTerminate (m_eStatus);
//
//            m_eStatus = Status.BH_INVALID;
//        }
//        
//        return m_eStatus;
    }

    virtual public Status update (IAIContext context)
	{
		return m_eStatus;
	}
    
	public void reset ()
	{
		m_eStatus = Status.BH_INVALID;
	}
    
	public void abort ()
	{
		onTerminate (Status.BH_ABORTED);
		m_eStatus = Status.BH_ABORTED;
	}

	public bool isTerminated ()
	{
		return m_eStatus == Status.BH_SUCCESS || m_eStatus == Status.BH_FAILURE;
	}
    
	public bool isRunning ()
	{
		return m_eStatus == Status.BH_RUNNING;
	}

	public Status getStatus ()
	{
		return m_eStatus;
	}

	private IBehaviorCondition _condition = null;
    
	public IBehaviorCondition getCondition ()
	{
		return _condition;
	}
    
	public void setCondition (IBehaviorCondition value)
	{
		_condition = value;
	}

	private string _name = "behavior";

	public string getName ()
	{
		return _name;
	}

	public void setName (string name)
	{
		_name = name;
	}

	private AIContext _context;

	public void setContext (AIContext context)
	{
		_context = context;
	}

	public AIContext getContext ()
	{
		return _context;
	}

	public void setParam(Dictionary<string,string> param)
	{
		m_param = param;
	}
	
	public Dictionary<string,string> getParam()
	{
        return m_param;
    }

    protected bool CheckCondition(IAIContext context)
	{
		IBehaviorCondition condition = getCondition();
		return condition == null || condition.isConditionTrue (context);
	}
}
