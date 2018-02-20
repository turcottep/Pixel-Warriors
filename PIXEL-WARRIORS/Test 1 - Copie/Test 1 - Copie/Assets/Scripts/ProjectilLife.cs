using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilLife : MonoBehaviour {

    private float lifeTime = 1.5f;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    

}
