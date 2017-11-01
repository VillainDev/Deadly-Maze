using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class CombatMusicControl : MonoBehaviour {


	public AudioMixerSnapshot outOfShark;
	public AudioMixerSnapshot inShark;

	public AudioMixerSnapshot outOfWolf;
	public AudioMixerSnapshot inWolf;

	public AudioMixerSnapshot outOfSpider;
	public AudioMixerSnapshot inSpider;

	public AudioMixerSnapshot outOfFinish;
	public AudioMixerSnapshot inFinish;

	public AudioMixerSnapshot outOfTreasure;
	public AudioMixerSnapshot inTreasure;

	public float bpm = 128;


	private float m_TransitionIn;
	private float m_TransitionOut;
	private float m_QuarterNote;

	// Use this for initialization
	void Start () 
	{
		m_QuarterNote = 60 / bpm;
		m_TransitionIn = m_QuarterNote;
		m_TransitionOut = m_QuarterNote * 32;

	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("FightZone"))
		{
			inShark.TransitionTo(m_TransitionIn);
		} else if (other.CompareTag("WolfZone"))
		{
			inWolf.TransitionTo(m_TransitionIn);
		} else if (other.CompareTag("SpiderZone"))
		{
			inSpider.TransitionTo(m_TransitionIn);
		} else if (other.CompareTag("FinishZone"))
		{
			inFinish.TransitionTo(m_TransitionIn);
		} else if (other.CompareTag("ChestZone"))
		{
			inTreasure.TransitionTo(m_TransitionIn);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("FightZone"))
		{
			outOfShark.TransitionTo(m_TransitionOut);
		} else if (other.CompareTag("WolfZone"))
		{
			outOfWolf.TransitionTo(m_TransitionOut);
		} else if (other.CompareTag("SpiderZone"))
		{
			outOfSpider.TransitionTo(m_TransitionOut);
		} else if (other.CompareTag("FinishZone"))
		{
			outOfFinish.TransitionTo(m_TransitionOut);
		} else if (other.CompareTag("ChestZone"))
		{
			outOfTreasure.TransitionTo(m_TransitionOut);
		}
	}
}