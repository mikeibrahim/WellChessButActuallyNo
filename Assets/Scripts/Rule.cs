using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Rule : MonoBehaviour {
	int ruleIndex;
	[SerializeField] private Image ruleThumbnail;
	[SerializeField] private TMP_Text text_ruleName;
	[SerializeField] private Button button_activated;
	[SerializeField] private TMP_Text text_activated;
	bool isActivated = false;

    void Start() {
        ruleThumbnail.sprite = Thumbnail.Instance.GetRuleThumbnail(ruleIndex);
		button_activated.onClick.AddListener( delegate {  ToggleActive();  }); // toggle activation
    }

	private void ToggleActive() {
		isActivated = !isActivated;

		text_activated.text = isActivated ? "<color=green>Activated</color>" : "<color=red>Deactivated</color>";
	}

	public void SetRuleIndex(int index) => ruleIndex = index;
	public void SetRuleName(string name) => text_ruleName.text = name;
}
