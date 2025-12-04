using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    [SerializeField]
    private Image m_fadeImage;
    [SerializeField]
    private float m_fadeDuration = 7.0f;

    public void StartFade()
    {

        StartCoroutine(FadeOut());
        m_fadeImage.gameObject.SetActive(true);
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = m_fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f); // Black with alpha 1.

        while (timer < m_fadeDuration)
        {
            // Interpolate the color between start and end over time.
            m_fadeImage.color = Color.Lerp(startColor, endColor, timer / m_fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the image is completely black at the end.
        m_fadeImage.color = endColor;
    }
}