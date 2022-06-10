using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text knightHealthText;
    [SerializeField] private Text dragonHealthText;
    [SerializeField] private Text coinText;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private Text winText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeKnightHealth(int _damage, int _currentHealth)
    {
        knightHealthText.text = "Knight Health: " + (_currentHealth - _damage).ToString();
    }

    public void ChangeDragonHealth(int _damage, int _currentHealth)
    {
        dragonHealthText.text = "Dragon Health: " + (_currentHealth - _damage).ToString();
    }

    public void ChangeCoinText(int _coins)
    {
        coinText.text = "Coins: " + _coins;
    }

    public void KnightWins()
    {
        winScreen.SetActive(true);
        winText.text = "THE KNIGHT WINS!";
    }

    public void DragonWins()
    {
        winScreen.SetActive(true);
        winText.text = "THE DRAGON WINS!";
    }
}
