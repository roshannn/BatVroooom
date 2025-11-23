using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WAS.EventBus;

public class BowlButton : MonoBehaviour
{
    [SerializeField]private Button bowlButton;

    private void Awake() {
        
    }
    void Start()
    {
        bowlButton.enabled = false;
        bowlButton.image.enabled = false;
        GameEventBus.Fire(new StartBowling());
        StartCoroutine(BeginBowling());
    }

    private IEnumerator BeginBowling() {
        yield return new WaitForSeconds(3f);
        GameEventBus.Fire(new StartBowling());
        StartCoroutine(BeginBowling());
    }

}
 