using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBarSlider : MonoBehaviour
{
    public GameObject fill;
    public SpriteRenderer fillRenderer;

    public void SetFraction(float fraction)
    {
        Vector3 scale = fill.transform.localScale;
        Vector3 position = fill.transform.localPosition;
        fill.transform.localScale = new Vector3(fraction, scale.y, scale.z);
        fill.transform.localPosition = new Vector3((1 - fraction) / 2, position.y, position.z);
    }

    public void setHeightScale(float heightScale)
    {
        Vector3 scale = gameObject.transform.localScale;
        gameObject.transform.localScale = new Vector3(scale.x, heightScale * 0.4f, scale.z);
    }

    public void SetColour(Color colour)
    {
        fillRenderer.color = colour;
    }

    public void SetColourByType(HealthType type)
    {
        switch (type)
        {
            case HealthType.health:
                SetColour(Color.red);
                break;
            case HealthType.shield:
                SetColour(Color.blue);
                break;
            case HealthType.armour:
                SetColour(Color.yellow);
                break;
        }
    }
}
