Shader "GenJi/SimpleShader"
{
	// �ν�����â���� ���� ��ų ����
	Properties
	{
		//_MainTex ("Texture", 2D) = "white" {}

		// _Color �� �����̰�, �̰��� �Ȱ��� SubShader�ȿ� �Ȱ��� ����
		_Color("Base Color", Color) = (1,1,1,1)
	}

		// ���� �����ϴ� Shader Code.
		// ������ �ۼ��̵Ǹ�, ���� ���� �ִ� SubShader�� �����ϰ� ����
		// �׷��� �Ұ����ϸ� ���������� Fallback "SHADER"�� ������
		SubShader
	{
		// �� ��ϵ� ������ �ۼ� ����
		Pass
		{
			 CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			 float4 _Color;

			struct vertexInput
			{
				// vector4��� ���� ��
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
