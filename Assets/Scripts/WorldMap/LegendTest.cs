using UnityEngine;
using System;

public class LegendTest : MonoBehaviour
{
	void Start()
	{
		LegendVO vo = new LegendVO();
		vo.id = 2;

		LegendFactory factory = new LegendFactory(this, vo);
		factory.LoadGlobal((UnityEngine.Object obj) => {
			factory.LoadMapblock(1, null);
			factory.LoadMapblock(2, null);
			factory.LoadMapblock(3, null);
		});
	}
}

