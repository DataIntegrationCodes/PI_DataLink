using OSIsoft.AF.Asset;
using OSIsoft.AF.Data;
using OSIsoft.AF.Time;
using System;
using System.Collections.Generic;

namespace PI_DataLink
{
    public class OSISoftAFData
    {
        public static System.Tuple<AFTimeRange, AFSummaryTypes, AFTimeSpan, AFCalculationBasis, AFTimestampCalculation> AFData(string startTime, string endTime, string interval)
        {
            AFTimeRange timeRange = new AFTimeRange(startTime, endTime);
            AFSummaryTypes summaryType = AFSummaryTypes.Average;
            AFTimeSpan summaryDuration = AFTimeSpan.Parse(interval);
            AFCalculationBasis calcBasis = AFCalculationBasis.TimeWeighted;
            AFTimestampCalculation timeType = AFTimestampCalculation.Auto;

            return Tuple.Create(timeRange, summaryType, summaryDuration, calcBasis, timeType);
        }

        public static Dictionary<string, List<AFValue>> tagSummaries = new Dictionary<string, List<AFValue>>();
        public static List<string> allTagNames = new List<string>();
       
    }
}
