﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private ObjectPool mr;
    private LevelManager lvlManager;
    private Coroutine currentImmortalHandler;
    private bool isImmportal;
    [HideInInspector]
    public bool DieOnCollision;

    private MeshRenderer meshRenderer;
    private Lerper lerper;
    private Controll controller;

    [HideInInspector]
    public int NumberKeys = 0;

    // Use this for initialization
    void Start()
    {
        mr = ObjectPool.getObjectPool();
        lvlManager = LevelManager.getLevelManager();
        meshRenderer = GetComponent<MeshRenderer>();
        lerper = GetComponent<Lerper>();
        controller = GameMaster.getGameMaster().GetComponent<Controll>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isImmportal)
        {
            // flicker player
            meshRenderer.enabled = !meshRenderer.enabled;
        }

        if (lerper.Lerping)
        {
            controller.updateTeleportProgress(lerper.Ratio);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (DieOnCollision)
        {
            Stats stats = GetComponent<Stats>();
            stats.gotHit(100000);
        }

        if (isImmportal == false)
        {
            if (hit.gameObject.tag == "Enemy")
            {
                // TODO :  push player back

                becomeImmortal(3);
                Stats stats = GetComponent<Stats>();
                stats.gotHit(hit.gameObject.GetComponent<Stats>().strength);
                mr.getObject(ObjectPool.categorie.essential, (int)ObjectPool.essential.UI).GetComponent<UI_Canvas>().UpdateLive(stats.health, stats.maxHealth);
            }
        }

        if (hit.gameObject.tag == "DeathPlane")
        {
            Stats stats = GetComponent<Stats>();
            stats.gotHit(hit.gameObject.GetComponent<Stats>().strength);
        }


        if (hit.gameObject.tag == "Portal")
        {
            Portal portal = hit.gameObject.GetComponent<Portal>();

            if (portal.PortalActivated == true)
            {
                portal.Teleport();
            }

        }

        if (hit.gameObject.tag == "Item")
        {
            Item item = hit.gameObject.GetComponent<Item>();

            if (item.collected == false)
            {
                item.Collect(gameObject);
            }

        }

        if (hit.gameObject.tag == "Boss-Portal")
        {
            PortalIsle portalIsle = lvlManager.bossIsle.IsleObj.GetComponent<PortalIsle>();

            if (portalIsle.open == true)
            {
                portalIsle.teleport();
            }
        }
    }

    public void becomeImmortal(float time)
    {
        isImmportal = true;

        if (currentImmortalHandler != null)
        {
            StopCoroutine(currentImmortalHandler);
        }

        currentImmortalHandler = StartCoroutine(ImmportalHandler(time));

    }

    IEnumerator ImmportalHandler(float time)
    {
        yield return new WaitForSeconds(time);

        meshRenderer.enabled = true;
        isImmportal = false;
    }
}
