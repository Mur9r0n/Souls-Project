using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithMeRalf : MonoBehaviour
{
    public void Use()
    {
        Debug.Log(("YOU DID IT!"));
        Destroy(this.gameObject);
    }
}
