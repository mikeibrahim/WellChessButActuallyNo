using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideBar : MonoBehaviour {
    public Slider slider;
	public GameObject fillColor;

	public void SetMaxValue(int value) {
		slider.maxValue = value;
		slider.value = value;
	}

	public void SetMinValue(int value) {
		slider.minValue = value;
		slider.value = value;
	}

	public void AddValue(int value) {
		slider.value += value;
		slider.value = Mathf.Clamp(slider.value, 0, slider.maxValue);
	}

    public void SetValue(int value) {
    	slider.value = value;
    }

	public int GetValue() {
		return (int) slider.value;
	}

	public int GetMaxValue() {
		return (int) slider.maxValue;
	}

	public void FillColor(Color32 col) {
		fillColor.GetComponent<Image>().color = col;
	}
}
