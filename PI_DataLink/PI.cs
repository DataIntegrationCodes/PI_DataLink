using OSIsoft.AF.Asset;
using OSIsoft.AF.PI;
using OSIsoft.AF.Data;
using OSIsoft.AF.Time;
using System;
using System.Collections.Generic;

namespace PI_DataLink
{
    public class PI
    {
        public static List<string> StoredData = new List<string>();
        public static List<string> PISearch(string PIServerName,string SaveFilePath, List<string> tagPatterns, string startDate, string EndDate, string Interval,int mode)
        {
            OSISoftAFData.AFData(startDate,EndDate,Interval).
                Deconstruct(out AFTimeRange timeRange
                ,out AFSummaryTypes summaryType
                ,out AFTimeSpan summaryDuration
                ,out AFCalculationBasis calcBasis
                ,out AFTimestampCalculation timeType);

            var tagSummaries = OSISoftAFData.tagSummaries;
            var allTagNames = OSISoftAFData.allTagNames;
            var piServer = PIConnect.pIServer(PIServerName);

            piServer.Connect();

            foreach (string pattern in tagPatterns)
            {
                try
                {
                    IEnumerable<PIPoint> piPoints = PIPoint.FindPIPoints(piServer, pattern);

                    foreach (PIPoint piPoint in piPoints)
                    {
                        try
                        {
                            AFValues summaries = piPoint.Summaries(
                                timeRange,
                                summaryDuration,
                                summaryType,
                                calcBasis,
                                timeType
                            )[summaryType];

                            tagSummaries[piPoint.Name] = new List<AFValue>(summaries);
                            allTagNames.Add(piPoint.Name);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error retrieving summaries for '{piPoint.Name}': {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error finding tags with pattern '{pattern}': {ex.Message}");
                }
            }

            if(mode==0) WriteToExcel.OutputResults(SaveFilePath, allTagNames, tagSummaries);
            else StoredData = PIDataSet.Generated(allTagNames, tagSummaries);
            piServer.Disconnect();

            return StoredData;

        }
    }
}
