using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomVision_Sample
{
    class Program
    {
        static private string trainkey = "7655c7396ebe401499488a180fabd5e3";
        static private string predictkey = "2064ab8df1614c8abcb0578b62cde626";
        static private string endpoints = "https://southcentralus.api.cognitive.microsoft.com/";
        static public Guid guid = new Guid();
        static string publishedModelName = "maskModel";


        static public CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
        {
            ApiKey = trainkey,
            Endpoint = endpoints
        };
        static public CustomVisionPredictionClient endpoint = new CustomVisionPredictionClient()
        {
            ApiKey = predictkey,
            Endpoint = endpoints
        };

        static void Main(string[] args)
        {
            string path = "test.jpg";
            var allprojects = trainingApi.GetProjects().Select(x => new {id = x.Id , name = x.Name});            
            foreach(var item in allprojects)
            {
                if (item.name == "mask")
                    guid = item.id;
            }

            ClassifyImage(guid, path);

            Console.ReadKey();
        }
        static public async void ClassifyImage(Guid guid ,string path)
        {
            using (var stream = File.Open(path, FileMode.Open))
            {
                var imageresult = await endpoint.DetectImageAsync(guid, publishedModelName, stream);
                var result = imageresult.Predictions.Select(x => x.Probability).ToList();
                var maskresult = result.Max();
                if (maskresult >= 0.75)
                {
                    Console.WriteLine("有戴口罩");
                }
                else
                {
                    Console.WriteLine("沒戴口罩");
                }
            }
        }

    }
}
