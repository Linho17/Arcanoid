using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float speed;
  

    private float y;
    private float x;

    private void Start()
    {
        y = transform.localPosition.y;
    }

    public void MoveHorizontal(float _direction,float _borderLeft, float _borderRight)
    {
        _borderLeft += transform.localScale.x / 2;
        _borderRight -= transform.localScale.x / 2;

        x = transform.localPosition.x; 
        x = Mathf.Clamp(x + _direction * speed * Time.deltaTime, _borderLeft, _borderRight);

        transform.localPosition = new Vector3(x,y);

    }

    public void Rotate(float _direction)
    {
        if (_direction == 0) return;

        float z = transform.localRotation.eulerAngles.z;
      
        z = Mathf.RoundToInt(z + (Vector2.right *_direction).normalized.x);
        if (z > 0 && z<180)
            z = Mathf.Clamp(z, 0, 45);
        else if (z < 360 && z>180 )
            z = Mathf.Clamp(z, 315, 360);
        transform.localRotation = Quaternion.Euler(0, 0, z);
    }
}
