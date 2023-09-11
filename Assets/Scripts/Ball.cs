using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody _rigid;
    [SerializeField] Renderer _render;

    [SerializeField] float _upForce = 1.0f;
    [SerializeField] float _sideForce = 0.1f;

    // �Ű� ������ ���� ���� Ȱ��ȭ�� �ʱ�ȭ ����
    private void OnEnable()
    {
        float _xForce = Random.Range(-_sideForce, _sideForce);
        float _yForce = Random.Range(_upForce * 0.5f, _upForce);
        float _zForce = Random.Range(-_sideForce, _sideForce);

        Vector3 _force = new Vector3(_xForce, _yForce, _zForce);

        _rigid.velocity = _force;

        Invoke(nameof(DeactiveDelay), 5);
    }

    // ���� �Ű������� ���Ͽ� �ٲ���ϴ� ����
    public void Setup(Color _color)
    {
        _render.material.color = _color;
    }

    void DeactiveDelay() => gameObject.SetActive(false);

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
