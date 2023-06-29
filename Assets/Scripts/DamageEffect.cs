using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageEffect : MonoBehaviour
{
    public TMP_Text textDisplay;
    public float lifeTime = 2;
    public float floatUp = 0;

    public void StartEffect(Vector3 _startPos, string _text, float _lifetime = 2, float _floatSpeed = 0)
    {
        StartEffect(_startPos, _text, Color.black, _lifetime, _floatSpeed);
    }
    public void StartEffect(Vector3 _startPos, string _text, Color _col, float _lifetime = 2, float _floatSpeed = 0)
    {
        transform.position = _startPos;
        textDisplay.text = _text;
        textDisplay.color = _col;
        lifeTime = _lifetime;
        floatUp = _floatSpeed;
    }
    public void Update()
    {
        lifeTime -= 1 * Time.deltaTime;
        transform.Translate(Vector3.up * floatUp * Time.deltaTime);
        if (lifeTime <= 0)
            gameObject.SetActive(false);
    }
}
