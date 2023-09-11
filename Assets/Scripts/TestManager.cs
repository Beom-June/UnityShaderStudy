using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 키입력 받는 스크립트
public class TestManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            // 맨 뒤에 회전값 추가 가능 (생략시  Quaternion.identity)
            //GameObject _ball = ObjectPooler.SpawnFromPool("Ball", Vector2.zero);
            //_ball.GetComponent<Ball>().Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1));

            // 제네릭 형태
            ObjectPooler.SpawnFromPool<Ball>("Ball", Vector2.zero).Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1));
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ObjectPooler.SpawnFromPool<Cube>("Cube", Vector2.zero).Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1));
        }

        // 비활성화
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            List<GameObject> _balls = ObjectPooler.GetAllPools("Ball");
            _balls.ForEach(x => x.SetActive(false));
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            List<GameObject> _cubes = ObjectPooler.GetAllPools("Cube");
            _cubes.ForEach(x => x.SetActive(false));
        }
    }
}
