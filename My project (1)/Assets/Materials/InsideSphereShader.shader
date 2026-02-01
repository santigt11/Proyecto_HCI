Shader "Custom/InsideSphereShader"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        Cull Off
        Lighting Off
        ZWrite On
        Pass {
            SetTexture [_MainTex] { combine texture }
        }
    }
}