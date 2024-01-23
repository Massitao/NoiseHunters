using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonIDK : MonoBehaviour
{
    public bool used;

    [SerializeField] CountDownQuest cd;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Use()
    {
        cd.CountDownSelector();
        used = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.GetComponent<PlayerHealth>() != null && Input.GetKey(KeyCode.E) && !used)
        {
            Use();
        }
    }
}
