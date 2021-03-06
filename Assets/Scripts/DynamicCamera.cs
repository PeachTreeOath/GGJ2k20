﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Camera))]
public class DynamicCamera : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera rotatingCam;
	[SerializeField] CinemachineVirtualCamera followingCam;

	[SerializeField] Transform centerPoint;
	[SerializeField] float arenaSize;

	[SerializeField] float thresholdOfInterest = 0.11f;
	[SerializeField] float reassessmentInterval = 5f;
	[SerializeField] float followedBotInterestHandicap = 0.02f;

	float timeSinceLastReassessment = 0f;

	Coroutine runningRoutine = null;
	bool coroutineIsRunning = false;
	
	// Start is called before the first frame update
    void Start()
    {
		rotatingCam.Priority = 10;
		followingCam.Priority = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!coroutineIsRunning && GameManager.instance.currentPhaseIndex != 0)
		{
			if (followingCam.Follow == null)
			{
				runningRoutine = StartCoroutine(AssessPotentialHighlights());
				coroutineIsRunning = true;
			}
			else if (timeSinceLastReassessment >= reassessmentInterval)
			{
				runningRoutine = StartCoroutine(AssessPotentialHighlights());
				coroutineIsRunning = true;
			}

			timeSinceLastReassessment += Time.deltaTime;
		}
    }

	private IEnumerator AssessPotentialHighlights()
	{
		Debug.Log("RUNNING ASSESSMENT");

		timeSinceLastReassessment = 0f;

		yield return new WaitForSeconds(3);

		CullDeadBots();

		float centralValue = arenaSize / 2f;

		List<KeyValuePair<float, BotBase>> sortedBots = new List<KeyValuePair<float, BotBase>>();

		List<BotBase> potentialBots = new List<BotBase>(GameManager.instance.ActiveBots ?? new List<BotBase>());

		foreach(BotBase bot in potentialBots)
		{
			float determination = Mathf.Clamp(
				Mathf.Pow(1f - ((centerPoint.position - bot.transform.position).magnitude / centralValue), 2f), 
				0f,
				1f);
			determination /= 10f;

			foreach (BotBase otherBots in potentialBots.Where(x => x != bot))
			{
				determination += 1f / Mathf.Clamp((bot.transform.position - otherBots.transform.position).sqrMagnitude * 20f, 1f, float.MaxValue); 
			}

			if (followingCam.Follow != null && bot == followingCam.Follow.gameObject.GetComponent<BotBase>())
			{
				determination -= followedBotInterestHandicap;
			}

			sortedBots.Add(new KeyValuePair<float, BotBase>(determination, bot));

			yield return null;
		}

		sortedBots = sortedBots.Where(x => GameManager.instance.ActiveBots.Contains(x.Value)).OrderByDescending(x => x.Key).ToList();

		float adjustedThreshold = thresholdOfInterest - (0.5f / Mathf.Sqrt(GameManager.instance.ActiveBots.Count));


		coroutineIsRunning = false;

		if (sortedBots.Count > 0 && sortedBots[0].Key > adjustedThreshold && sortedBots[0].Value != null)
		{
			Debug.Log("THRESHOLD MET");
			FocusOnBot(sortedBots[0].Value);
		}
		else if (followingCam.Follow != null)
		{
			ReleaseCamera();
		}
		
	}

	private IEnumerator EndFollow()
	{
		Debug.Log("RELEASING CAM");

		yield return new WaitForSeconds(1f);

		ReleaseVCReference();
	}

	public void CullDeadBots()
	{

	}

	private void ReleaseVCReference()
	{
		followingCam.Priority = 5;

		followingCam.Follow = null;
		followingCam.LookAt = null;

		coroutineIsRunning = false;
	}

	private void FocusOnBot(BotBase botToFocus)
	{
		if (followingCam.Follow != null)
		{
			followingCam.Follow.gameObject.GetComponent<BotBase>().DamageTaken -= ReassessHighlights;
			followingCam.Follow.gameObject.GetComponent<BotBase>().Death -= ReleaseCamera;
		}

		followingCam.transform.position = botToFocus.transform.position +
			(botToFocus.transform.forward * -2f) + (Vector3.up * 2f);
		followingCam.Follow = botToFocus.transform;
		followingCam.LookAt = botToFocus.transform;
		followingCam.Priority = 20;

		botToFocus.DamageTaken += ReassessHighlights;
		botToFocus.Death += ReleaseCamera;
	}

	public void ReleaseCamera(BotBase _ = null)
	{
		if (followingCam.Follow != null)
		{
			followingCam.Follow.gameObject.GetComponent<BotBase>().DamageTaken -= ReassessHighlights;
			followingCam.Follow.gameObject.GetComponent<BotBase>().Death -= ReleaseCamera;
		}

		if (coroutineIsRunning)
		{
			StopCoroutine(runningRoutine);
		}

		//runningRoutine = StartCoroutine(EndFollow());
		//coroutineIsRunning = true;
		ReleaseVCReference();
	}

	private void ReassessHighlights()
	{
		if (!coroutineIsRunning)
		{
			runningRoutine = StartCoroutine(AssessPotentialHighlights());
			coroutineIsRunning = true;
		}
	}
}
