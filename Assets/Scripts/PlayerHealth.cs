using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public Image Bar;
    public float Fill;

    void Start () {
        Fill = 1f;
    }

    void Update () {

        Fill -= Time.deltaTime * 0.1f;

        Bar.fillAmount = Fill;
    }

}