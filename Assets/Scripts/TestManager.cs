using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ű�Է� �޴� ��ũ��Ʈ
public class TestManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            // �� �ڿ� ȸ���� �߰� ���� (������  Quaternion.identity)
            //GameObject _ball = ObjectPooler.SpawnFromPool("Ball", Vector2.zero);
            //_ball.GetComponent<Ball>().Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1));

            // ���׸� ����
            ObjectPooler.SpawnFromPool<Ball>("Ball", Vector2.zero).Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1));
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ObjectPooler.SpawnFromPool<Cube>("Cube", Vector2.zero).Setup(Random.ColorHSV(0, 1, 0.5f, 1, 1, 1));
        }

        // ��Ȱ��ȭ
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
