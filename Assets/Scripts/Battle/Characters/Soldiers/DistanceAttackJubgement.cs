using UnityEngine;
using System.Collections;

public class DistanceAttackJubgement : IAttackJubgement 
{
	public float distanceThreshold = 5f;

	public override string JubgeAttackStyle (string[] animationClips, Transform shootTransform, Transform enemyTransform, float distance, int attackMode)
	{
		return distance < distanceThreshold && animationClips.Length > 1 ? animationClips[1] : animationClips[0];
	}
}
