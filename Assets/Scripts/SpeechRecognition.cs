using System;
using System.Collections;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

using UnityEngine.Video;

public class SpeechRecognition : MonoBehaviour {

	[SerializeField]

	private string[] m_Keywords;

	private KeywordRecognizer m_Recognizer;

	public Animator animm;

	//private string[] rightWords;
	//private string[] wrongWords;

	public Text scoreText;
	public static int score;
	public float timeRemaining;
	private AudioSource animAudio;

	private bool pauseTimeIsRunning = false;
	//private Coroutine old;
	private IEnumerator controlCoroutine;

	private float timeWasted;
	private bool animPlayed;

	public AudioSource reminderAudioSource;
	public AudioSource repetitionAudioSource;
	
	public AudioClip reminderAudioClip;
	public AudioClip repetitionAudioClip;
	

	void Start()
	{
		timeWasted = 10f;
		//Start counting the time for the entire game.
		StartCoroutine(TimeUp());

		m_Keywords = new string[5];

		m_Keywords[0] = "Tunde";
		m_Keywords[1] = "Stand up";
		m_Keywords[2] = "Hello Tunde";
		m_Keywords[3] = "Great job";
		m_Keywords[4] = "Come";


		m_Recognizer = new KeywordRecognizer(m_Keywords);

		m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;

		m_Recognizer.Start();

		animAudio = GetComponent<AudioSource> ();
		
		score = 0;
    	scoreText.text = "Points: " + score;
		
	}

	private void Update()
	{
		//TimeExpired();
		timeWasted -= Time.deltaTime;
		
	}

	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{

		StringBuilder builder = new StringBuilder();
	
		if (args.text == m_Keywords[0])
		{
			animm.SetTrigger("LookUp");
			//...start counting for the pause time for the reminder audio to play.
			controlCoroutine = PauseTime(45f);
			StartCoroutine(controlCoroutine);
			if (animm.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
			{
				score += 5;
				scoreText.text = "Points: " + score;
				StopCoroutine(controlCoroutine);
			}

			/*if (pauseTimeIsRunning)
			{
				StopCoroutine(old);
				Debug.Log("First Coroutine stopped at " + Time.time);
			}*/
			
		}

		else if (args.text == m_Keywords[1])
		{
			animm.SetTrigger("StandUp");
			//...start counting for the pause time for the reminder audio to play.
			controlCoroutine = PauseTime(45f);
			StartCoroutine(controlCoroutine);
			if (animm.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
			{
				score += 7;
				scoreText.text = "Points: " + score;
				StopCoroutine(controlCoroutine);
				Debug.Log("Second Coroutine stopped at " + Time.time);

			}
		}
		else if (args.text == m_Keywords[2])
		{
			animm.SetTrigger("HelloTunde");
			//...start counting for the pause time for the reminder audio to play.
			controlCoroutine = PauseTime(45f);
			StartCoroutine(controlCoroutine);
			if (animm.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
			{
				score += 10;
				scoreText.text = "Points: " + score;
				StopCoroutine(controlCoroutine);
			}
		}
		else if (args.text == m_Keywords[3])
		{
			animm.SetTrigger("Respond");
			//...start counting for the pause time for the reminder audio to play.
			controlCoroutine = PauseTime(45f);
			StartCoroutine(controlCoroutine);
			//if the animation clip has finished playing...
			if (animm.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
			{
				score += score;
				scoreText.text = "Points: " + score;
				StopCoroutine(controlCoroutine);
			}
		}
		if (args.text == m_Keywords[4])
		{
			animm.SetTrigger("Ignore");
			//...start counting for the pause time for the reminder audio to play.
			controlCoroutine = PauseTime(45f);
			StartCoroutine(controlCoroutine);
			//if the animation clip has finished playing...
			if (animm.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
			{
				score -= 8;
				scoreText.text = "Points: " + score;
				StopCoroutine(controlCoroutine);
			}
		}
		
		//if you say Tunde and the picking book animation is playing, this block will be executed.
		if (args.text == m_Keywords[0] && animm.GetCurrentAnimatorStateInfo(0).IsName("Picking_book"))
		{
			//Temporarily, this is to know when you say a word more than twice. CHECK IT OUT!!!
			if (args.text.Length > 2)
			{
				//Do stuff
				//also check for when you are too loud!!! 
			}
		}

		builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);

		builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);

		builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);

		Debug.Log(builder.ToString());

	}
	
	void WastingTime()
	{
		if (timeWasted < 0)
		{
			//if the player has spent a considerable amount of time but is not getting anywhere
			//they should be reminded about the rules before they get thrown out of the experience.
		}
	}

	//PlayAnimAudio() plays the "hello uncle" or "hello aunty" audioclips.
	public void PlayAnimAudio()
	{
		repetitionAudioSource.clip = repetitionAudioClip;
		repetitionAudioSource.Play();
		
		//animAudio.Play();
	}

	public void HelloRepetition()
	{
		animAudio.Play();
		
		//repetitionAudioSource.clip = repetitionAudioClip;
		//repetitionAudioSource.Play();
	}

	public void ReminderAudio()
	{
		reminderAudioSource.clip = reminderAudioClip;
		reminderAudioSource.Play();
	}

	//PauseTime() starts counting once the current animation starts.
	//A voice reminds the player about how they are supposed to interact with Tunde.
	public IEnumerator PauseTime(float waitingTime)
	{
		yield return new WaitForSeconds(waitingTime);
		//old = StartCoroutine(PauseTime());
		pauseTimeIsRunning = true;
		ReminderAudio();
	}

	//Let's consider this//Start counting down when Tunde looks up. Meaning that the game technically starts when you get Tunde to look at you.
	//What it actually is - start counting down when the game starts.
	IEnumerator TimeUp()
	{
		yield return new WaitForSeconds(timeRemaining);
		animm.SetTrigger("Angry");
		//animm.SetBool("Angry", true);
		score -= score;
		scoreText.text = "Points: " + score;
		Debug.Log("Time is up");
	}
}