using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Composite : Behavior 
{
	protected List<Behavior> m_Children = new List<Behavior>();
	public void addChild(Behavior child) 
	{
		m_Children.Add(child);
	}
	public void removeChild(Behavior child)
	{
	}
 	public void clearChildren()
	{
	}
	public List<Behavior> getAllChild()
	{
		return m_Children;
	}
}