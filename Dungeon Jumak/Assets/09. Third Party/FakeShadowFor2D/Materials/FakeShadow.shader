Shader "TheAnh/FakeShadow"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        _ShadowColor ("Shader Color", Color) = (0,0,0,0.5)
        _Direction ("Shadow Direction", Range(-1,1)) = 0
		[MaterialToggle] _Tilt ("Tilt On Screen Position", Float) = 0
        _Height ("Shadow Height", Range(-1.5, 1.5)) = 1
	    [HideInInspector] _SpriteSize ("Sprite Size", Vector) = (0,0,0,0)
	    [HideInInspector] _SpriteSizeRel ("Sprite Size", Vector) = (0,0,0,0)
		[HideInInspector] _SpritePivot ("Sprite Pivot", Vector) = (0,0,0,0)
		_Offset ("Offset", Vector) = (0,0,0,0)

        [MaterialToggle] _Soft ("Soft Shadow", Float) = 0
        _Quality ("Soft Shadow Quality", Range(1,4 )) = 1
        _Intensity ("Softness", Range(0,0.05)) = 0.01

        [MaterialToggle] _Fade ("Shadow Fading", Float) = 0
        _FadeAmount ("Fading Intensity", Range(0,1)) = 0.5

		[MaterialToggle] _OnlyShadow ("Only Shadow", Float) = 0

		[MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0

	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		Blend One OneMinusSrcAlpha

        Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
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
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
            float4 _Offset;
            float _Direction;
            float _Height;
			float4 _SpriteSizeRel;
            float _Soft;
			float _Tilt;
			float2 _SpritePivot;
			float4 _MainTex_TexelSize;
			float2 _SpriteSize;


			v2f vert(appdata_t IN)
			{
				v2f OUT;

				float s = 1;
				if (_Soft)
				s = 2;

				float xpos = 0;
				float ypos = 0;
				if (_Tilt)
				{
					float4 clipspace = UnityObjectToClipPos(float4(0,0,0,0));
					xpos = (clipspace.x + 0.5)/3;
					ypos = (clipspace.y + 0.5)/5;
				}

				float4 v = IN.vertex - float4((0.5 -_SpritePivot.x)*_SpriteSizeRel.x, (0.5 -_SpritePivot.y)*_SpriteSizeRel.y ,0,0);
				float4 vertex = float4(v.x, (v.y*(_Height+ypos)) - (_SpriteSizeRel.y - _SpriteSizeRel.y*(_Height+ypos))/(2*s), v.z, v.w);
				float4 vercoord = vertex + float4((v.y + _SpriteSizeRel.y/(2*s))*(_Direction+xpos) ,0,0,0) + _Offset/s;
				vercoord += float4((0.5 -_SpritePivot.x)*_SpriteSizeRel.x/s,(0.5 -_SpritePivot.y)*_SpriteSizeRel.y/s,0,0);
				OUT.vertex = UnityObjectToClipPos(vercoord*s);
				
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
            float4 _ShadowColor;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}
            float _Intensity;
            float _Quality;
            float _Fade;
            float _FadeAmount;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;

				

                if (_Soft == 1)
				{
                    float size = round(_Quality);
                    float sum = 0;
                    float count = 0;

					float lx = _SpriteSize.x / _MainTex_TexelSize.z;
					float x = IN.texcoord.x - (IN.texcoord.x % lx);
					float x_end = x + lx;

					float ly = _SpriteSize.y / _MainTex_TexelSize.w;
					float y = IN.texcoord.y - (IN.texcoord.y % ly);
					float y_end = y + ly;

					float2 uv = IN.texcoord * 2 - float2(x + lx/2, y + ly/2);

					float intenseX = _Intensity*(lx/_SpriteSizeRel.x);
					float intenseY = _Intensity*(ly/_SpriteSizeRel.y);

                    for (int i= -size; i <= size; i++)
                    {
                        for (int j= -size; j <= size; j++)
                        {
							// 192F57
							float a = uv.x + i*intenseX;
							float b = uv.y + j*intenseY;

							if (a >= x && a < x_end && b >= y && b < y_end)
							sum += (SampleSpriteTexture(uv + float2(i*intenseX,j*intenseY)) * IN.color).a;
							// else sum+=1;
							
							count++;
                        }
                    }
					
					c.a = (sum/count) * _ShadowColor.a;

					float n = frac(IN.texcoord.y/(_SpriteSize.y/_MainTex_TexelSize.w));
					if (_Fade == 1 && n > 0.25)
					{
						n = n*2 - 0.5;
						c.a -= n *_FadeAmount;
					}

					c.rgb = _ShadowColor.rgb * c.a;
                }
                else
				{
					
					if (_Fade == 1)
					c.a -= frac(IN.texcoord.y/(_SpriteSize.y/_MainTex_TexelSize.w))*_FadeAmount;

					c.a *= _ShadowColor.a;
                    c.rgb = _ShadowColor.rgb * c.a;
                }

				return c;
			}
		ENDCG
		}

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
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
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			float _OnlyShadow;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				float4 vertex = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			float clam(float a, float b, float c)
		    {
				if (a < b) return b;
				else if (a > c) return c;
				else return a;
		    }

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = 0;
				if (!_OnlyShadow)
				{
					c = SampleSpriteTexture(IN.texcoord) * IN.color;
					c.rgb *= c.a;
				}
				else c = fixed4(0,0,0,0);
				return c;
			}
		ENDCG
		}
	}
    
}