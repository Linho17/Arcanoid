using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector2 direction;
    [SerializeField] Vector2 startPosition;
    public Vector2 NextPositionBall
    {
        private set;
        get;
    }
    public Vector2 Direction
    {
        private set
        {
            direction = value.normalized;
            
        
        }
        get
        {
            return direction;
        }
    }
    [SerializeField] private float speed;
    private float Radius => transform.localScale.x;

    private void Start()
    {
        SetStartPosition();


    }

    public void Move()
    {
        
        transform.position = NextPositionBall;

        NextPositionBall = (Vector2)transform.position + Direction * speed * Time.deltaTime;
    }


    public void SetStartPosition()
    {
        transform.position = startPosition;
        Direction = new Vector2(Random.Range(-.5f,.5f), 1f);

        NextPositionBall = (Vector2)transform.position + Direction * speed * Time.deltaTime;
    }

    public void ChangeDirection(Vector2 normalForPlane)
    {

        if (normalForPlane == Vector2.zero) return;
        float angle = 180 - Vector2.SignedAngle (normalForPlane, Direction);


        Vector2 newDirection = RotateByAngle(normalForPlane, angle / 180 * Mathf.PI);

        if (newDirection.x == 0)
        {
            newDirection += Vector2.right * Random.Range(-1f, 1f);
        }
        else if (newDirection.y == 0)
        {
            newDirection += Vector2.up * Random.Range(-1f, 1f);
        }

        if (newDirection == Vector2.zero) Direction = normalForPlane;
        else Direction = newDirection;

        NextPositionBall = (Vector2)transform.position + Direction * speed * Time.deltaTime;
    }

    public Vector2 GetCheckPoint(Vector2 centerPoint, Vector2 _directionPoint)
    {
        
        return centerPoint + _directionPoint.normalized * transform.localScale.x / 2;
    }

   

    public Vector2 TopPoint
    {
        
        get => new Vector2(transform.localPosition.x, transform.localPosition.y + transform.localScale.y / 2);
    }
    public Vector2 LeftPoint
    {
        
        get => new Vector2(transform.localPosition.x - transform.localScale.x / 2, transform.localPosition.y);
    }
    public Vector2 RightPoint
    {
        
        get => new Vector2(transform.localPosition.x + transform.localScale.x / 2, transform.localPosition.y);
    }
    public Vector2 BottomPoint
    {
        
        get => new Vector2(transform.localPosition.x, transform.localPosition.y - transform.localScale.y / 2);
    }


    private Vector2 RotateByAngle(Vector2 _vector, float _angle)
    {
        float x = _vector.x * Mathf.Cos(_angle) - _vector.y * Mathf.Sin(_angle);
        float y = _vector.x * Mathf.Sin(_angle) + _vector.y * Mathf.Cos(_angle);
        return new Vector2(x, y);
    }
}
   
