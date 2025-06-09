using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadGameButtonBinder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => GameManager.GetInstance()?.LoadGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
