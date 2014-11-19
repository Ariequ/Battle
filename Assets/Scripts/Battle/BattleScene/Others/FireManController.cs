using UnityEngine;
using System.Collections;

public class FireManController : ISkillAdditionEffectController 
{
	private const float DISAPPEAR_Y = -1.0f;

	[Range(1.0f, 10.0f)]
	public float speed = 1.5f;

	public Animation die;
	public Animation run;
	
	private Vector3 targetDirection;
	private float runCompleteTime;

	private float MIN_RUN_TIME = 1.0f;
	private float MAX_RUN_TIME = 3.0f;

	private float RUN_LENGTH = 8f;

	protected override void OnAwake ()
	{
		die [die.clip.name].speed = BattleTime.timeScale;
		run [run.clip.name].speed = BattleTime.timeScale;
	}

	// Update is called once per frame
	void Update () 
	{
		if (BattleTime.time > this.runCompleteTime)
		{
			if (die.gameObject.activeSelf)
			{
				if (!die.isPlaying)
				{
					Vector3 position = transform.position;

					if (position.y < DISAPPEAR_Y * 0.5)
					{
						StopRun();
					}
					else
					{
						position.y = Mathf.Lerp(position.y, DISAPPEAR_Y, BattleTime.deltaTime);
						transform.position = position;
					}
				}
			}
			else
			{
				PlayDie();
			}
		}
		else
		{
			transform.position = transform.position + this.targetDirection * this.speed * BattleTime.deltaTime;
		}
	}

	public override void StartEffect ()
	{
		Vector2 targetPosition = Random.insideUnitCircle * RUN_LENGTH;
		this.targetDirection = new Vector3(targetPosition.x, 0, targetPosition.y);
		this.runCompleteTime = BattleTime.time + Random.Range(MIN_RUN_TIME, MAX_RUN_TIME);

		transform.rotation = Quaternion.LookRotation(this.targetDirection, Vector3.up);

		die.gameObject.SetActive(false);
		run.gameObject.SetActive(true);
		run.Play();
	}

	private void PlayDie ()
	{
		run.Stop();
		die.Play();

		run.gameObject.SetActive(false);
		die.gameObject.SetActive(true);
	}

	private void StopRun ()
	{
		run.Stop();
		die.Stop();

		elementController.Recycle();
	}
}
