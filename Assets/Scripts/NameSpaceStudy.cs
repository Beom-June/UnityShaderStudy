using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GenJi;

namespace GenJi
{
    namespace Test
    {
        public class Test1
        {
            int _like;
            public void SetLike(int value)
            {
                _like = value;
            }

            public bool IsLike()
            {
                return _like != 0;
            }
        }
    }
}

public class NameSpaceStudy : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
