using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreManager : MonoBehaviour
{
    public GameObject player;
    int score = 0;
    TextMeshProUGUI myText;
    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        score = player.GetComponent<playerManager>().score;
        myText.text = score.ToString();
    }
}
