using OSIsoft.AF.Asset;
using System.Collections.Generic;
using System.Text;

namespace PI_DataLink
{
    public class PIDataSet
    {
        public static List<string> Generated(List<string> allTagNames, Dictionary<string, List<AFValue>> tagSummaries)
        {
            List<string> StoreData = new List<string>();
            StringBuilder sb = new StringBuilder();

            allTagNames.Insert(0, "TimeStamp");

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
                StoreData.Add(string.Join("|",allTagNames.ToArray()));
                // Write data rows
                for (int i = 0; i < timeStamps.Count; i++)
                {
                    sb.Append($"{timeStamps[i].Timestamp.LocalTime.ToString("yyyy-MM-dd HH:mm:ss")}|");

                    for (int col = 0; col < allTagNames.Count; col++)
                    {
                        var tagName = allTagNames[col];
                        if (tagSummaries.ContainsKey(tagName) && tagSummaries[tagName].Count > i)
                        {
                            var val = tagSummaries[tagName][i].Value;
                            //var tmp = val != null ? val.ToString() : "N/A";
                            sb.Append($"{val}|").ToString();
                        }
                        else
                        {
                            //sb.Append("N/A|").ToString();
                        }
                    }
                    StoreData.Add(sb.ToString().TrimEnd('|'));
                    sb.Clear();
                }
            }

            return StoreData;
        }
    }
}
