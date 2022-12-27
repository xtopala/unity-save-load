using UnityEngine;
using CodeMonkey;
using System.IO;

public class GameHandler : MonoBehaviour
{
    private const string SAVE_SEPARATOR = "#SAVE-VALUE#";

    [SerializeField] private GameObject unitGameObject;
    private IUnit unit;

    private void Awake()
    {
        unit = unitGameObject.GetComponent<IUnit>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    private void SavePlayerPrefs()
    {
        Vector3 playerPosition = unit.GetPosition();
        int goldAmount = unit.GetGoldAmount();

        PlayerPrefs.SetFloat("playerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("playerPositionY", playerPosition.y);
        PlayerPrefs.SetInt("goldAmount", goldAmount);

        PlayerPrefs.Save();
    }

    private void Save()
    {
        Vector3 playerPosition = unit.GetPosition();
        int goldAmount = unit.GetGoldAmount();

        
        string[] contents = new string[]
        {
            ""+goldAmount,
            ""+playerPosition.x,
            ""+playerPosition.y
        };
        string saveString = string.Join(SAVE_SEPARATOR, contents);
        File.WriteAllText(Application.dataPath + "/save.txt", saveString);

        CMDebug.TextPopupMouse("Saved");
    }

    private void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("playerPositionX"))
        {
            float playerPositionX = PlayerPrefs.GetFloat("playerPositionX");
            float playerPositionY = PlayerPrefs.GetFloat("playerPositionY");
            Vector3 playerPosition = new(playerPositionX, playerPositionY);
            int goldAmount = PlayerPrefs.GetInt("goldAmount", 0);
            CMDebug.TextPopupMouse("" + playerPosition);

            unit.SetPosition(playerPosition);
            unit.SetGoldAmount(goldAmount);
        }
        else
        {
            // No save available
            CMDebug.TextPopupMouse("No save");
        }
    }

    private void Load()
    {
        string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
        CMDebug.TextPopupMouse("Loaded: " + saveString);

        string[] contents = saveString.Split(SAVE_SEPARATOR, System.StringSplitOptions.None);
        int goldAmount = int.Parse(contents[0]);
        float playerPositionX = float.Parse(contents[1]);
        float playerPositionY = float.Parse(contents[2]);
        Vector3 playerPosition = new(playerPositionX, playerPositionY);

        unit.SetGoldAmount(goldAmount);
        unit.SetPosition(playerPosition);
    }
}
