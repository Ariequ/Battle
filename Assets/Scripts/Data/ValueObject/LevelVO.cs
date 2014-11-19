using System;
using System.Collections;
using System.Collections.Generic;

public class LevelVO
{
	private LevelMeta meta;

	public LevelVO (LevelMeta meta)
	{
		this.meta = meta;
	}

	public LevelMeta Meta
	{
		get
		{
			return this.meta;
		}
	}
}
