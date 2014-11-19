using System;
using System.Collections.Generic;

public class BuffVO
{
    public BuffMeta meta;

    public Faction casterFaction;

    public float remainTime;
    public float remainFrequency;

    public float remainCount;

    public int point;
    public float percent;

    public BuffVO(BuffMeta meta)
    {
        this.meta = meta;
        
        remainTime = meta.duration;
        remainFrequency = meta.frequency;
        remainCount = meta.lastCount;
        
        point = RandomUtil.Range(meta.minValue, meta.maxValue);
        percent = meta.addPercent;
    }

    public void Add()
    {
        remainTime += meta.duration;
        remainCount += meta.lastCount;
        
        point = RandomUtil.Range(meta.minValue, meta.maxValue);
        percent = meta.addPercent;
    }
}

