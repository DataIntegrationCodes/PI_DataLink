using ClosedXML.Excel;
using OSIsoft.AF.Asset;
using System;
using System.Collections.Generic;

namespace PI_DataLink
{
    public class WriteToExcel
    {
        public static void OutputResults(string Filename, List<string> allTagNames, Dictionary<string, List<AFValue>> tagSummaries)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Summary");

                // Write headers
                worksheet.Cell(1, 1).Value = "Timestamp";
                for (int col = 0; col < allTagNames.Count; col++)
                {
                    worksheet.Cell(1, col + 2).Value = allTagNames[col];
                }

                // Find first tag with data for timestamps
                string firstTag = null;
                foreach (var tag in allTagNames)
                {
                    if (tagSummaries.ContainsKey(tag))
                    {
                        firstTag = tag;
                        break;
                    }
                }

                if (firstTag != null)
                {
                    var timeStamps = tagSummaries[firstTag];

                    // Write data rows
                    for (int i = 0; i < timeStamps.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = timeStamps[i].Timestamp.LocalTime;
                        worksheet.Cell(i + 2, 1).Style.DateFormat.Format = "yyyy-MM-dd HH:mm";

                        for (int col = 0; col < allTagNames.Count; col++)
                        {
                            var tagName = allTagNames[col];
                            if (tagSummaries.ContainsKey(tagName) && tagSummaries[tagName].Count > i)
                            {
                                var val = tagSummaries[tagName][i].Value;
                                worksheet.Cell(i + 2, col + 2).Value = val != null ? val.ToString() : "N/A";
                            }
                            else
                            {
                                worksheet.Cell(i + 2, col + 2).Value = "N/A";
                            }
                        }
                    }
                }

                workbook.SaveAs(Filename);
                Console.WriteLine($"Excel file saved to {Filename}");
            }
        }
    }
}
