using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class QueueRoll : MonoBehaviour
{
	GameState gameState;

	int counter = 0;

    void Start () {}
    void Update() {}

    public void LinkWorld (GameState gs) {
    	this.gameState = gs;
    }

    public int Roll () {
    	return (int)(Math.Floor(Random.value * 6f) + 1f); 
    }

    public int Current () {
    	return counter % gameState.players.Length;
    }
    public int NextPlayer () {
    	counter += 1;
    	return Current();
    }    
}
