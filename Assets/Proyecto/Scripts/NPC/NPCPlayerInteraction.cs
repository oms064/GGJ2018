﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Movement))]
public class NPCPlayerInteraction : MonoBehaviour {
    [Range(1f, 50f)]
    public float sanity, conversion;
    private float currentSanity, currentConversion;
    private PlayerTeam currentPlayer = PlayerTeam.NONE, convertingPlayer = PlayerTeam.NONE;
    private bool converting;
    private NPCMovement movement;
    [Range(0.1f, 2f)]
    public float conversionRate, sanityRate;
    //public AudioMixer attackinMixer;
    //public GlobalGameManager ggm;
    
    private void Awake () {
        movement = GetComponent<NPCMovement>();
        //ggm =  GetComponent<GlobalGameManager>();
    }

    public void Update() {
        /*Debug.Log("con" + converting);
        if (converting) {
            float vol;
            bool x = attackinMixer.GetFloat("attacking", out vol);
            if(vol < 0.0f)
                attackinMixer.SetFloat("attacking", vol + (Time.deltaTime*10));
        } else {
            float vol;
            bool x = attackinMixer.GetFloat("attacking", out vol);
            if(vol > -40.0f)
                attackinMixer.SetFloat("attacking", vol - (Time.deltaTime * 10));
        }*/
    }

    public void StartConversion (PlayerTeam team) {
        if ( !converting && currentPlayer != team ) {
            converting = true;
            convertingPlayer = team;
        }
    }

    public void Converting(PlayerMovement movement, float delta) {
        var team = movement.team;
        if ( convertingPlayer == PlayerTeam.NONE ) {
            StartConversion( team );
        }
        if ( !converting || team != convertingPlayer ) {
            return;
        }

        currentConversion += delta * conversionRate;
        Debug.LogFormat( "Converting: {2} {0} {1}", delta, convertingPlayer, currentConversion );
        if(currentConversion >= conversion ) {
            Convert( movement );
        }
    }

    public void Convert(PlayerMovement movement) {
        var team = movement.team;
        movement.AddScore();
        Debug.LogFormat( "Converted {0}", team );
        currentPlayer = team;
        currentConversion = 0f;
        EndConversion();
        movement.Shout();
        currentSanity += sanityRate;
        if(currentSanity > sanity ) {

        }
    }

    public void EndConversion () {
        converting = false;
        convertingPlayer = PlayerTeam.NONE;
    }

    private void Explode () {
        GetComponent<NPCPoolObject>().Despawn();
    }

    public bool IsConverting() {
        return converting;
    }
}
