// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/ScannerEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_DetailTex("Texture", 2D) = "white" {}
		_ScanSpeed("Scan Speed", float) = 0
		_ScanDistance("Scan Distance", float) = 0
		_ScanMaxDistance("Scan Max Distance", float) = 0
		_ScanStopOffset("Scan Stop Distance Offset", float) = 0
		_ScanWidth("Scan Width", float) = 10
		_LeadSharp("Leading Edge Sharpness", float) = 10
		_Repetitions("Repetitions", float) = 3
		_LeadColor("Leading Edge Color", Color) = (1, 1, 1, 0)
		_MidColor("Mid Color", Color) = (1, 1, 1, 0)
		_TrailColor("Trail Color", Color) = (1, 1, 1, 0)
		_HBarColor("Horizontal Bar Color", Color) = (0.5, 0.5, 0.5, 0)

		[Toggle(HORIZONTAL_BARS_ENABLE)]
		_HBarEnabled("Enable Horizontal Bars", float) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma shader_feature HORIZONTAL_BARS_ENABLE

			#include "UnityCG.cginc"

			struct VertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
			};

			struct VertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uv_depth : TEXCOORD1;
				float4 interpolatedRay : TEXCOORD2;
			};

			float4 _MainTex_TexelSize;
			float4 _CameraWS;

			VertOut vert(VertIn v)
			{
				VertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv.xy;
				o.uv_depth = v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif				

				o.interpolatedRay = v.ray;

				return o;
			}

			sampler2D _MainTex;
			sampler2D _DetailTex;
			sampler2D_float _CameraDepthTexture;
			float4 _WorldSpaceScannerPos;
			float _ScanDistance;
			float _ScanMaxDistance;
			float _ScanStopOffset;
			float _ScanWidth;
			float _LeadSharp;
			float _Repetitions;
			float4 _LeadColor;
			float4 _MidColor;
			float4 _TrailColor;
			float4 _HBarColor;
			float _HBarEnabled;

			float4 horizBars(float2 p)
			{
				return 1 - saturate(round(abs(frac(p.y * 100) * 2)));
			}

			float4 horizTex(float2 p)
			{
				return tex2D(_DetailTex, float2(p.x * 30, p.y * 40));
			}

			half4 frag (VertOut i) : SV_Target
			{
				half4 col = tex2D(_MainTex, i.uv);

				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
				float linearDepth = Linear01Depth(rawDepth);
				float4 wsDir = linearDepth * i.interpolatedRay;
				float3 wsPos = _WorldSpaceCameraPos + wsDir;
				half4 scannerCol = half4(0, 0, 0, 0);

				float dist = distance(wsPos, _WorldSpaceScannerPos);

				if ((dist < _ScanDistance) && (dist < _ScanMaxDistance + _ScanStopOffset) && (dist > _ScanDistance - _ScanWidth ) && (linearDepth < 1))
				{
					float diff = (_Repetitions - (_ScanDistance - dist) / (_ScanWidth / _Repetitions))%1;
					half4 edge = lerp(_MidColor, _LeadColor, pow(diff, _LeadSharp));

					float4 barsOrTex;

						if (_HBarEnabled == 1){barsOrTex = horizBars(i.uv);}		
						else{barsOrTex = horizTex(i.uv);}
							


					scannerCol = lerp(_TrailColor, edge, diff) + barsOrTex * _HBarColor;
					scannerCol *= diff;
				}

				return col + scannerCol;
			}
			ENDCG
		}
	}
}
