// Hash without Sine
// MIT License...
/* Copyright (c)2014 David Hoskins.
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

Shader "Sprites/Static"
{
	Properties
	{
		[PerRendererData] _MainTex("Static mask", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		PixelSize("Pixel size", Float) = 1
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile _ PIXELSNAP_ON
				#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
				#include "UnityCG.cginc"

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
				};

				fixed4 _Color;

				v2f vert(appdata_t IN)
				{
					v2f OUT;
					OUT.vertex = UnityObjectToClipPos(IN.vertex);
					OUT.texcoord = IN.texcoord;
					OUT.color = IN.color;

					#ifdef PIXELSNAP_ON
					OUT.vertex = UnityPixelSnap(OUT.vertex);
					#endif

					return OUT;
				}

				sampler2D _MainTex;
				sampler2D _AlphaTex;
				float4 _MainTex_TexelSize;
				float PixelSize;

				fixed4 SampleSpriteTexture(float2 uv)
				{
					fixed4 color = tex2D(_MainTex, uv);

					#if ETC1_EXTERNAL_ALPHA
					// get the color from an external texture (usecase: Alpha support for ETC1 on android)
					color.a = tex2D(_AlphaTex, uv).r;
					#endif

					return color;
				}

				float hash12(float2 p)
				{
					float3 p3 = frac(p.xyx * .1031);
					p3 += dot(p3, p3.yzx + 33.33);
					return frac((p3.x + p3.y) * p3.z);
				}

				float hash13(float3 p)
				{
					float3 p3 = frac(p.xyz * .1031);
					p3 += dot(p3, p3.zyx + 31.32);
					return frac((p3.x + p3.y) * p3.z);
				}

				float2 GetPos(float4 vertex) {
					return ComputeScreenPos(vertex).xy * float2(2.,-2.);
				}

				fixed4 frag(v2f IN) : SV_Target
				{

					#ifdef PIXELSNAP_ON
					float2 uv = IN.texcoord.xy * _MainTex_TexelSize.zw;
					uv = round(floor(uv - _MainTex_TexelSize.zw / 2.) / PixelSize) * PixelSize + _MainTex_TexelSize.zw / 2.;
					fixed4 c = SampleSpriteTexture(uv / _MainTex_TexelSize.zw) * IN.color * _Color;
					#else
					float2 uv = GetPos(IN.vertex);
					uv = round(floor(uv - _ScreenParams.xy / 2.) / PixelSize) * PixelSize + _ScreenParams / 2.;
					fixed4 c = SampleSpriteTexture(IN.texcoord.xy) * IN.color * _Color;
					#endif
					
					//float3 pos = (float3(IN.texcoord.xy, _Time.y) * .152 * 9377. + _Time * 1500. + 50.0);
					float3 pos = float3(uv, _Time.y * .3) + _Time.y * 500. + 50.0;
					float a = hash13(pos);

					c.rgb *= a;

					return c;
				}
				ENDCG
			}
		}
}