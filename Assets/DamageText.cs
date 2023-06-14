using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;
    public float destroyDelay = 1f;

    public TextMeshProUGUI damageTextPrefab;


    public IEnumerator MoveAndFade()
    {
        float timer = 0f;

        while (timer < destroyDelay)
        {
            Vector3 newPos = transform.position + Vector3.up * moveSpeed * Time.deltaTime;
            transform.position = newPos;

            timer += Time.deltaTime;
            yield return null;
        }
        /**
        Color startColor = damageText.color;
        float alpha = startColor.a;

        while (alpha > 0)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            damageText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        **/
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
