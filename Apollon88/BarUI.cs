using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;     // this is to call the amazing textmeshpro

public class BarUI : MonoBehaviour
{


    public TextMeshProUGUI scoreText;

    public ComboKill comboKill;

    public int playerScore = 0; // player score

    private void Start()
    {                       
        GameManager.Instance.OnEnemyKilled += ModifyScore;
        ModifyScore(0);
        
        scoreText.text ="0"; // set the score to 0, since it will not be 0 (we dont use numberKilled)
    }

    void ModifyScore(int numberKilled)
    {
        //float _enemyScore = numberKilled;
        float killScore = 1000 * comboKill.comboMultiplier; // default score is 1000 with the number of combo multiplier we got, enemy deal damage? multiplier 0
        playerScore = playerScore + (int)killScore; // set it to player score to do ++ since it will stays the same number without ++

        scoreText.text = playerScore.ToString(); // set it to int just for safety...
    }
}
