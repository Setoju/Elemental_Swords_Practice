using UnityEngine;

public class BladeSwinger : MonoBehaviour
{
    public float swingAngle = 90f;       
    public float swingSpeed = 5f;        
    private bool isSwinging = false;
    private Quaternion originalRotation;

    public GameObject bladeEffects;
    private bool effectsEnabled = true;

    void Start()
    {
        originalRotation = this.transform.localRotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSwinging)
        {
            StartCoroutine(SwingBlade());
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleBladeEffects();
        }
    }

    private System.Collections.IEnumerator SwingBlade()
    {
        isSwinging = true;

        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, 0, -swingAngle);
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * swingSpeed;
            this.transform.localRotation = Quaternion.Slerp(originalRotation, targetRotation, t);
            yield return null;
        }

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * swingSpeed;
            this.transform.localRotation = Quaternion.Slerp(targetRotation, originalRotation, t);
            yield return null;
        }

        this.transform.localRotation = originalRotation;
        isSwinging = false;
    }

    private void ToggleBladeEffects()
    {
        effectsEnabled = !effectsEnabled;

        ParticleSystem[] particles = bladeEffects.GetComponentsInChildren<ParticleSystem>(true);

        foreach (var ps in particles)
        {
            if (effectsEnabled)
            {
                ps.gameObject.SetActive(true);
                ps.Play();
            }
            else
            {
                ps.Stop();
                ps.gameObject.SetActive(false);
            }
        }

        Debug.Log("Blade effects " + (effectsEnabled ? "enabled" : "disabled") + ".");
    }
}
