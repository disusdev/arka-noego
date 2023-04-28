using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace disusdev
{

public class CameraShaker : MonoBehaviour
{
	public static CameraShaker Instance;

  public Transform camTransform;
	public float shakeDuration = 0f;
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;

	Vector3 originalPos;

	void Awake()
	{
		Instance = this;

		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	public void AddDuration(float amount)
	{
		shakeDuration += amount;
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (shakeDuration > 0)
		{
			float trauma = shakeDuration * shakeDuration * shakeAmount;

			camTransform.localPosition = originalPos + Random.insideUnitSphere * trauma;
			
			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
		}
	}
}

}
