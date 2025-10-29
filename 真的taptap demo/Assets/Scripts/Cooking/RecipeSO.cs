using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeSO_", menuName = "Cooking/Recipe")]
public class RecipeSO : ScriptableObject
{
    [Header("基础信息")]
    public string recipeName;
    public Sprite recipeIcon;

    [Header("食材品质定义（未在以下两个列表中的主料、铺料均视为冲突，主料0分，辅料-3分）")]
    [Tooltip("标准主料 (37分")]
    public List<IngredientName> standardMain;
    [Tooltip("普通主料(27分")]
    public List<IngredientName> commonMain;

    [Space(10)]
    [Tooltip("标准辅料（3分")]
    public List<IngredientName> standardGarnish;
    [Tooltip("普通辅料(0分")]

    public List<IngredientName> commonGarnish;


    [Header("烹饪参数")]
    [Tooltip("理想火力区间 [x, y]")]
    public Vector2Int idealHeatRange;
    [Tooltip("烹饪时间区间 [xX, y] (秒)")]
    public Vector2Int idealTimeRange;

    public int GetMainIngredientQuality(IngredientName main)
    {
        if (standardMain.Contains(main)) return 37;
        if (commonMain.Contains(main)) return 27;
        return 0;
    }

    public int GetGarnishQuality(IngredientName garnish)
    {
        if (standardGarnish.Contains(garnish)) return 3;
        if (commonGarnish.Contains(garnish)) return 0;
        return -3;
    }
}