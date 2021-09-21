using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripBar : MonoBehaviour
{
    Slider tripBar;

    void Start()
    {
        tripBar = GetComponent<Slider>();
    }

    private void Update()
    {
        SetValue(TripManager.Instance.tripFactor);
    }

    public void SetValue(float val)
    {
        tripBar.value = val;
    }
}
