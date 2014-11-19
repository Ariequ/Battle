using System;

public enum BuffType
{
    Damage,
    Recover,
    Faint,
    Slow,
    Vulnerablity,
    Property,
}

public class BuffMeta
{
    ///状态ID
    public int id;
    
    public string metaName;
    
    ///状态名称ID
    public int nameID;
    ///状态描述ID
    public int descriptionID;
    
    ///状态类型
    public BuffType type;
    
    ///状态分组
    public int group;
    
    ///状态目标
    public bool toSelf;
    
    ///是否AOE
    public bool isAOE;
    
    ///是否随机目标
    public bool isRandom;
    
    ///技能中心范围
    public float coreRadius;
    ///技能边缘范围
    public float edgeRadius;
    
    ///最远有效施放距离
    public float maxDistance;
    
    ///技能图标
    public string iconPath;
    ///视觉特效
    public string effectPath;
    ///效果延迟
    public float valueDelay;
    
    ///技能影响属性
    public PropertyType propertyType; 
    ///技能加成系数
    public float addPercent;
    ///技能效果数值最小值
    public int minValue;
    ///技能效果数值最大值
    public int maxValue;
    
    ///触发技能列表
    public SkillLinkMeta[] skillLinkMetaArray;

    ///状态持续时间
    public int duration;
    ///状态频率
    public int frequency;
    ///状态持续次数
    public int lastCount;
}

