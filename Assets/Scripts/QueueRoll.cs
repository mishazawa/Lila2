using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class QueueRoll : MonoBehaviour {
    State gameState;
    int counter = 0;

    public void LinkWorld (State gs) {
    	this.gameState = gs;
    }

    public int DebugRoll (int a) {
      Debug.Log("Debug Roll");
      return a;
    }
    public int Roll () {
    	return (int)(Math.Floor(Random.value * 6f) + 1f);
    }

    public int Current () {
    	return counter % gameState.GetPlayers().Count;
    }
    public int NextPlayer () {
    	counter += 1;
    	return Current();
    }
    public void Reset () {
      counter = 0;
    }
}
