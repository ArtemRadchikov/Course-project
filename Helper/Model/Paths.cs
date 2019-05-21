using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper.Model
{
    public static class Paths
    {
        public static string PathToMaximaLogs{get;set;}
        public static string PathToNewDecision { get; set; }
        public static string PathToNewDecisionFunctionValues { get; set; }
        public static string PathToNewDecisionСoefficientValues_a0 { get; set; }
        public static string PathToNewDecisionСoefficientValues_an { get; set; }
        public static string PathToNewDecisionСoefficientValues_bn { get; set; }
        public static string PathToNewDecisionFourierSeriesValues { get; set; }
        public static string PathToNewDecisionsegmentValues { get; set; }
        public static string PathToMaximaLogsBatch { get; set; }
        public static string PathMaximaCMD { get; set; }
        public static string PathToMaxima { get; set; }
        public static string PathToBatchCalculateNewDecision { get; set; }
        public static string PathToBatchExample { get; set; }

        static Paths()
        {
            PathToMaximaLogs = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug\", @"MaximaLogs\").Replace(@"\", @"/");
            PathToNewDecision = PathToMaximaLogs + "newdecision/";
            PathToNewDecisionFunctionValues = PathToNewDecision + "func.txt";
            PathToNewDecisionСoefficientValues_a0 = PathToNewDecision + "a0.txt";
            PathToNewDecisionСoefficientValues_an = PathToNewDecision + "an.txt";
            PathToNewDecisionСoefficientValues_bn = PathToNewDecision + "bn.txt";
            PathToNewDecisionFourierSeriesValues = PathToNewDecision + "FS.txt";
            PathToNewDecisionsegmentValues= PathToNewDecision + "SegmentValues.txt";
            PathToMaxima = "C:/maxima-5.42.2";
            PathMaximaCMD = PathToMaxima+"/bin/maxima.bat";

            PathToBatchCalculateNewDecision = PathToMaximaLogs + "CalculateNewdecision.txt";
            PathToBatchExample = PathToMaximaLogs + "batchExample.txt";
        }
    }
}
