using UnityEngine;
using UnityEngine.UI;
using WAS.EventBus;

public class BowlButton : MonoBehaviour
{
    [SerializeField]private Button bowlButton;
    void Start()
    {
        bowlButton.onClick.AddListener(() => GameEventBus.Fire(new StartBowling()));
    }
    
}
 