using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _time;
    [SerializeField] private Transform _savePointTrans;
    [SerializeField] private Animator _animator;
    public Rigidbody _rb;
    private Vector3 _savePoint;
    public Action _onJump;
    public float _jumpForce;
    public float _jumpForceWall;
    public float _speed = 2f;
    private float width;
    private float height;
    public bool _isGrounded;
    public bool _isGroundedWall;

    void Awake()
    {
        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
    }

    private void FixedUpdate()
    {
        if (GameController.instance.IsGame == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || (Input.touchCount == 1 && CheckTouch()))
        {
            if (_isGrounded)
            {
                _rb.velocity = transform.up * _jumpForce;
                _isGrounded = false;
                transform.parent = null;
                _rb.useGravity = true;
                _player._audioSource.PlayOneShot(_player._audioJump);
                _onJump?.Invoke();
            }

            if (_isGroundedWall)
            {
                _rb.velocity = (Vector3.up + transform.up) * _jumpForceWall;
                _isGroundedWall = false;
                transform.parent = null;
                _rb.useGravity = true;
                _player._audioSource.PlayOneShot(_player._audioJump);
                _onJump?.Invoke();
            }
        }

        if (_isGroundedWall) //Скольжение в низ по стене
        {
            float vy = _rb.velocity.y - _speed * Time.deltaTime;
            if (vy < _speed)
            {
                vy = _speed;
            }
            _rb.velocity = new Vector3(0, _speed, 0);
        }
    }

    public bool CheckTouch()
    {
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPos = Input.GetTouch(0).position;
            if (touchPos.y < height * 1.6f)
            {
                return true;
            }
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Planeta planeta = collision.gameObject.GetComponent<Planeta>();
        if (planeta != null)
        {
            _animator.SetBool("Open_Anim", true);
            _isGrounded = true;
            _rb.useGravity = false;
            if (planeta._isFinalPlanet)
            {
                Menu.instance.WinGame();
            }
            transform.parent = collision.transform;
            Quaternion rotation = Quaternion.FromToRotation(-transform.up, collision.transform.position - transform.position);
            transform.rotation = rotation * transform.rotation;
            _rb.velocity = Vector3.zero;
        }


        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            if (obstacle._isWall)
            {
                _animator.SetBool("Open_Anim", true);
                _isGroundedWall = true;
                _rb.useGravity = false;
                Quaternion rotation = Quaternion.identity;
                if (transform.position.x > 0)
                {
                    rotation = Quaternion.FromToRotation(-transform.up, Vector3.right);
                }
                else
                {
                    rotation = Quaternion.FromToRotation(-transform.up, Vector3.left);
                }
                transform.rotation = rotation * transform.rotation;
                _rb.velocity = Vector3.zero;
            }

            if (obstacle._asteroid)
            {
                _player._audioSource.PlayOneShot(_player._audioDamageAsteroid);
                Destroy(collision.gameObject);
                _player.Damage();
                StartCoroutine(ReturnToSavePoint());
            }
        }
    }

    IEnumerator ReturnToSavePoint()
    {
        float t = 0;
        float timeLerp = 0;
        Vector3 endPoint = transform.position;
        while (t < 1)
        {
            t = Mathf.Clamp01(timeLerp / _time);
            transform.position = Vector3.Lerp(endPoint, _savePoint, t);
            timeLerp += Time.deltaTime;
            yield return null; //ждем обновление кадра
            //yield return new WaitForSeconds(1f);
        }
        transform.position = _savePoint;
    }

    private void OnCollisionExit(Collision collision)
    {
        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();
        if (obstacle != null)
        {
            if (obstacle._isWall)
            {
                _animator.SetBool("Open_Anim", false);
                _isGroundedWall = false;
                _savePoint = _savePointTrans.transform.position;             
            }
        }
        Planeta planeta = collision.gameObject.GetComponent<Planeta>();
        if (planeta != null)
        {
            _animator.SetBool("Open_Anim", false);
            _savePoint = _savePointTrans.transform.position;
            _isGrounded = false;
            _rb.useGravity = true;
        }
    }
}
