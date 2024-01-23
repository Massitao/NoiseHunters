using UnityEngine;

public class Footprint : MonoBehaviour
{
    private Renderer rend;

    [SerializeField] private float fadeOffTime;

    private float decalAlpha;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();

        decalAlpha = rend.material.color.a;
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (decalAlpha > 0f)
        {
            decalAlpha = Mathf.Round(Mathf.InverseLerp(timer + fadeOffTime, timer, Time.time) * 100) / 100;

            rend.sharedMaterial.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, decalAlpha);
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
