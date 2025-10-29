using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public struct CookingResult
{
    public int basicTasteScore;
    public int performanceScore;
    public int finalScore;
    public bool isPass;
}


public class CookingManager : Singleton<CookingManager>
{
    private const int MAX_BASIC_TASTE_SCORE = 40;

    private const int PERFORMANCE_INITIAL_SCORE = 60;
    private const int MIN_PERFORMANCE_SCORE = 27;
    private const int MAX_PERFORMANCE_SCORE = 60;
    private const int PASS_SCORE = 82;

    private const float HEAT_FACTOR_MIN = 0.75f;
    private const float HEAT_FACTOR_DROP_FIRST = 0.05f;
    private const float HEAT_FACTOR_DROP_NEXT = 0.1f;
    private const float HEAT_DEVIATION_STEP_FIRST = 5f;
    private const float HEAT_DEVIATION_STEP_NEXT = 10f;

    private const float TIME_FACTOR_MIN = 0.6f;
    private const float TIME_DEVIATION_STEP_PERCENT = 0.1f;
    private const float TIME_FACTOR_DROP = 0.1f;
    private const float TIME_MAX_ERROR_PERCENT = 0.4f;

    public CookingResult CalculateScore(RecipeSO recipe, IngredientName mainIngredient, List<IngredientName> garnishes, float playerHeat, float playerTime)
    {
        int basicTasteScore = CalculateBasicTasteScore(recipe, mainIngredient, garnishes);
        int performanceScore = CalculatePerformanceScore(recipe, playerHeat, playerTime);
        int finalScore = basicTasteScore + performanceScore;

        return new CookingResult
        {
            basicTasteScore = basicTasteScore,
            performanceScore = performanceScore,
            finalScore = finalScore,
            isPass = (finalScore >= PASS_SCORE)
        };
    }

    private int CalculateBasicTasteScore(RecipeSO recipe, IngredientName mainIngredient, List<IngredientName> garnishes)
    {
        if (garnishes == null || garnishes.Count < 1 || garnishes.Count > 3)
        {
            Debug.LogWarning($"辅料数量不正确: {garnishes?.Count}。必须是 1-3 个。");
        }
        int mainQuality = recipe.GetMainIngredientQuality(mainIngredient);
        int garnishQualitySum = 0;
        if (garnishes != null)
        {
            foreach (var garnish in garnishes.Take(3))
            {
                garnishQualitySum += recipe.GetGarnishQuality(garnish);
            }
        }
        int totalBasicScore = mainQuality + garnishQualitySum;

        return Mathf.Clamp(totalBasicScore, 0, MAX_BASIC_TASTE_SCORE);
    }

    private int CalculatePerformanceScore(RecipeSO recipe, float playerHeat, float playerTime)
    {
        float heatFactor = CalculateHeatFactor(recipe.idealHeatRange, playerHeat);
        float timeFactor = CalculateTimeFactor(recipe.idealTimeRange, playerTime);
        float score = heatFactor * timeFactor * PERFORMANCE_INITIAL_SCORE;
        return Mathf.RoundToInt(Mathf.Clamp(score, MIN_PERFORMANCE_SCORE, MAX_PERFORMANCE_SCORE));
    }

    private float CalculateHeatFactor(Vector2Int idealRange, float playerHeat)
    {
        if (playerHeat >= idealRange.x && playerHeat <= idealRange.y)
        {
            return 1.0f;
        }
        float deviation = 0;
        if (playerHeat < idealRange.x)
        {
            deviation = idealRange.x - playerHeat;
        }
        {
            deviation = playerHeat - idealRange.y;
        }
        float factor = 1.0f;
        if (deviation >= HEAT_DEVIATION_STEP_FIRST)
        {
            factor -= HEAT_FACTOR_DROP_FIRST;
            float remainingDeviation = deviation - HEAT_DEVIATION_STEP_FIRST;
            int deviationSteps = Mathf.FloorToInt(remainingDeviation / HEAT_DEVIATION_STEP_NEXT);
            factor -= (deviationSteps * HEAT_FACTOR_DROP_NEXT);
        }
        return Mathf.Max(factor, HEAT_FACTOR_MIN);
    }

    private float CalculateTimeFactor(Vector2Int idealRange, float playerTime)
    {
        if (playerTime >= idealRange.x && playerTime <= idealRange.y)
        {
            return 1.0f;
        }

        float deviation = 0;
        if (playerTime < idealRange.x)
        {
            deviation = idealRange.x - playerTime;
        }
        else
        {
            deviation = playerTime - idealRange.y;
        }
        float idealTimeBase = (idealRange.x + idealRange.y) / 2.0f;
        if (idealTimeBase <= 0) return TIME_FACTOR_MIN;
        float errorPercent = deviation / idealTimeBase;
        if (errorPercent > TIME_MAX_ERROR_PERCENT)
        {
            return TIME_FACTOR_MIN;
        }
        int deviationSteps = Mathf.FloorToInt(errorPercent / TIME_DEVIATION_STEP_PERCENT);
        float factor = 1.0f - (deviationSteps * TIME_FACTOR_DROP);
        return Mathf.Max(factor, TIME_FACTOR_MIN);
    }
}