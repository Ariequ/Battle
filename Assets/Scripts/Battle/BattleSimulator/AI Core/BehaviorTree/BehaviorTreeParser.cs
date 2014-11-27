using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

public class BehaviorTreeParser : MonoBehaviour
{
    public static XmlDocument loadXml(string filePath)
    {
        TextAsset xmlContent = Resources.Load<TextAsset>("Configs/AI/" + filePath);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlContent.text);
        return xmlDoc;
    }

    public static Behavior parseBehaviorTree(XmlNode xml)
    {
        if (xml != null)
        {
            switch (xml.Name)
            {
                case "Selector":
                    return parseCompositeNode(xml, typeof(Selector));
                case "PrioritySelector":
                    return parseCompositeNode(xml, typeof(PrioritySelector));
                case "Sequence":
                    return parseCompositeNode(xml, typeof(Sequence));
                case "Parallel":
                    return parseParallelNode(xml);
                case "Decorator":
                    return parseDecoratorNode(xml);
                case "Action":
                    return parseActionNode(xml);
            }
        }

        return new Behavior();
    }

    private static Behavior parseParallelNode(XmlNode xml)
    {
        string name = xml.Attributes["name"].Value;

//        Type parallelType = classMap[name];
		Parallel parallelNode = (Parallel)Activator.CreateInstance(Type.GetType(name));

        if (xml.SelectNodes("Condition").Count != 0)
        {
            IBehaviorCondition condition = parseCondition(xml.SelectNodes("Condition")[0]);
            parallelNode.setCondition(condition);
        }

        if (xml.Attributes["successPolicy"] != null)
        {
            parallelNode.setSussessPlicy(0);
        }
        if (xml.Attributes["failurePolicy"] != null)
        {
            parallelNode.setFailurePolicy(System.Convert.ToInt32(xml.Attributes["failurePolicy"].Value));
        }

        foreach (XmlNode child in xml.ChildNodes)
        {
            if (child.Name != "Condition")
            {
                parallelNode.addChild(parseBehaviorTree(child));
            }
        }

        Dictionary<string,string> param = new Dictionary<string,string>();
        for (int i = 0; i < xml.Attributes.Count; i++)
        {//遍历一级节点属性
            param.Add(xml.Attributes[i].Name, xml.Attributes[i].Value);
        }
        parallelNode.setParam(param);

        return parallelNode;
    }

    private static Behavior parseCompositeNode(XmlNode xml, System.Type type)
    {
        Composite compositeNode = (Composite)Activator.CreateInstance(type);

        if (xml.SelectNodes("Condition").Count != 0)
        {
            IBehaviorCondition condition = parseCondition(xml.SelectNodes("Condition")[0]);
            compositeNode.setCondition(condition);
        }
        
        foreach (XmlNode child in xml.ChildNodes)
        {
            if (child.Name != "Condition")
            {
                compositeNode.addChild(parseBehaviorTree(child));
            }
        }

        Dictionary<string,string> param = new Dictionary<string,string>();
        for (int i = 0; i < xml.Attributes.Count; i++)
        {//遍历一级节点属性
            param.Add(xml.Attributes[i].Name, xml.Attributes[i].Value);
        }
        compositeNode.setParam(param);

        return compositeNode;
    }

    private static Behavior parseDecoratorNode(XmlNode xml)
    {
        string name = xml.Attributes["name"].Value;

		Decorator decorator = (Decorator)Activator.CreateInstance(Type.GetType(name));
        decorator.setChild(parseBehaviorTree(xml.ChildNodes[0]));

        Dictionary<string,string> param = new Dictionary<string,string>();
        for (int i = 0; i < xml.Attributes.Count; i++)
        {//遍历一级节点属性
            param.Add(xml.Attributes[i].Name, xml.Attributes[i].Value);
        }
        decorator.setParam(param);
    
        return decorator;
    }

    private static IBehaviorCondition parseCondition(XmlNode xml)
    { 
        IBehaviorCondition condition = null;
        XmlNodeList children = xml.ChildNodes;

        string name = xml.Attributes["name"].Value;
//        Type conditionType = classMap[name];

        if (children.Count == 0)
        {
			condition = (IBehaviorCondition)Activator.CreateInstance(Type.GetType(name));

            Dictionary<string,string> param = new Dictionary<string,string>();
            
            for (int i = 0; i < xml.Attributes.Count; i++)
            {//遍历一级节点属性
                param.Add(xml.Attributes[i].Name, xml.Attributes[i].Value);
            }
            
            condition.setParam(param);
        }
        else
        if (children.Count == 1)
        {
            IBehaviorCondition targetConditon = parseCondition(children[0]);
            IBehaviorCondition[] args = new IBehaviorCondition[1];
            args[0] = targetConditon;
			condition = (IBehaviorCondition)Activator.CreateInstance(Type.GetType(name), args);
        }
        else
        if (children.Count == 2)
        {
            IBehaviorCondition targetConditonA = parseCondition(children[0]);
            IBehaviorCondition targetConditonB = parseCondition(children[1]);
            IBehaviorCondition[] args = new IBehaviorCondition[2];
            args[0] = targetConditonA;
            args[1] = targetConditonB;
			condition = (IBehaviorCondition)Activator.CreateInstance(Type.GetType(name), args);
        }

        return condition;
    }

    private static BTAction parseActionNode(XmlNode xml)
    {
        string name = xml.Attributes["name"].Value;

		BTAction action = (BTAction)Activator.CreateInstance(Type.GetType(name));

        if (xml.SelectNodes("Condition").Count != 0)
        {
            IBehaviorCondition condition = parseCondition(xml.SelectNodes("Condition")[0]);
            action.setCondition(condition);
        }

        Dictionary<string,string> param = new Dictionary<string,string>();  
        for (int i = 0; i < xml.Attributes.Count; i++)
        {//遍历一级节点属性
            param.Add(xml.Attributes[i].Name, xml.Attributes[i].Value);
        }   
        action.setParam(param);
        return action;
    }


}
