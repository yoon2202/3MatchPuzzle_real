using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoManager : Singleton<InfoManager>
{
    public ResourceScript resource;
    public World LevelList;

    [SerializeField]
    private Sprite[] SpriteGroup;

    public Dictionary<string, Sprite> SpriteTable = new Dictionary<string, Sprite>();

    void Start()
    {
        Init_SpriteTable();
    }

    public static int ReturnStageCount()
    {
        return Instance.resource.CurrentLevel;
    }


    public static Level ReturnCurrentStage()
    {
        return Instance.LevelList.levels[Instance.resource.CurrentLevel];
    }

    public static Sprite ReturnMissionSprite(string Tag)
    {
        return Instance.SpriteTable[Tag];
    }

    /// <summary>
    /// 딕셔너리 초기화
    /// </summary>
    void Init_SpriteTable()
    {
        SpriteTable.Add("Black", SpriteGroup[0]);
        SpriteTable.Add("Orange", SpriteGroup[1]);
        SpriteTable.Add("Pink", SpriteGroup[2]);
        SpriteTable.Add("Purple", SpriteGroup[3]);
        SpriteTable.Add("Yellow", SpriteGroup[4]);
    }

}
