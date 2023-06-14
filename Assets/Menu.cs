using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject plane;
    public Image menu;
    private Button button;
    public TextMeshProUGUI highscore;
    public SpaceShip spaceship;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        highscore.text = PlayerPrefs.GetFloat("MaxLevel").ToString();
        // spaceship.maxLevel.ToString();
    }

    private void OnClick()
    {

        menu.gameObject.SetActive(false);
        plane.gameObject.SetActive(true);

    }
}
