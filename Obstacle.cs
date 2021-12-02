using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] float _rotation;
    [SerializeField] float _rotationSpeed;
    [SerializeField] public GameObject _VFX;
    [SerializeField] public GameObject _enemyOff;
    [SerializeField] private bool _isRotate;
    public Vector3 _toRot;
    public Vector3 _toRotDistance;
    public GameObject _pivotObject;
    public bool _isWall;
    public bool _asteroid;
    public bool _enemy;

    private void FixedUpdate()
    {
        if (_isRotate)
        {
            transform.Rotate(_toRot * _rotation);
            transform.RotateAround(_pivotObject.transform.position, _toRotDistance, _rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            if (_enemy)
            {
                Player.instance._audioSource.PlayOneShot(Player.instance._audioDamage);
                _VFX.SetActive(true);
                StartCoroutine(DelayDestroy());
                _enemyOff.SetActive(false);
                player.Damage();
            }
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        Debug.Log("Удалить врага");
    }
}
