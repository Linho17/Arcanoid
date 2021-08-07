using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxObject : MonoBehaviour
{
    private float checkDistance = 0.5f;
    public UnityEvent onCollision;
    public Vector2 TopLeftPoint
    {
        get
        {
            Vector2 vector = RotateByAngle(new Vector2(-transform.localScale.x / 2, transform.localScale.y / 2), Angle);
            return new Vector2(transform.position.x + vector.x, transform.position.y + vector.y);
        }
    }

    public Vector2 TopRightPoint
    {
        get
        {
            Vector2 vector = RotateByAngle(new Vector2(transform.localScale.x / 2, transform.localScale.y / 2), Angle);
            return new Vector2(transform.position.x + vector.x, transform.position.y + vector.y);
        }
    }

    public Vector2 BottomLeftPoint
    {
        get
        {
            Vector2 vector = RotateByAngle(new Vector2(-transform.localScale.x / 2, -transform.localScale.y / 2), Angle);
           
            return new Vector2(transform.position.x + vector.x, transform.position.y + vector.y);
        }
    }

    public Vector2 BottomRightPoint
    {
        get
        {
            Vector2 vector =  RotateByAngle(new Vector2(transform.localScale.x / 2, -transform.localScale.y / 2), Angle); 
            return new Vector2(transform.position.x + vector.x, transform.position.y + vector.y);
        }
    }

    private Vector2 RotateByAngle(Vector2 _vector, float _angle)
    {
        float x = _vector.x * Mathf.Cos(_angle) - _vector.y * Mathf.Sin(_angle);
        float y = _vector.x * Mathf.Sin(_angle) + _vector.y * Mathf.Cos(_angle);
        return new Vector2(x, y);
    }

    private float Angle=> transform.localRotation.eulerAngles.z / 180 * Mathf.PI;
    public override string ToString()
    {
        return $"TL: {TopLeftPoint}; TR: {TopRightPoint}; BL: {BottomLeftPoint}; BR: {BottomRightPoint};";
    }



    private Vector2 NormalToPlane(Vector2 PointA, Vector2 PointB)
    {
       
        

        return RotateByAngle((PointB - PointA).normalized, Mathf.PI / 2);
    }


    public void CheckCollision(Ball _ball)
    {
        Vector2 normalVector  = CheckPlaneWithTwoPoint(_ball,TopLeftPoint,TopRightPoint);

        
        normalVector += CheckPlaneWithTwoPoint(_ball, BottomRightPoint, BottomLeftPoint);
        

        normalVector += CheckPlaneWithTwoPoint(_ball, TopRightPoint, BottomRightPoint);
        
        normalVector += CheckPlaneWithTwoPoint(_ball, BottomLeftPoint, TopLeftPoint);






       


        _ball.ChangeDirection(normalVector);

        
    }

    private Vector2 CheckPlaneWithTwoPoint(Ball _ball, Vector2 _pointA, Vector2 _pointB)
    {


        if (IsSegment(_ball, _pointA, _pointB))

            if (CheckCollisionLineAndPoint(_ball.GetCheckPoint(_ball.transform.position,Vector2.up), _ball.GetCheckPoint(_ball.NextPositionBall, Vector2.up), _pointA, _pointB)
                || CheckCollisionLineAndPoint(_ball.GetCheckPoint(_ball.transform.position, Vector2.down), _ball.GetCheckPoint(_ball.NextPositionBall,Vector2.down), _pointA, _pointB)
                || CheckCollisionLineAndPoint(_ball.GetCheckPoint(_ball.transform.position, Vector2.right), _ball.GetCheckPoint(_ball.NextPositionBall, Vector2.right), _pointA, _pointB)
                || CheckCollisionLineAndPoint(_ball.GetCheckPoint(_ball.transform.position, Vector2.left), _ball.GetCheckPoint(_ball.NextPositionBall, Vector2.left), _pointA, _pointB)
                || CheckCollisionLineAndPoint(_ball.GetCheckPoint(_ball.transform.position, Vector2.up+ Vector2.left), _ball.GetCheckPoint(_ball.NextPositionBall, Vector2.up+ Vector2.left), _pointA, _pointB)
                || CheckCollisionLineAndPoint(_ball.GetCheckPoint(_ball.transform.position, Vector2.down+ Vector2.left), _ball.GetCheckPoint(_ball.NextPositionBall, Vector2.down+ Vector2.left), _pointA, _pointB)
                || CheckCollisionLineAndPoint(_ball.GetCheckPoint(_ball.transform.position, Vector2.right + Vector2.up), _ball.GetCheckPoint(_ball.NextPositionBall, Vector2.right+ Vector2.up), _pointA, _pointB)
                || CheckCollisionLineAndPoint(_ball.GetCheckPoint(_ball.transform.position, Vector2.right + Vector2.down), _ball.GetCheckPoint(_ball.NextPositionBall, Vector2.right + Vector2.down), _pointA, _pointB)) 
            {
                onCollision?.Invoke();
                return NormalToPlane(_pointA, _pointB);
            }

        return Vector2.zero;
    }

    private bool IsSegment(Ball _ball, Vector2 _pointA, Vector2 _pointB)
    {
        if ((_ball.RightPoint.x > _pointA.x && _ball.LeftPoint.x < _pointB.x) || (_ball.RightPoint.x > _pointB.x && _ball.LeftPoint.x < _pointA.x) ||
            (_pointA.x == _pointB.x && (Mathf.Abs(_ball.RightPoint.x - _pointB.x) <= checkDistance || Mathf.Abs(_ball.LeftPoint.x - _pointB.x) <= checkDistance)))
            if (_ball.Direction.y < 0)
            {
                if ((_ball.BottomPoint.y < _pointA.y && _ball.BottomPoint.y > _pointB.y) || (_ball.BottomPoint.y < _pointB.y && _ball.BottomPoint.y > _pointA.y)
                || (_pointA.y == _pointB.y && Mathf.Abs(_ball.BottomPoint.y - _pointB.y) <= checkDistance))
                    return true;
            }
            else
                if ((_ball.TopPoint.y < _pointA.y && _ball.TopPoint.y > _pointB.y) || (_ball.TopPoint.y < _pointB.y && _ball.TopPoint.y > _pointA.y)
                || (_pointA.y == _pointB.y && Mathf.Abs(_ball.TopPoint.y - _pointB.y) <= checkDistance))
                return true;
        return false;
    }







    private bool CheckCollisionLineAndPoint(Vector2 _pointCheck, Vector2 _pointCheckNext, Vector2 _pointLineA, Vector2 _pointLineB)
    {

        float position = (_pointLineA.y - _pointLineB.y) * _pointCheck.x + (_pointLineB.x - _pointLineA.x) * _pointCheck.y + (_pointLineB.y * _pointLineA.x - _pointLineB.x * _pointLineA.y);
        float positionNext = (_pointLineA.y -_pointLineB.y) * _pointCheckNext.x + (_pointLineB.x - _pointLineA.x) * _pointCheckNext.y + (_pointLineB.y * _pointLineA.x - _pointLineB.x * _pointLineA.y);
       
        return position / Mathf.Abs(position) > 0 && positionNext / Mathf.Abs(positionNext) < 0;

    }
}
