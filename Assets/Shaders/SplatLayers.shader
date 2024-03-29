﻿//------------------------------//
//  SplatLayers.shader  //
//  Written by Jay Kay  //
//  2015/2/19  //
//------------------------------//
 
// derived from :
//  http://www.blog.radiator.debacle.us/2013/09/hacking-blend-transition-masks-into.html
//  https://alastaira.wordpress.com/2013/12/07/custom-unity-terrain-material-shaders/
//
 
 
Shader "Custom/SplatLayers" {
   Properties {
     [NoScaleOffset] _Control ("Control texture", 2D) = "white" {}
     [NoScaleOffset] _BumpMap ("Normal texture", 2D) = "bump" {}
     _Splat3 ("Layer 3 (A)", 2D) = "white" {}
     _Splat2 ("Layer 2 (B)", 2D) = "white" {}
     _Splat1 ("Layer 1 (G)", 2D) = "white" {}
     _Splat0 ("Layer 0 (R)", 2D) = "white" {}
   }
   SubShader {
     Tags {
       "SplatCount" = "4"
       "Queue" = "Geometry-100"
       "RenderType"="Opaque"
     }
 
     //LOD 200
 
     CGPROGRAM
     #pragma target 3.0
     #pragma require interpolators15
     #pragma surface surf Lambert
 
     sampler2D _MainTex;
 
     struct Input {
       float2 uv_Control : TEXCOORD0;
       float2 uv_Splat0 : TEXCOORD1;
       float2 uv_Splat1 : TEXCOORD2;
       float2 uv_Splat2 : TEXCOORD3;
       float2 uv_Splat3 : TEXCOORD4;
       float2 uv_NormalMap;
       
       float3 worldNormal;
       INTERNAL_DATA
     };
 
     sampler2D _Control, _NormalMap;
     sampler2D _Splat0,_Splat1,_Splat2,_Splat3;
 
     void surf (Input IN, inout SurfaceOutput o) {
        fixed4 splat_control = tex2D (_Control, IN.uv_Control);
        fixed3 col;
        col  = splat_control.r * tex2D (_Splat0, IN.uv_Splat0).rgb;
        col += splat_control.g * tex2D (_Splat1, IN.uv_Splat1).rgb;
        col += splat_control.b * tex2D (_Splat2, IN.uv_Splat2).rgb * (1 - splat_control.a);
        o.Albedo = col;
 
        float3 normalDirection = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
        normalDirection.z = -normalDirection.z; // correct for other-handedness of Unity world coordinates

        o.Normal = normalDirection;
     }
     ENDCG
   }
   FallBack "Diffuse"
}