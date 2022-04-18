using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
  public static int[,] PATHS = {
    {8,   26},
    {21,  82},
    {43,  77},
    {50,  91},
    {54,  93},
    {62,  96},
    {66,  87},
    {80, 99},
    // snakes
    {98, 28},
    {95, 24},
    {92, 51},
    {83, 19},
    {73,  1},
    {69,  9},
    {64, 36},
    {59, 17},
    {55,  7},
    {52, 11},
    {48,  9},
    {46,  5},
    {44, 22},
  };
  public static int[,] DEBUG_PATHS = {
    {2, 51},
    {52, 1},
  };

  public static int MAX_PLAYERS = 4;
  public static int START_SPOT = 98;

  public enum GAME_STATE {
    WAIT_PLAYERS,
    WAIT_ROLL,
    MOVING,
    GAME_OVER,
    PAUSE,
    NEW_TURN,
  };

  public static Vector3[] SPOT_OFFSETS = {
    new Vector3( 1f, 0f,  1f),
    new Vector3(-1f, 0f,  1f),
    new Vector3( 1f, 0f, -1f),
    new Vector3(-1f, 0f, -1f),
  };

  public static string MAIN_SCENE = "Main";
}
