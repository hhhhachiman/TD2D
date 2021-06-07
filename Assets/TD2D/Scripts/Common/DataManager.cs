using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[Serializable]

public class DataVersion
{
    public int major;
    public int minor;
}

[Serializable]

public class GameProgessData
{
    public System.DateTime savTime;
    public string lastCompetedLevel;
    public List<string> openedLevels = new List<string>();   //liset with levles available to play
}
public class DataManager : MonoBehaviour
{   //game progress data container
    public GameProgessData progress = new GameProgessData();
    //initial instance
    public static DataManager instance;

    // Name of file with data version
    private string dataVersionFile = "/DataVersion.dat";
    // Name of file with game progress data
    private string gameProgressFile = "/GameProgress.dat";
    //data version container
    private DataVersion dataVersion = new DataVersion();
    private GameProgessData gameProgessDefaultData = new GameProgessData();
    void Awake()
    {
        if (instance == null)
        {
            dataVersion.major = 1;
            dataVersion.minor = 0;

            //default game progress data
            progress.savTime = gameProgessDefaultData.savTime = DateTime.MinValue;
            progress.lastCompetedLevel = gameProgessDefaultData.lastCompetedLevel = "";
            instance = this;
            DontDestroyOnLoad(gameObject);

            UpdateDataVersion();
            LoadGameProgess();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

    }

    private void LoadGameProgess()
    {
        if (File.Exists(Application.persistentDataPath + gameProgressFile) == true)
        {   //加载文件，反序列化文件生成游戏进程
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + gameProgressFile, FileMode.Open);

            progress = (GameProgessData)bf.Deserialize(file);
            file.Close();


        }
    }
        private void UpdateDataVersion()
        {
            if (File.Exists(Application.persistentDataPath + dataVersionFile) == true)
            {
                BinaryFormatter bfOpen = new BinaryFormatter();
                FileStream fileToOpen = File.Open(Application.persistentDataPath + dataVersionFile, FileMode.Open);
                DataVersion version = (DataVersion)bfOpen.Deserialize(fileToOpen);
                fileToOpen.Close();

            switch (version.major)
            {
                case 1:
                    break;
            }

            }
        BinaryFormatter bfCreate = new BinaryFormatter();
        FileStream fileToCreate = File.Create(Application.persistentDataPath + dataVersionFile);
        bfCreate.Serialize(fileToCreate, dataVersion);
        fileToCreate.Close();
        }

        public void SaveGameProgess()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + gameProgressFile);
            progress.savTime = DateTime.Now;
            bf.Serialize(file, progress);
            file.Close();
        }
        private void DeleteGameProgess()
        {
            File.Delete(Application.persistentDataPath + gameProgressFile);
        }

    
}
