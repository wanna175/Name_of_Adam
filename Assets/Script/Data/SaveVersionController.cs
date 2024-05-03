using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 버전에 따라 세이브 데이터의 무결성을 체크하는 클래스

public class SaveVersionController
{
    // ※ Unity Version에 따라 History를 반드시 기억해야 합니다!
    // 따라서 게임 런칭을 할 때 반드시 버전을 체크하고, 업데이트가 필요한 경우에만 마이그레이션을 진행해야 합니다.

    private List<string> versionHistory = new List<string>
    {
        "1.0.0-release",
        "1.0.1-release",
    };

    public bool IsValildVersion()
        => GameManager.OutGameData.GetVersion().Equals(Application.version);

    public bool CheckNeedMigration()
    {
        if (IsValildVersion() == true)
        {
            Debug.Log("The OutGameData is up to data.");
            return false;
        }

        Debug.Log($"Data Version is not matched! Save Version : {GameManager.OutGameData.GetVersion()} / Build Version {Application.version}");
        return true;
    }

    public void MigrateData()
    {
        string userVersion = GameManager.OutGameData.GetVersion();
        int userVersionIndex = versionHistory.IndexOf(userVersion);

        if (userVersionIndex == -1)
        {
            Debug.LogError("Invalid Version! Check [ProjectSetting] and [SaveVersionController]!");
            return;
        }

        // 버전은 순차적(Linearly)으로 업데이트 되어야 합니다.
        while (userVersion.Equals(Application.version) == false)
        {
            switch (userVersion)
            {
                // 이후 업데이트에서 구현 필요 시 추가
                case "1.0.0-release":
                    GameManager.SaveManager.DeleteSaveData();

                    // 전당 유닛 고유키(PrivateKey) 추가
                    foreach (HallUnit unit in GameManager.OutGameData.FindHallUnitList())
                    {
                        if (string.IsNullOrEmpty(unit.PrivateKey))
                            unit.PrivateKey = GameManager.CreatePrivateKey();
                    }
                    break;
            }

            userVersionIndex++;
            GameManager.OutGameData.SetVersion(versionHistory[userVersionIndex]);
            userVersion = GameManager.OutGameData.GetVersion();
        }

        GameManager.OutGameData.SaveData();
    }
}
