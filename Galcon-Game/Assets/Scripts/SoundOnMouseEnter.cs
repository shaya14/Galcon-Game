using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnMouseEnter : MonoBehaviour
{
    void OnMouseEnter()
    {
        Debug.Log("Mouse enter");
        SoundFx.Instance.PlaySound(SoundFx.Instance._selectSound, .3f);
    }
}
