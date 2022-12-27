using UnityEngine;
using CodeMonkey;
using System.IO;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private GameObject unitGameObject;
    private IUnit unit;

    private void Awake()
    {
        unit = unitGameObject.GetComponent<IUnit>();

        SaveSystem.Init();

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


        SaveObject saveObject = new SaveObject { goldAmount = goldAmount, playerPosition = playerPosition };
        string json = JsonUtility.ToJson(saveObject);
        SaveSystem.Save(json);

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
        string saveString = SaveSystem.Load();
        if (saveString != null)
        {
            CMDebug.TextPopupMouse("Loaded: " + saveString);
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            unit.SetGoldAmount(saveObject.goldAmount);
            unit.SetPosition(saveObject.playerPosition);
        }
        else
        {
            CMDebug.TextPopupMouse("No save");
        }
    }

    private class SaveObject
    {
        public int goldAmount;
        public Vector3 playerPosition;
    }
}
