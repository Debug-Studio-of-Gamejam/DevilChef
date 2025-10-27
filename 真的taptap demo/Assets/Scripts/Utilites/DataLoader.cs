using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using OfficeOpenXml;


public class DataLoader: Singleton<DataLoader>
{
    // Start is called before the first frame update
    void Start()
    {
        ReadDialogue();
        
    }

    void ReadDialogue()
    {
        string path = Application.dataPath + "/Excel/database.xlsx";
        Debug.Log(path);

        FileInfo fileInfo = new FileInfo(path);
        using (ExcelPackage package = new ExcelPackage(fileInfo))
        {
            //为了方便直接读第二页Event
            ExcelWorksheet eventsheet = package.Workbook.Worksheets[2];
            int rowCount = eventsheet.Dimension.Rows;
            int colCount = eventsheet.Dimension.Columns;

            //Debug.Log($"eventsheet: {rowCount} 条数据，{colCount} 个字段");
            //第三行开始读（行1是说明，行2是字段）
            for (int i = 3; i < rowCount; i++)
            {
                int idx = 0;
                Dialogue newDialogue = new Dialogue();

                int.TryParse(eventsheet.Cells[i, 1].Value.ToString(), out idx);
                // newEvent.name = eventsheet.Cells[i, 2].Value.ToString();
                //
                // //没有判空，没有表检查，可能不安全。粗暴处理
                // int.TryParse(eventsheet.Cells[i, 3].Value?.ToString() ?? "0", out newEvent.humidity);
                // int.TryParse(eventsheet.Cells[i, 4].Value?.ToString() ?? "0", out newEvent.heat);
                // int.TryParse(eventsheet.Cells[i, 5].Value?.ToString() ?? "0", out newEvent.fertility);
                // int.TryParse(eventsheet.Cells[i, 6].Value?.ToString() ?? "0", out newEvent.vitality);
                //
                // newEvent.effectFileName = eventsheet.Cells[i, 7].Value.ToString();
                // float.TryParse(eventsheet.Cells[i, 8].Value.ToString(), out newEvent.duration);
                // newEvent.description = newEvent.effectFileName; //eventsheet.Cells[i, 9].Value.ToString();
                //
                // eventList.Add(newEvent);
                // Debug.Log(
                //     $"{idx} {newEvent.name} {newEvent.humidity} {newEvent.heat} {newEvent.fertility} {newEvent.vitality} {newEvent.effectFileName} {newEvent.duration}");
            }
        }
    }
}