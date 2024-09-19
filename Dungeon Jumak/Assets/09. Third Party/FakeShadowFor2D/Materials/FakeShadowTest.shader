Shader "TheAnh/FakeShadowTest"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
        _ShadowColor ("Shader Color", Color) = (0,0,0,0.5)
        _Direction ("Shadow Direction", Range(-1,1)) = 0
        _Height ("Shadow Height", Range(0, 1.5)) = 1
	    [HideInInspector] _SpriteSize ("Sprite Size", Vector) = (0,0,0,0)
	    [HideInInspector] _SpriteSizeRel ("Sprite Size", Vector) = (0,0,0,0)
		[HideInInspector] _SpritePivot ("Sprite Pivot", Vector) = (0,0,0,0)
		_Offset ("Offset", Vector) = (0,0,0,0)

        [MaterialToggle] _Fade ("Shadow Fading", Float) = 0
        _FadeAmount ("Fading Intensity", Range(0,1)) = 0.5

		[MaterialToggle] _OnlyShadow ("Only Shadow", Float) = 0

		[MaterialToggle] PixelSnap ("Pixel Snap", Float) = 0
		[HideInInspector] _DeltaY ("Delta Y", Float) = 0
		_Test ("Test", Float) = 0

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
		Blend SrcAlpha OneMinusSrcAlpha

		Pass 
		{
			Stencil 
			{
				Ref 1
				Comp equal
			}
			
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			sampler2D _MainTex;
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

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

                #if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
                #endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				return color;
			}
			
			fixed4 _Color;
            float4 _Offset;
            float _Direction;
            float _Height;
			float4 _SpriteSizeRel;
			float2 _SpritePivot;
			float4 _MainTex_TexelSize;
			float2 _SpriteSize;
			float _DeltaY;
			float _Test;
			

			v2f vert(appdata_t IN)
			{
				v2f OUT;

				float dy = _DeltaY * _Direction / _Height;

				float4 vertex = IN.vertex - float4((0.5 -_SpritePivot.x)*_SpriteSizeRel.x, (0.5 -_SpritePivot.y)*_SpriteSizeRel.y,0,0);
				vertex.y =  (vertex.y*_Height) - (_SpriteSizeRel.y - _SpriteSizeRel.y*(_Height))/2;
				vertex.x += dy + _SpritePivot.y;
				vertex += float4((0.5 -_SpritePivot.x)*_SpriteSizeRel.x, (0.5 -_SpritePivot.y)*_SpriteSizeRel.y,0,0) + _Offset;
				OUT.vertex = UnityObjectToClipPos(vertex);
				
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
            float4 _ShadowColor;
			
            float _Fade;
            float _FadeAmount;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
               
				if (_Fade == 1)
				c.a -= frac(IN.texcoord.y/(_SpriteSize.y/_MainTex_TexelSize.w))*_FadeAmount;
				c.a *= _ShadowColor.a;
				c.rgb = _ShadowColor.rgb * c.a;
					
				return c;
			}
		ENDCG
		}

        Pass 
		{
			ZTest Less
			Stencil 
			{
				Ref 1
				Comp notequal
			}
			
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
			float2 _SpritePivot;
			float4 _MainTex_TexelSize;
			float2 _SpriteSize;


			v2f vert(appdata_t IN)
			{
				v2f OUT;

				float4 v = IN.vertex - float4((0.5 -_SpritePivot.x)*_SpriteSizeRel.x, (0.5 -_SpritePivot.y)*_SpriteSizeRel.y,0,0);
				float4 vertex = float4(v.x, (v.y*(_Height)) - (_SpriteSizeRel.y - _SpriteSizeRel.y*(_Height))/2, v.z, v.w);
				vertex += float4((v.y + _SpriteSizeRel.y/2)*(_Direction) ,0,0,0) + _Offset;
				vertex += float4((0.5 -_SpritePivot.x)*_SpriteSizeRel.x, (0.5 -_SpritePivot.y)*_SpriteSizeRel.y,0,0);
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
            float _Fade;
            float _FadeAmount;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				
				if (_Fade == 1)
				c.a -= frac(IN.texcoord.y/(_SpriteSize.y/_MainTex_TexelSize.w))*_FadeAmount;
				c.a *= _ShadowColor.a;
                c.rgb = _ShadowColor.rgb * c.a;
				if (!(c.a > 0 ))discard;

				return c;
			}
		ENDCG
		}

		Pass //base
		{
        Stencil 
        {
			Ref 1
			Comp Always
			Pass Replace
        }
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
				c = SampleSpriteTexture(IN.texcoord) * IN.color;
				if (!(c.a > 0)) discard;
				if (_OnlyShadow)
				c.rgba = 0;
				else c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
    
}