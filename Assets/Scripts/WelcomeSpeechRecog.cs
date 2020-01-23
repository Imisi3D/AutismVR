using System;
using System.Collections;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.UI;

using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class WelcomeSpeechRecog : MonoBehaviour {

	[SerializeField]

	private string[] m_Keywords;

	private KeywordRecognizer m_Recognizer;
	
	

	void Start()
	{

		m_Keywords = new string[2];

		m_Keywords[0] = "Male";
		m_Keywords[1] = "Female";

		m_Recognizer = new KeywordRecognizer(m_Keywords);

		m_Recognizer.OnPhraseRecognized += OnPhraseRecognized;

		m_Recognizer.Start();

	}

	private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
	{

		StringBuilder builder = new StringBuilder();
	
		//float newX = UnityEngine.Random.Range(-3, 3);

		//float newZ = UnityEngine.Random.Range(-3, 3);
	
		if (args.text == m_Keywords[0])
		{
			SceneManager.LoadScene("Male");
		}

		else if (args.text == m_Keywords[1])
		{
			SceneManager.LoadScene("Female");
		}
		
		builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);

		builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);

		builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);

		Debug.Log(builder.ToString());

	}
}