using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboKill : MonoBehaviour
{
    //for get set, that way it will not accidently change number 
    [SerializeField] private float m_combo;
    [SerializeField] private float m_maxCombo = 5;
    [SerializeField] private float m_cooldown;
    [SerializeField] private float m_maxCooldown = 8;

    public int currentCombo = 1; // what combo are we on

    public TextMeshProUGUI comboText;
    private Image comboFiller;   // combo filler, I put private since this is from the mask gameobject

    public Image cooldownFiller; // cooldown filler, I put public since this is not on the mask gameobject (this script is from comboMask not comboCooldown)

    private bool isComboRunning = false; // check if the combo is starting (once reach 2x)

    public float comboMultiplier;

    public void Start()
    {
        comboFiller = GetComponent<Image>();
        GameManager.Instance.OnEnemyKilled += ModifyCombo;

        comboFiller.fillAmount = combo / maxCombo;
    }

    void ModifyCombo(int numberCombo) // I dont see a good use using the int but yeah. DIDN'T USE
    {
        // remember to times the combo number to the score

        combo++;
        
        if (combo >= maxCombo)
        {
            //cooldown = maxCooldown; // set the cooldown back to MAX
            //isComboRunning = true; // set it to true to start countdown

            if (currentCombo < 5) // if the current combo is bigger than 5(HIGHEST)
            {
                //reason why we put here rather outside so only if currentCombo is less than 5 that cooldown could change to MAX
                cooldown = maxCooldown; // set the cooldown back to MAX
                isComboRunning = true; // set it to true to start countdown , don't worry by getting return since with that it will stay true

                combo -= maxCombo; // set combo back to 0 by minus it to maxCombo, safer than auto set to 0
                currentCombo++; // current combo go up
            }
            else
            {
                comboFiller.fillAmount = combo / maxCombo; // reason why is that if I don't call this, then it will not fillamount the combo on the same with max 5/5
                return;                                    //as it will stay on 4 / 5 bar since we return if currentCombo is less than 5.
            }
        }
        comboFiller.fillAmount = combo / maxCombo;
    }

    private void Update()
    {
        if(cooldown > maxCooldown) // just  if the projectile did make the cooldown went higher than the max
        {
            cooldown = maxCooldown;
        }
        if (isComboRunning) // if the combo is running
        {
            cooldown -= Time.deltaTime;    // decrease the time for the combo to run out

            if(cooldown <= 0)
            {
                isComboRunning = false;
                currentCombo = 1;
            }
        }
        cooldownFiller.fillAmount = cooldown / maxCooldown; // put here since we want to fillamount cooldown when its 0 on start

        Combo();
        //comboText.text = currentCombo.ToString(); // put it over here rather than ModifyCombo since this could change back to 1 if timer ran out
        // Rather than combo number, I change to D-S, put it on Combo function since Combo() do call from Update;
    }

    public void Combo() // the combo that has the multiplier score
    {
        if (currentCombo == 1)      // IF WANT TO SET Combo Cooldown, or MaxCombo then this is the place
        {
            comboMultiplier = 1;

            comboText.text = "D";

            comboText.color = new Color32(128, 128, 128, 255); // GRAY
        }
        else if (currentCombo == 2)
        {
            comboMultiplier = 1.5f;

            comboText.text = "C";

            comboText.color = new Color32(0, 255, 0, 255); // GREEN
        }
        else if (currentCombo == 3)
        {
            comboMultiplier = 2;

            comboText.text = "B";

            comboText.color = new Color32(0, 0, 255, 255); // BLUE
        }
        else if (currentCombo == 4)
        {
            comboMultiplier = 2.5f;

            comboText.text = "A";

            comboText.color = new Color32(255, 0, 0, 255); // RED
        }
        else if (currentCombo == 5)
        {
            comboMultiplier = 3;

            comboText.text = "S";

            comboText.color = new Color32(255, 195, 0, 255); // GOLD
        }
    }


    // GET and SET
    public float combo
    {
        get { return m_combo; }
        set { m_combo = value; }
    }

    public float maxCombo
    {
        get { return m_maxCombo; }
        set { m_maxCombo = value; }
    }

    public float cooldown
    {
        get { return m_cooldown; }
        set { m_cooldown = value; }
    }

    public float maxCooldown
    {
        get { return m_maxCooldown; }
        set { m_maxCooldown = value; }
    }
    // ----------------------------------
}
