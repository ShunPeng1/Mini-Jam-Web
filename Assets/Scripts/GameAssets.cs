using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public GameAssets Instance;

    void Start()
    {
        Instance = this;
    }



    #region Instrument

    [Header("Instrument")] public GameObject drum;

    #endregion

}
