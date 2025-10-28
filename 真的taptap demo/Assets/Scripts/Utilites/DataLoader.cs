using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OfficeOpenXml;


public class DataLoader: Singleton<DataLoader>
{
    public List<Dialogue> dialogues = new List<Dialogue>();
    public List<Option> options = new List<Option>();
    
    void Awake()
    {
        Read();
    }
    void Read()
    {
        string path = Application.dataPath + "/Excel/database.xlsx";
        Debug.Log(path);

        FileInfo fileInfo = new FileInfo(path);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            //Debug.Log($"表格数量 : {package.Workbook.Worksheets.Count}");
            ReadDialogue(package.Workbook.Worksheets[1]);
            ReadOptions(package.Workbook.Worksheets[2]);
            // 示例
            // int.TryParse(eventsheet.Cells[i, 3].Value?.ToString() ?? "0", out newEvent.humidity);
            // float.TryParse(eventsheet.Cells[i, 8].Value.ToString(), out newEvent.duration);
        }
    }

    void ReadDialogue(ExcelWorksheet dialoguesheet)
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
                dialogues.Add(newDialogue);
            }
        }
    }
    
    void ReadOptions(ExcelWorksheet sheet)
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
                options.Add(option);
            }
        }
    }


}