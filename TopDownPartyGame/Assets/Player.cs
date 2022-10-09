using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum State
    {
        None,
        Active,
        Dead
    }
    public float health = 100;
    public State state = State.None;
    public int playerIndex;
    string startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton = $"Start_{playerIndex}";
    }
    public void OnPlay()
    {
        if (state == State.None)
        {
            Debug.Log("on play");
               health = 100;
                state = State.Active;
        }
    }
    // Update is called once per frame
    void Update()
    {
       
    }
}
