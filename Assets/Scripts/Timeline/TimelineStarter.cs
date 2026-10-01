using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineStarter : MonoBehaviour
{
    private PlayableDirector director;
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        director.Play();
    }
}
