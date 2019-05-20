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
        public static string PathToNewDecesion { get; set; }
        public static string PathToNewDecesionFunctionValues { get; set; }
        public static string PathToNewDecesionСoefficientValues_a0 { get; set; }
        public static string PathToNewDecesionСoefficientValues_an { get; set; }
        public static string PathToNewDecesionСoefficientValues_bn { get; set; }
        public static string PathToNewDecesionFourierSeriesValues { get; set; }
        public static string PathToNewDecesionSegmentValues { get; set; }
        public static string PathToMaximaLogsBatch { get; set; }
        public static string PathMaximaCMD { get; set; }
        public static string PathToMaxima { get; set; }
        public static string PathToBatchCalculateNewDecesion { get; set; }
        public static string PathToBatchExample { get; set; }

        static Paths()
        {
            PathToMaximaLogs = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug\", @"MaximaLogs\").Replace(@"\", @"/");
            PathToNewDecesion = PathToMaximaLogs + "newDecision/";
            PathToNewDecesionFunctionValues = PathToNewDecesion + "func.txt";
            PathToNewDecesionСoefficientValues_a0 = PathToNewDecesion + "a0.txt";
            PathToNewDecesionСoefficientValues_an = PathToNewDecesion + "an.txt";
            PathToNewDecesionСoefficientValues_bn = PathToNewDecesion + "bn.txt";
            PathToNewDecesionFourierSeriesValues = PathToNewDecesion + "FS.txt";
            PathToNewDecesionSegmentValues= PathToNewDecesion + "SegmentValues.txt";
            PathToMaxima = "C:/maxima-5.42.2";
            PathMaximaCMD = PathToMaxima+"/bin/maxima.bat";

            PathToBatchCalculateNewDecesion = PathToMaximaLogs + "CalculateNewDecesion.txt";
            PathToBatchExample = PathToMaximaLogs + "batchExample.txt";
        }
    }
}
