#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using OfficeOpenXml;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


public class ExcelToJsonConverter
{
    public static Dictionary<int, Dialogue> dialogueDict = new Dictionary<int, Dialogue>();
    public static Dictionary<int, Option> optionDict = new Dictionary<int, Option>();
    public static Dictionary<CharacterName, CharacterEvent> characterEventDict = new Dictionary<CharacterName, CharacterEvent>();
    
    
    [MenuItem("Tools/Export Database To JSON")]
    public static void ConvertDialogueExcel()
    {
        string excelPath = Application.dataPath + "/Resources/database.xlsx"; // Excel 文件路径

        Read();

        DataWrapper data = new DataWrapper
        {
            dialogues = dialogueDict,
            options = optionDict,
            characterEvents = characterEventDict
        };

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);

        string outputPath = Application.dataPath + "/Resources/database.json";
        File.WriteAllText(outputPath, json);
        Debug.Log("Database exported to: " + outputPath);

        // 刷新编辑器资源
        AssetDatabase.Refresh();
        
    }
    public static void Read()
    {
        string path = Application.dataPath + "/Resources/database.xlsx";
        Debug.Log(path);

        FileInfo fileInfo = new FileInfo(path);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            //Debug.Log($"表格数量 : {package.Workbook.Worksheets.Count}");
            ReadDialogue(package.Workbook.Worksheets[1]);
            ReadOptions(package.Workbook.Worksheets[2]);
            ReadCharacterEvent(package.Workbook.Worksheets[3]);
            // 示例
            // int.TryParse(eventsheet.Cells[i, 3].Value?.ToString() ?? "0", out newEvent.humidity);
            // float.TryParse(eventsheet.Cells[i, 8].Value.ToString(), out newEvent.duration);
        }
    }

    public static void ReadDialogue(ExcelWorksheet dialoguesheet)
    {
        int rowCount = dialoguesheet.Dimension.Rows;
        int colCount = dialoguesheet.Dimension.Columns;
        Debug.Log($"dialoguesheet: {rowCount} 条数据，{colCount} 个字段");
        for (int i = 3; i <= rowCount; i++)
        {
            int idx = 0;
            Dialogue newDialogue = new Dialogue();
            //防止有空行直接跳过
            if (dialoguesheet.Cells[i, 1].Value != null)
            {
                int.TryParse(dialoguesheet.Cells[i, 1].Value.ToString(), out idx);
                newDialogue.dialogueId = idx;
                newDialogue.dialogueText = dialoguesheet.Cells[i, 2].Value.ToString();
                newDialogue.description = dialoguesheet.Cells[i, 3].Value?.ToString();
                dialogueDict[idx] = newDialogue;
            }
        }
    }
    
    public static void ReadOptions(ExcelWorksheet sheet)
    {
        int rowCount = sheet.Dimension.Rows;
        int colCount = sheet.Dimension.Columns;
        Debug.Log($"OptionSheet: {rowCount} 条数据，{colCount} 个字段");
        for (int i = 3; i <= rowCount; i++)
        {
            Option option = new Option();
            //防止有空行直接跳过
            if (sheet.Cells[i, 1].Value != null)
            {
                int.TryParse(sheet.Cells[i, 1].Value?.ToString() ?? "0", out option.optionsId);
                option.optionText = sheet.Cells[i, 2].Value.ToString();
                // nextDialogueId = 0 表示结束对话
                int.TryParse(sheet.Cells[i, 3].Value?.ToString() ?? "0", out option.nextDialogueId);
                //TODO：防止空
                option.getItemId = sheet.Cells[i, 4].Value?.ToString();
                optionDict[option.optionsId] = option;
            }
        }
    }

    public static void ReadCharacterEvent(ExcelWorksheet eventsheet)
    {
        int rowCount = eventsheet.Dimension.Rows;
        int colCount = eventsheet.Dimension.Columns;
        Debug.Log($"eventsheet: {rowCount} 条数据，{colCount} 个字段");
        for (int i = 3; i <= rowCount; i++)
        {
            //防止有空行直接跳过
            if (eventsheet.Cells[i, 1].Value != null)
            {

                string nameStr = eventsheet.Cells[i, 1].Value.ToString();
                string roundsStr = eventsheet.Cells[i, 2].Value.ToString();

                if (!Enum.TryParse(nameStr, out CharacterName name))
                {
                    Debug.LogWarning($"未知角色名：{nameStr}");
                    continue;
                }

                List<int> specialRounds = roundsStr
                    .Split('|', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s, out int n) ? n : 0)
                    .Where(n => n > 0)
                    .ToList();

                string requiredItemStr = eventsheet.Cells[i, 8].Value?.ToString();
                if (!Enum.TryParse(requiredItemStr, out ItemName required))
                {
                    required = ItemName.None; // 转换失败时设默认值
                }

                string rewardIngredientStr = eventsheet.Cells[i, 9].Value?.ToString();
                if (!Enum.TryParse(rewardIngredientStr, out IngredientName reward))
                {
                    reward = IngredientName.None;
                }

                var ev = new CharacterEvent
                {
                    characterName = name,
                    specialRounds = specialRounds,
                    specialDialogueID = int.Parse(eventsheet.Cells[i, 3].Value.ToString()),
                    normalDialogueID = int.Parse(eventsheet.Cells[i, 4].Value.ToString()),
                    conditionRound = int.Parse(eventsheet.Cells[i, 5].Value.ToString()),
                    requiredDialogueID = int.Parse(eventsheet.Cells[i, 6].Value.ToString()),
                    successDialogueID = int.Parse(eventsheet.Cells[i, 7].Value.ToString()),
                    requiredItem = required,
                    rewardIngredient = reward
                };
                characterEventDict[name] = ev;
            }
        }
    }
}
#endif