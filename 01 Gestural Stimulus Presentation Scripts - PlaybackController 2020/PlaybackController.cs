using System.Collections;
using System.Collections.Generic;
using System.IO; 
using UnityEngine;
using EliCDavis.RecordAndPlay;
using EliCDavis.RecordAndPlay.Playback;
using EliCDavis.RecordAndPlay.IO;
using EliCDavis.RecordAndPlay.Record;
using System;
using UnityEngine.Assertions.Must;
using System.Text;
using UnityEditorInternal;
using System.Numerics;

public class PlaybackController : MonoBehaviour, IActorBuilder, IPlaybackCustomEventHandler
{
	[SerializeField]
	GameObject collisionEmitter;

	public float trialDuration = 1500.0f ; 

	[SerializeField]
	AURORA_Proxy m_auroraProxy;
	[SerializeField]
	AURORA_Spawnable m_spawnable;
	[SerializeField]
	Transform spawnParent;


	private GameObject m_handVariantClone;

	public int numberOfStim; // = 20;
	[SerializeField]
	List<UnityEngine.Vector3> m_referenceFrameOffsets;

	[SerializeField]
	ReferenceFrameController m_referenceFrameController; 


	[SerializeField]
	Recording[] recordings = new Recording[20];

	private AwsS3.ClientState m_AwsS3Client; 


	public int[]  stimulusID ;
	private int CurrentStim = 0 ;
	private int NextStim;
	private Boolean changedToPlay = false;
	private float timer;

	public PlaybackBehavior[] playbackBehaviors;
	public PlaybackBehavior playbackBehavior;

	private StringBuilder csvBuilder2 = new StringBuilder();
	private string savePath2;

	bool dataHasBeenSaved = false;
	private float m_currentSecondsBetweenMotionChecks; 



	void Start()
	{
		savePath2 = DataPath2();
		// AwsS3 client 
		m_AwsS3Client = new AwsS3.ClientState();
		csvBuilder2.AppendLine("Time stamp,stimulusID,Seconds Between Motion Checks ");

		playbackBehaviors = new PlaybackBehavior[numberOfStim];
		stimulusID = new int[numberOfStim]; 

		//	Build the array of playback behaviors 
		for (var i = 0; i < numberOfStim; i++ ){
			playbackBehaviors[i] = PlaybackBehavior.Build(recordings[i], this, this, false);  // CHANGE THIS TO FALSE so that the recording does not repeat or loop
			playbackBehaviors[i].transform.parent = spawnParent;
			stimulusID[i] = i;
		 
		}
		m_referenceFrameController.AssignSecondsBetweenMotionChecks(0);

		// reshuffle the stimulusID array
		stimulusID = Shuffle(stimulusID);

		playbackBehavior = playbackBehaviors[CurrentStim];

		
	}
	

	void FixedUpdate() 
	{

		if (CurrentStim <= (numberOfStim-1) )
		{
		//	Debug.Log("current stimulus " + CurrentStim); 
			timer += Time.deltaTime;

			if (timer >= trialDuration && !playbackBehavior.CurrentlyPlaying())
			{
				changedToPlay = true;

				if (changedToPlay) // || timer >= 6.0f) 
				{
				//	m_referenceFrameController.AssignSecondsBetweenMotionChecks(UnityEngine.Random.Range(0,6));
				//	m_referenceFrameController.AssignSecondsBetweenMotionChecks(conditionNumber);
					NextStim = stimulusID[CurrentStim]; 

					playbackBehavior = playbackBehaviors[NextStim];

					playbackBehavior.transform.position = new UnityEngine.Vector3(0.001f,0.001f,0.001f);  // global position set to 0 in a way that's compliant with Google protobuff 
		//			spawnParent.transform.localPosition = m_referenceFrameOffsets[NextStim];

					playbackBehavior.Play();


					CurrentStim = CurrentStim + 1;
				
					timer = 0f;
					Debug.Log(CurrentStim);
					changedToPlay = false;

					m_currentSecondsBetweenMotionChecks = m_referenceFrameController.GetCurrentSecondsBetweenMotionChecks();
					AppendStimulusData();
				}
			}
		}
		else 
		{

			Debug.Log("END OF EXPERIMENT BLOCK !!!!!!!!!!!!! "); 
			if (!dataHasBeenSaved) 
			{
					SaveStimulusData();
					dataHasBeenSaved = true;

				//	Debug.Log("PRESS save!");
				if (dataHasBeenSaved) { 
					stimulusID = Shuffle(stimulusID);
					Debug.Log("Stimulus RESHUFFLED....!");
				}
				
			}

		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (!dataHasBeenSaved)
			{
				dataHasBeenSaved = true;
				SaveStimulusData();
				Debug.Log("StimulusID file saved by pressing ESCAPE ");
			}
		}


		 

	}


	public void RestartTrials()
	{
		for (var i = 0; i < numberOfStim; i++)
		{
			playbackBehaviors[i] = PlaybackBehavior.Build(recordings[i], this, this, false);  // CHANGE THIS TO FALSE so that the recording does not repeat or loop
			playbackBehaviors[i].transform.parent = spawnParent;

		}
		CurrentStim = 0;

	}

	public void SetTrialDuration(float duration)
    {
		trialDuration = duration; 

	}

	public Actor Build(int actorId, string actorName, Dictionary<string, string> metadata)
	{
		AURORA_GameObject ago = m_auroraProxy.PrimaryAuroraContext.Spawn(
			m_spawnable.SpawnableTypeUuid,
			new UnityEngine.Vector3(0.5f, 1.5f, -0.5f), UnityEngine.Quaternion.identity);  // This Vector3 is consistent with Google protobuff. Distance offset is 1 Unity meter 
		return new Actor(ago.gameObject);

	}

	public int[] Shuffle(int[] a) // 
	{
		// Knuth shuffle algorithm :: courtesy of Wikipedia :)
		for (int t = 0; t < a.Length; t++)
		{
			int tmp = a[t];
			int r = UnityEngine.Random.Range(t, a.Length);
			a[t] = a[r];
			a[r] = tmp;
		}
		return a; 
	}

	public void OnCustomEvent(SubjectRecording subject, CustomEventCapture customEvent)
	{
		if (subject == null)
		{
			Debug.LogFormat("Global Custom Event: {0} - {1}", customEvent.Name, customEvent.Contents);
		}
		else
		{
			Debug.LogFormat("Custom Event For {0}: {1} - {2}", subject.SubjectName, customEvent.Name, customEvent.Contents);
		}
	}

	public static string DataPath2()
	{
		return string.Format("{0}/{1}.csv", Application.dataPath, System.DateTime.Now.ToString("Stimulus_yyyy_MM_dd_HH_mm_ss"));
	}

	public void AppendStimulusData()
    {
		string newLine = string.Format("{0},{1},{2}",System.DateTime.UtcNow.ToString("u") , NextStim, m_currentSecondsBetweenMotionChecks); // additional info to write to file 
		csvBuilder2.AppendLine(newLine);
	}
	public void SaveStimulusData()
	{

		File.WriteAllText(savePath2, csvBuilder2.ToString());

			if (m_AwsS3Client.S3Client != null)
			{
				var response = m_AwsS3Client.PutTextObject(
					System.DateTime.UtcNow.ToString("yyyy/MM/dd/")
					 + "GesturesStimulus-" + System.DateTime.UtcNow.ToString("yyyy-MM-dd_") +Guid.NewGuid().ToString() + ".csv",
					 csvBuilder2.ToString() );

				Console.WriteLine(response.ToString());
			}

			Console.WriteLine("Done");
		dataHasBeenSaved = true;
	}
	

}

