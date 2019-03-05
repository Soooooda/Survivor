using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class day_night : MonoBehaviour {

    public float intensityvl = 1;
    private Light light;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
        StartCoroutine(startNightCycle());
    }

    // Update is called once per frame
    void Update () {
        light.intensity = intensityvl;
	}
    IEnumerator startNightCycle()
    {
        yield return new WaitForSeconds(7);
        intensityvl = 0.8f;
        yield return new WaitForSeconds(0.5f);
        intensityvl = 0.6f;
        yield return new WaitForSeconds(0.5f);
        intensityvl = 0.4f;
        yield return new WaitForSeconds(0.5f);
        intensityvl = 0.2f;
        yield return new WaitForSeconds(0.5f);
        intensityvl = 0;
        StartCoroutine(startDayCycle());
    }

    IEnumerator startDayCycle()
    {
        yield return new WaitForSeconds(7);
        intensityvl = 0.2f;
        yield return new WaitForSeconds(0.5f);
        intensityvl = 0.4f;
        yield return new WaitForSeconds(0.5f);
        intensityvl = 0.6f;
        yield return new WaitForSeconds(0.5f);
        intensityvl = 0.8f;
        yield return new WaitForSeconds(0.5f);
        intensityvl = 1;
        StartCoroutine(startNightCycle());
    }
}
