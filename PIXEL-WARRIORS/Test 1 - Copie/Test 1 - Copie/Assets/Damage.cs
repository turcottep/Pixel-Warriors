using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {

    public float damage;
    public int attackType;
	
    public float getDamage()
    {
        return damage;
    }

    public void setDamage(float d)
    {
        damage = d;
    }

}
