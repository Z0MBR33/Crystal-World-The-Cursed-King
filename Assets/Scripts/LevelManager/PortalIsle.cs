﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PortalIsle : MonoBehaviour {

    public GameObject BossPortal;
    public GameObject Podest1;
    public GameObject Podest2;
    public GameObject Podest3;

    [HideInInspector]
    public int PortalKeys = 0;
    [HideInInspector]
    public bool open;

    public void KeyArrived()
    {
        PortalKeys++;

        if (PortalKeys >= 3)
        {
            BossPortal.GetComponent<Renderer>().material.color = Color.yellow;
            open = true;
        }
    }

    public void teleport()
    {
        ObjectPool mr = ObjectPool.getObjectPool();
        
        //mr.returnAllObjects();
        //mr.preserveObjectsOnLoad();
        //mr.DestroyAllObjects();
        //SceneManager.LoadScene("Scenes/Boss");
    }
}