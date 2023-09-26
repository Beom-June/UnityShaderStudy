using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBRScript : MonoBehaviour
{

    public Renderer _rd;
    //public Material _mat;                     //  안전하게 하려면 Renderer 건들일 것
    public Texture2D _t2d;
    public Texture2DArray _t2dArray;            //  쓰고 싶으면 AssetStore에서 Asset 쓰는 것을 추천
    public Texture3D _t3d;                      //  안씀
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
