using UnityEngine;
using System.Collections;

public class IceManController : ISkillAdditionEffectController 
{
	private ParticleSystem[] particleSystems;

	public override void StartEffect ()
	{
		foreach (ParticleSystem particleSystem in particleSystems)
		{
			particleSystem.Play ();
		}
	}

	protected override void OnAwake ()
	{
		this.particleSystems = GetComponentsInChildren<ParticleSystem> ();
	}
}
