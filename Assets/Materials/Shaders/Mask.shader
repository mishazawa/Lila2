Shader "Unlit/Hider"
{
    SubShader
    {
        Tags { "Queue"="Transparent-1" }
        Zwrite On
        ColorMask 0
        Pass {}
    }
}
