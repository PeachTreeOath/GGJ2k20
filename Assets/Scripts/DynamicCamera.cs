using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Camera))]
public class DynamicCamera : MonoBehaviour
{
	List<BotBase> Bots = new List<BotBase>();
	//List<CinemachineVirtualCamera> VirtualCams = new List<CinemachineVirtualCamera>();
	[SerializeField] CinemachineVirtualCamera rotatingCam;
	[SerializeField] CinemachineVirtualCamera followingCam;

	[SerializeField] Transform centerPoint;
	[SerializeField] float arenaSize;

	[SerializeField] float thresholdOfInterest = 0.11f;

	Coroutine runningRoutine = null;
	bool coroutineIsRunning = false;
	
	// Start is called before the first frame update
    void Start()
    {
		Bots = FindObjectsOfType<BotBase>().ToList();
		rotatingCam = FindObjectOfType<CinemachineVirtualCamera>();
		rotatingCam.Priority = 10;
		followingCam.Priority = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (followingCam.Follow == null && !coroutineIsRunning)
		{
			runningRoutine = StartCoroutine(AssessPotentialHighlights());
			coroutineIsRunning = true;
		}
    }

	private IEnumerator AssessPotentialHighlights()
	{
		Debug.Log("RUNNING ASSESSMENT");

		yield return new WaitForSeconds(3);

		CullDeadBots();

		float centralValue = arenaSize / 2f;

		List<KeyValuePair<float, BotBase>> sortedBots = new List<KeyValuePair<float, BotBase>>();

		List<BotBase> potentialBots = new List<BotBase>(Bots);

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

			sortedBots.Add(new KeyValuePair<float, BotBase>(determination, bot));

			yield return null;
		}

		sortedBots = sortedBots.OrderByDescending(x => x.Key).ToList();

		if (sortedBots.Count > 0 && sortedBots[0].Key > thresholdOfInterest && sortedBots[0].Value != null)
		{
			Debug.Log("THRESHOLD MET");
			FocusOnBot(sortedBots[0].Value);
		}

		coroutineIsRunning = false;
	}

	private IEnumerator EndFollow()
	{
		Debug.Log("RELEASING CAM");

		yield return new WaitForSeconds(2f);

		followingCam.Priority = 5;

		followingCam.Follow = null;
		followingCam.LookAt = null;

		coroutineIsRunning = false;
	}

	public void CullDeadBots()
	{

	}

	private void FocusOnBot(BotBase botToFocus)
	{
		followingCam.transform.position = botToFocus.transform.position +
			(botToFocus.transform.forward * -2f) + (Vector3.up * 2f);
		followingCam.Follow = botToFocus.transform;
		followingCam.LookAt = botToFocus.transform;
		followingCam.Priority = 20;

		botToFocus.ReleaseCam += ReleaseCamera;
	}

	private void ReleaseCamera()
	{
		followingCam.Follow.gameObject.GetComponent<BotBase>().ReleaseCam -= ReleaseCamera;

		runningRoutine = StartCoroutine(EndFollow());
		coroutineIsRunning = true;
	}
}
