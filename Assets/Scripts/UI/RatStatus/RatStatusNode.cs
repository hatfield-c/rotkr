using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RatStatusNode : MonoBehaviour
{
    [SerializeField] Slider healthSlider = null;
    [SerializeField] RectTransform statusBar = null;
    [SerializeField] GameObject swimmingIconPrefab = null;
    [SerializeField] TextMeshProUGUI label = null;

    public void Init(RatData data, RatStateManager rat)
    {
        rat.ChangedHealth += OnChangedHealth;
        rat.ChangedStatus += OnChangedStatus;

        healthSlider.minValue = 0f;
        healthSlider.maxValue = data.MaxHealth;
        OnChangedHealth(data.CurrentHealth, data.CurrentHealth, data.MaxHealth);

        label.text = data.Name;
    }

    void OnChangedHealth(float oldHealth, float newHealth, float maxHealth)
    {
        Debug.Log($"Setting healthSlider value: {newHealth}");
        healthSlider.value = newHealth;
    }
    void OnChangedStatus(RatStateManager.RatAnimationMode mode)
    {
        switch (mode)
        {
            case RatStateManager.RatAnimationMode.Swimming:
                ApplyStatus(swimmingIconPrefab);
                break;
            default:
                ClearStatusBar();
                break;
        }   
    }
    void ClearStatusBar()
    {
        while(statusBar.childCount > 0)
            Destroy(statusBar.GetChild(0));
    }
    void ApplyStatus(GameObject icon)
    {
        Instantiate(icon, statusBar);
    }
}
