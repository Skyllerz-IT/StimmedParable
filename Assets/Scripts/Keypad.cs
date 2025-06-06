using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    public GameObject player;
    public GameObject keypadOBJ;
    public GameObject hud;
    
    public GameObject animateOBJ;
    public Animator ANI;

    public TMP_Text textOBJ;
    public string answer = "1948";

    [SerializeField] private SoundPlayer correctSound;
    [SerializeField] private SoundPlayer wrongSound;
    [SerializeField] private SoundPlayer buttonSound;
    
    public bool animate;

    void Start()
    {
        keypadOBJ.SetActive(false);
    }

    public void Number(int number)
    {
        if (textOBJ.text == "Wrong")
        {
            textOBJ.text = "";
        }
        textOBJ.text += number.ToString();
        buttonSound.PlayClickSound();
    }

    public void Execute()
    {
        if (textOBJ.text == answer)
        {
            correctSound.PlayClickSound();
            textOBJ.text = "Correct";
            keypadOBJ.SetActive(false);
            hud.SetActive(true);
            Destroy(keypadOBJ, 1.5f);
        }
        else
        {
            wrongSound.PlayClickSound();
            textOBJ.text = "Wrong";
        }
    }

    public void Clear()
    {
        textOBJ.text = "";
        buttonSound.PlayClickSound();
    }

    public void Exit()
    {
        keypadOBJ.SetActive(false);
        hud.SetActive(true);
        
    }

    public void Update()
    {
        if (textOBJ.text == "Correct" && animate)
        {
            ANI.SetBool("animate", true);
            print("isOpen");
        }

        if (keypadOBJ.activeInHierarchy)
        {
            hud.SetActive(false);
        }
    }
}
