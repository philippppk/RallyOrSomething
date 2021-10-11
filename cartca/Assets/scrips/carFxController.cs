using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class carFxController : MonoBehaviour
{
    //all the cool FX and stuff and dying for some reason

    public GameObject explosionFX;

    public TMP_Text deathText;
    public TMP_Text restartText;
    private bool dead = false;

    [SerializeField] bool inWater = false;
    [SerializeField] Animator deathAnim;
    [SerializeField] Animator restartAnim;
    // Start is called before the first frame update
    void Awake()
    {
        deathText.gameObject.SetActive(false);
        restartText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -3 && inWater == false)
        {
            inWater = true;
            waterboom();
            showDeathText();
        }
    }

    public void showDeathText()
    {
        deathText.gameObject.SetActive(true);
        restartText.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void waterboom()
    {
        if (dead == false)
        {
            Instantiate(explosionFX, transform.position, Quaternion.identity);
            dead = true;
        }
     
    }
}
