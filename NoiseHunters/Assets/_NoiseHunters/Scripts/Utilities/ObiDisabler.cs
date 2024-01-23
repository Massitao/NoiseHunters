using UnityEngine;
using Obi;

public class ObiDisabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this, Time.deltaTime);
    }

    private void OnDestroy()
    {
        GetComponent<ObiFixedUpdater>().enabled = false;
    }
}
