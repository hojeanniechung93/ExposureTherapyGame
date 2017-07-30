using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRFadeHelper : MonoBehaviour {


    private Color transparent;

    public Color InvisibleScreenColor
    {
        get
        {
            return transparent;
        }
    }


    private Material fadeMaterial = null;
    private bool isFading = false;
    private YieldInstruction fadeInstruction = new WaitForEndOfFrame();

    /// <summary>
    /// Initialize.
    /// </summary>
    void Awake()
    {
        // create the fade material
        fadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
        transparent = fadeMaterial.color;
        transparent.a = 0f;
        fadeMaterial.color = transparent;
    }

    /// <summary>
    /// Turns the screen to a specific color
    /// </summary>
    /// <param name="to">The color we want to fade the screen to</param>
    /// <param name="fadeTime">The duration the fade animation</param>
    /// <param name="turnOffOverlayAtTheEnd">If the screen color overlay should be turned off at the end of the animation</param>
    /// <returns></returns>
    public IEnumerator FadeTo(Color to, float fadeTime, bool turnOffOverlayAtTheEnd = true)
    {
        float elapsedTime = 0.0f;
        Color from = fadeMaterial.color;
        Color color;
        isFading = true;

        while (elapsedTime < 1)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime/fadeTime;

            color = Color.Lerp(from, to, elapsedTime);
            fadeMaterial.color = color;
        }

        if(turnOffOverlayAtTheEnd)
        {
            isFading = false;
        }
    }

    /// <summary>
    /// Pulses the screen to a specific color
    /// </summary>
    /// <param name="pulseColor">The color it will pulse to</param>
    /// <param name="pulseDuration">The duration of each pulse</param>
    /// <param name="numberOfPulses">The number of pulses it should do</param>
    /// <returns></returns>
    public IEnumerator Pulse(Color pulseColor, float pulseDuration, int numberOfPulses)
    {
        float elapsedTime = 0.0f;

        bool isRed = false;
        Color current = pulseColor;

        float fadeDuration = pulseDuration/2f;
        var waitTime = pulseDuration * numberOfPulses;

        while (elapsedTime < waitTime)
        {
            yield return new WaitForSeconds(fadeDuration);
            if (isRed)
            {
                current = InvisibleScreenColor;
            }
            else
            {
                current = pulseColor;
            }
            isRed = !isRed;
            elapsedTime += fadeDuration;
            StartCoroutine(FadeTo(current, fadeDuration, turnOffOverlayAtTheEnd: false));
        }

        StartCoroutine(FadeTo(InvisibleScreenColor, fadeDuration, turnOffOverlayAtTheEnd: true));
    }

    /// <summary>
    /// Renders the fade overlay when attached to a camera object
    /// </summary>
    void OnPostRender()
    {
        if (isFading)
        {
            fadeMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Color(fadeMaterial.color);
            GL.Begin(GL.QUADS);
            GL.Vertex3(0f, 0f, -12f);
            GL.Vertex3(0f, 1f, -12f);
            GL.Vertex3(1f, 1f, -12f);
            GL.Vertex3(1f, 0f, -12f);
            GL.End();
            GL.PopMatrix();
        }
    }
}
