using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform _player;
    public float _zOffset;
    [SerializeField]private float _speed;


    private void LateUpdate()
    {
        if (transform.position.y<_player.position.y)
        {
            Vector3 newPos = new Vector3(0, _player.position.y, _zOffset);         
            transform.position = Vector3.Lerp(transform.position, newPos, _speed);
        }

    }
}