using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAsteroidAimHelper : UIGameplayElement {
    public RectTransform helperFill;
    public Color minColor = Color.gray;
    public Color maxColor = Color.white;

    protected override void OnPlayerAssigned(Planet player) {
        base.OnPlayerAssigned(player);
    }

    protected override void OnPlayerUnassigned(Planet player) {
        base.OnPlayerUnassigned(player);
    }

    private void Update() {
        helperFill.localScale = new Vector3(0f, 1f, 1f);


        if (ActivePlanet != null && ActiveController != null) {
            if (ActiveController.ActiveAsteroid != null && ActivePlanet.TargetPlanet != null) {
                Vector3 toTargetPlanet = (ActivePlanet.TargetPlanet.transform.position
                    - ActiveController.ActiveAsteroid.transform.position).normalized;

                float dot = Vector3.Dot(ActiveController.ActiveAsteroid.transform.forward, toTargetPlanet);

                var image = helperFill.GetComponent<UnityEngine.UI.Image>();
                image.color = Color.Lerp(minColor, maxColor, Mathf.Clamp01(dot));

                helperFill.localScale = new Vector3(Mathf.Clamp01(dot), 1f, 1f);
            }
        }
    }
}
