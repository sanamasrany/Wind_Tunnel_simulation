using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class choosingModel : MonoBehaviour
{
    public modelsdatabais Modelsdb;

    public Text nametext;

    private int selectedoption = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedoption"))
        {
            selectedoption = 0;
        }
        else
        {
            Load();
        }
        Updatemodel(selectedoption);
    }

    public void Nextoption()
    {
        selectedoption++;

        if(selectedoption >= Modelsdb.modelscount)
        {
            selectedoption = 0;
        }

        Updatemodel(selectedoption);
        Save();
    }

    public void Backoption()
    {
        selectedoption--;

        if (selectedoption < 0)
        {
            selectedoption = Modelsdb.modelscount -1;
        }

        Updatemodel(selectedoption);
        Save();
    }
    private void Updatemodel(int selectedoption)
    {
        modelsui mod = Modelsdb.Getmodel(selectedoption);
        nametext.text = mod.modelname;
    }

    private void Load()
    {
        selectedoption = PlayerPrefs.GetInt("selectedoption");
    }
    private void Save()
    {
        PlayerPrefs.SetInt("selectedoption", selectedoption);
    }

    public void changescene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
