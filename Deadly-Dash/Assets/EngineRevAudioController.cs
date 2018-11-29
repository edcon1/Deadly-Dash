using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineRevAudioController : MonoBehaviour
{
    public AnimationCurve engineRevVolume;
  
    // Use this for initialization
    public IEnumerator RunSceneTransition()
    {
        GetComponent<AudioSource>().Play();
        DontDestroyOnLoad(gameObject);
        float timer = 0;
        float maxTime = 3.0f;
        
        while (timer <= maxTime)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        
            float t = timer / maxTime;
            float audioVolume = engineRevVolume.Evaluate(t);
            GetComponent<AudioSource>().volume = audioVolume;
        }
        Destroy(gameObject);

    }

    public void Test()
    {
        StartCoroutine(RunSceneTransition());
    }

}
