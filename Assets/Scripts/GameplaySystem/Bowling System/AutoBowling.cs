using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WAS.EventBus;

public class AutoBowling : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(BeginBowling());
    }

    private IEnumerator BeginBowling() {
        yield return new WaitForSeconds(3f);
        GameEventBus.Fire(new StartBowling());
        StartCoroutine(BeginBowling());
    }

}
 