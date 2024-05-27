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
        "1.0.2-release",
        "1.0.0v",
        "1.0.1v",
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

                case "1.0.1-release":

                    // 호루스 이름 변경
                    foreach (HallUnit unit in GameManager.OutGameData.FindHallUnitList())
                        if (unit.UnitName == "호루스")
                            unit.UnitName = "구원자";

                    SaveData saveData = GameManager.SaveManager.GetSaveData();
                    foreach (SaveUnit unit in saveData.DeckUnitData)
                        if (unit.UnitDataID == "호루스")
                            unit.UnitDataID = "구원자";

                    foreach (SaveUnit unit in saveData.FallenUnitsData)
                        if (unit.UnitDataID == "호루스")
                            unit.UnitDataID = "구원자";

                    GameManager.SaveManager.SaveData(saveData);

                    break;

                case "1.0.2-release":
                    GameManager.SaveManager.DeleteSaveData();
                    GameManager.OutGameData.DeleteAllData();
                    GameManager.OutGameData.CreateData();
                    break;

                case "1.0.0v":
                    GameManager.SaveManager.DeleteSaveData();

                    List<HallUnit> hallUnits = GameManager.OutGameData.FindHallUnitList();
                    for (int i = 0; i < hallUnits.Count; i++)
                    {
                        HallUnit unit = hallUnits[i];
                        unit.IsMainDeck = false;
                        unit.ID = 4 + i;
                    }

                    HallUnit darkKnight = hallUnits.Find(x => x.PrivateKey == "흑기사_Origin");
                    darkKnight.IsMainDeck = true;
                    darkKnight.ID = 0;

                    HallUnit prisoner = hallUnits.Find(x => x.PrivateKey == "죄수_Origin");
                    prisoner.IsMainDeck = true;
                    prisoner.ID = 1;

                    HallUnit gravekeeper = hallUnits.Find(x => x.PrivateKey == "묘지기_Origin");
                    gravekeeper.IsMainDeck = true;
                    gravekeeper.ID = 2;

                    break;
            }

            userVersionIndex++;

            if (userVersionIndex >= versionHistory.Count)
            {
                Debug.LogError($"버전 히스토리를 등록하세요! 새로운 버전을 SaveVersionController.cs 상단에 기록하세요.");
                Debug.LogError($"현재 버전: {versionHistory[versionHistory.Count - 1]}");
                return;
            }

            GameManager.OutGameData.SetVersion(versionHistory[userVersionIndex]);
            userVersion = GameManager.OutGameData.GetVersion();
        }

        GameManager.OutGameData.SaveData();
    }
}
