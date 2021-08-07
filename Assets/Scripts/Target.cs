using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Target : MonoBehaviour
{
    [SerializeField] private BoxObject _boxObject;
    private Game game;
    private void Start()
    {
        game = Game.game;
        game.AddTarget(_boxObject);
    }
    public void DestroyTarget()
    {
        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        game.RemoveTarget(_boxObject);
    }


}
