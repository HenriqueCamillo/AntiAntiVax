using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    [SerializeField] Image lifeBar;
    [SerializeField] GameObject completeLifeBar;
    [SerializeField] float barSpeed;
    [SerializeField] int life;
    int maxLife;
    float newFillAmount;
    bool willDie = false;

    void Start()
    {
        maxLife = life;
    
        GameManager.instance.OnGameStart += Enable;
        GameManager.instance.OnGameEnd += Reset;
    }

    void Reset()
    {
        CancelInvoke("MoveBar");
        lifeBar.fillAmount = 1;
        life = maxLife;
        completeLifeBar.SetActive(false);
        willDie = false;
    }

    void Enable()
    {
        completeLifeBar.SetActive(true);
    }

    public void TakeDamage(int level)
    {
        
        life -= level;
        if (life < 0)
            life = 0;
            
        newFillAmount = (float)life/(float)maxLife;
        CancelInvoke("MoveBar");
        InvokeRepeating("MoveBar", 0, Time.deltaTime);

        if (life <= 0)
            willDie = true;
            // GameManager.instance.GameOver();
        
    }


    void MoveBar()
    {
        lifeBar.fillAmount = Mathf.Lerp(lifeBar.fillAmount, newFillAmount, barSpeed);
        if (Mathf.Abs(lifeBar.fillAmount - newFillAmount) < 0.01)
        {
            if (willDie)
            {
                willDie = false;
                GameManager.instance.GameOver();
            }

            CancelInvoke("MoveBar");
        }
    }

}
