﻿using UnityEngine;
using System.Collections;

public class EmoteSpriteAction : MonoBehaviour
{
	Animator animator;

	int hashStatePositive = Animator.StringToHash("Emotion.Positive");
	int hashStateNegative = Animator.StringToHash("Emotion.Negative");
	int hashStateWarmingup = Animator.StringToHash("Emotion.Warmingup");

	int hashSpeed = Animator.StringToHash("Speed");

	void Awake ()
	{
		animator = GetComponent<Animator>();
	}

	void Update ()
	{
		animator.SetFloat(hashSpeed, Input.GetAxisRaw("Horizontal") );

		if (Input.GetKeyDown (KeyCode.Z)) { animator.Play(hashStatePositive); }
		if (Input.GetKeyDown (KeyCode.X)) { animator.Play(hashStateNegative); }
		if (Input.GetKeyDown (KeyCode.C)) { animator.Play(hashStateWarmingup); }
	}
}
