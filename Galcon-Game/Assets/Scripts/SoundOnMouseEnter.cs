using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnMouseEnter : MonoBehaviour
{
    void OnMouseEnter()
    {
        Debug.Log("Mouse enter");
        SoundFx.instance.PlaySound(SoundFx.instance.selectSound, .3f);
    }
}
