using UnityEngine;
using UnityEngine.UI;

public class FillBarController : MonoBehaviour
{
    [SerializeField]private Image fillBG;
    [SerializeField]private Image fillImage;

    public void UpdateFill(int maxAmount,int currAmount) {
        float value = (float)currAmount / (float)maxAmount;
        SetFill(value);
    }

    public void UpdateFill(float currAmount, float maxAmount) {
        float value = currAmount / maxAmount;
        SetFill(value);
    }

    public void SetFill(float value) {
        fillImage.fillAmount = value;
    }
}
