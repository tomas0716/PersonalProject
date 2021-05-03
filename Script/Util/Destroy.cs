using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void Start()
    {
#if !_DEBUG
        GameObject.Destroy(gameObject);
#endif
    }
}