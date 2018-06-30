using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHealthSegment : MonoBehaviour {
    public RectTransform fill;
    public float fillRate = 1f;
    private Coroutine fillRoutine = null;
    public bool IsFilled { get; private set; }

    public void SetFilled(bool value, bool skipAnim = false) {
        if (value != IsFilled) {
            IsFilled = value;

            if (skipAnim == false) {
                StartCoroutine(FillSegment(value));
            } else {
                this.IsFilled = value;
                if (value == true) {
                    fill.localScale = Vector3.one;
                } else {
                    fill.localScale = Vector3.zero;
                }
            }
        }
    }

    private IEnumerator FillSegment(bool value) {
        if (fillRate <= 0f) yield break;

        float counter = 0f;
        while (counter < fillRate) {
            counter += Time.deltaTime;

            float fillAmount = counter / fillRate;
            if (value == false) {
                fillAmount = 1f - fillAmount;
            }

            fill.localScale = new Vector3(fillAmount, 1f, 1f);
            yield return null;
        }

        if (value == true) {
            fill.localScale = new Vector3(1f, 1f, 1f);
        } else {
            fill.localScale = new Vector3(0f, 1f, 1f);
        }
    }
}
