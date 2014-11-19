using System;
using System.Collections;
using System.Collections.Generic;

using PureMVC.Patterns;

public class HeroProxy : Proxy
{
	new public const string NAME = "HeroProxy";

	public HeroProxy () : base(NAME)
	{

	}

	public override void OnRemove ()
	{

	}

	public HeroVO GetHeroVO(int heroMetaID)
	{
		foreach (HeroVO heroVO in MockServer.heroVOList)
		{
			if (heroVO.Meta.id == heroMetaID)
			{
				return heroVO;
			}
		}

		return null;
	}

	public HeroVO[] GetHeroVOArray()
	{
		return MockServer.heroVOList.ToArray ();
	}
}

