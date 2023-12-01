using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score ScoreInstance;
    public int score { get; set; } = 0; 

    private void Awake()
    {
        if (ScoreInstance == null)
        {
            ScoreInstance = this;
            DontDestroyOnLoad(ScoreInstance);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
