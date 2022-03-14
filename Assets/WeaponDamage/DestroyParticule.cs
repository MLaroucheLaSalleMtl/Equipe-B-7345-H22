using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticule : MonoBehaviour
{
    public void DestroyMe()
    {
        Destroy(gameObject, 10f);
    }
}
