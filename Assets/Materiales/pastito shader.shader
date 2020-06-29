// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Cylinder/Diffuse-Cyl" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			Cull Back

			CGPROGRAM
			#pragma surface surf Lambert vertex:vert addshadow

			sampler2D _MainTex;
			fixed4 _Color;
			float CURVE_OFFSET;

			void vert(inout appdata_full v)
			{
				if (CURVE_OFFSET != 0)
				{
					float4 vpos = UnityObjectToClipPos(v.vertex);
					float l = length(float2(vpos.x, vpos.z));
					v.vertex.xyz += mul(unity_WorldToObject, float4(0,-1,0,0)) * l * l * CURVE_OFFSET;
					v.normal = normalize(v.normal + mul(unity_WorldToObject, float4(0, -1, 0, 0)) * l * l * CURVE_OFFSET);
				}
			}

			struct Input {
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}

			ENDCG

	}

		Fallback "Diffuse"
}