using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableMaterialChanger : MonoBehaviour
{
    public Color onColor = new Color(30, 101, 171, 255);
    public float colorIntensity = 0.35f;
    //public Color offColor;
    public float changeSpeed = 0.2f;
    public bool activeOnPlay;

    Renderer myRend;
    bool isChanging;
    float t = 0;
    Color lerpedColor;

    // Start is called before the first frame update
    void Start()
    {
        myRend = GetComponent<Renderer>();
        if (activeOnPlay)
        {
            myRend.material.SetColor("_EmissionColor", onColor * colorIntensity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TurnOnCable();
    }

    public void TurnOnCable()
    {
        Color oldColor = myRend.material.GetColor("_EmissionColor");
        //myRend.material = new Material(Shader.Find("Standard"));
        if (isChanging)
        {
            if (t < 1)
            {
                t += changeSpeed * Time.deltaTime;
                lerpedColor = Color.Lerp(oldColor, onColor, t);
                myRend.material.SetColor("_EmissionColor", lerpedColor * colorIntensity);
            }
        }
    }
    public void ChangeMaterial()
    {
        isChanging = true;
    }

}
