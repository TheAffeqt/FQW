﻿Shader "CrossSection/OnePlaneBSP_modify" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_CrossColor("Cross Section Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		//_Glossiness("Smoothness", Range(0,1)) = 0.5
		_MetallicGlossMap("MetallGlosMap", 2D) = "MetalGlos"{}
		_PlaneNormal("PlaneNormal",Vector) = (0,1,0,0)
		_PlanePosition("PlanePosition",Vector) = (0,0,0,1)
		_StencilMask("Stencil Mask", Range(0, 255)) = 255
		_BumpMap ("Bumpmap", 2D) = "bump" {}
		//_AOTex ("Ambient Occlusion", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		//LOD 200
		Stencil
		{
			Ref [_StencilMask]
			CompBack Always
			PassBack Replace

			CompFront Always
			PassFront Zero
		}
		Cull Back
			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _MetallicGlossMap;
			sampler2D _BumpMap;
			sampler2D _AOTex;
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float2 uv_AOTex;
			float3 worldPos;
		};

		
		fixed4 _Color;
		fixed4 _CrossColor;
		fixed3 _PlaneNormal;
		fixed3 _PlanePosition;
		bool checkVisability(fixed3 worldPos)
		{
			float dotProd1 = dot(worldPos - _PlanePosition, _PlaneNormal);
			return dotProd1 > 0  ;
		}
		void surf(Input IN, inout SurfaceOutputStandard o) {
			if (checkVisability(IN.worldPos))discard;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 cSpec = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			//fixed4 aocclusion = tex2D(_AOTex, IN.uv_AOTex);
			o.Albedo = c.rgb;
			//o.Occlusion = aocclusion.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = cSpec.rgb;
			o.Smoothness = cSpec.a;
			o.Alpha = c.a;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
		
			Cull Front
			CGPROGRAM
#pragma surface surf NoLighting  noambient

		struct Input {
			half2 uv_MainTex;
			float3 worldPos;

		};
		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _CrossColor;
		fixed3 _PlaneNormal;
		fixed3 _PlanePosition;
		bool checkVisability(fixed3 worldPos)
		{
			float dotProd1 = dot(worldPos - _PlanePosition, _PlaneNormal);
			return dotProd1 >0 ;
		}
		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
		{
			fixed4 c;
			c.rgb = s.Albedo;
			c.a = s.Alpha;
			return c;
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			if (checkVisability(IN.worldPos))discard;
			o.Albedo = _CrossColor;

		}
			ENDCG
		
	}
	//FallBack "Diffuse"
}
