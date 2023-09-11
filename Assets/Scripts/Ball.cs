using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] Rigidbody _rigid;
    [SerializeField] Renderer _render;

    [SerializeField] float _upForce = 1.0f;
    [SerializeField] float _sideForce = 0.1f;

    // 매개 변수와 관계 없이 활성화시 초기화 로직
    private void OnEnable()
    {
        float _xForce = Random.Range(-_sideForce, _sideForce);
        float _yForce = Random.Range(_upForce * 0.5f, _upForce);
        float _zForce = Random.Range(-_sideForce, _sideForce);

        Vector3 _force = new Vector3(_xForce, _yForce, _zForce);

        _rigid.velocity = _force;

        Invoke(nameof(DeactiveDelay), 5);
    }

    // 들어온 매개변수에 대하여 바꿔야하는 로직
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
