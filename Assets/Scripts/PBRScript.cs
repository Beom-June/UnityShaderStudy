using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBRScript : MonoBehaviour
{

    public Renderer _rd;
    //public Material _mat;                     //  �����ϰ� �Ϸ��� Renderer �ǵ��� ��
    public Texture2D _t2d;
    public Texture2DArray _t2dArray;            //  ���� ������ AssetStore���� Asset ���� ���� ��õ
    public Texture3D _t3d;                      //  �Ⱦ�
    public Cubemap _cubeMap;

    void Start()
    {
        _rd.material.SetFloat("_Vector1", 1);
        _rd.material.SetInt("_Vector1", 0);

        _rd.material.SetVector("_Vector2", new Vector2(0, 1));
        _rd.material.SetVector("_Vector3", new Vector3(0, 1, 2));
        _rd.material.SetVector("_Vector4", new Vector4(0, 1, 2, 3));

        _rd.material.SetColor("_Color", new Color(1, 1, 1, 1));

        _rd.material.SetTexture("_Texture2D",_t2d);
        _rd.material.SetTexture("_Texture2DArray", _t2dArray);
        _rd.material.SetTexture("_Texture3D", _t3d);
        _rd.material.SetTexture("_Cubemap", _cubeMap);

        _rd.material.SetInt("_Boolean", 1);
    }
}
