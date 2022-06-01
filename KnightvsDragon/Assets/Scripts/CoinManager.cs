using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] private Vector2[] coinSpawnPositions;
    [SerializeField] private GameObject coinObject;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < coinSpawnPositions.Length; i++)
        {
            GameObject newCoin = Instantiate(coinObject, coinSpawnPositions[i], Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
