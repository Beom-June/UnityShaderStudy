Shader "GenJi/SimpleShader"
{
	// 인스펙터창에서 노출 시킬 영역
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}

		// _Color 가 변수이고, 이것을 똑같게 SubShader안에 똑같게 선언
		_Color("Base Color", Color) = (1,1,1,1)
	}

		// 실제 동작하는 Shader Code.
		// 여러개 작성이되며, 가장 위에 있는 SubShader를 인지하고 실행
		// 그래도 불가능하면 마지막으로 Fallback "SHADER"를 실행함
		SubShader
	{
		// 이 블록도 나눠서 작성 가능
		Pass
		{
			 CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			 float4 _Color;

			struct vertexInput
			{
				// vector4라고 보면 됨
				float4 positionOnObjectSpace : POSITION;
			};

			struct fragmentInput
			{
				float4 positionOnClipSpace : SV_POSITION;
			};

			fragmentInput vert(vertexInput input)
			{
				float4 positionOnClipSpace = UnityObjectToClipPos(input.positionOnObjectSpace);

				fragmentInput output;
				output.positionOnClipSpace = positionOnClipSpace;
				
				return output;
			}

			fixed4 frag(fragmentInput input) : SV_TARGET
			{
				return _Color;
			}
			ENDCG
		}
	}

}
