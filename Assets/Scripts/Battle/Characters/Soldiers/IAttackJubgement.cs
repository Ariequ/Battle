using UnityEngine;
using System.Collections;

public abstract class IAttackJubgement : MonoBehaviour 
{
	public abstract string JubgeAttackStyle (string[] animationClips, Transform shootTransform, Transform enemyTransform, float distance, int attackMode);
}
