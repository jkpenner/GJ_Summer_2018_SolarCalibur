using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHealth : UIGameplayElement {
    public UIPlayerHealthSegment segmentPrefab;
    private UIPlayerHealthSegment[] segments;
    public Transform segmentParent;

    protected override void OnPlayerAssigned(Planet player) {
        base.OnPlayerAssigned(player);

        segments = new UIPlayerHealthSegment[player.MaxHealth];
        for (int i = 0; i < player.MaxHealth; i++) {
            segments[i] = Instantiate(segmentPrefab);
            segments[i].transform.SetParent(segmentParent, false);
            segments[i].SetFilled(true, true);
            segments[i].transform.SetSiblingIndex(i);
        }

        player.EventDamaged += OnPlayerDamaged;
    }

    protected override void OnPlayerUnassigned(Planet player) {
        base.OnPlayerUnassigned(player);

        for (int i = 0; i < segments.Length; i++) {
            Destroy(segments[i].gameObject);
        }
        segments = null;
    }

    private void OnPlayerDamaged(Planet player) {
        for (int i = 0; i < player.MaxHealth; i++) {
            segments[i].SetFilled(i < player.CurHealth);
        }
    }
}
