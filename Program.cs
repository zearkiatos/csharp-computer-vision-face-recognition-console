using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Rest;
using ComputerVisionFaceRecognitionConsole.Configuration;

class Program
{
    private static readonly string endpoint = AppConfiguration.azureComputerVisionBaseUrl;
    private static readonly string apiKey = AppConfiguration.azureComputerVisionApiKey;

    static async Task Main(string[] args)
    {
        var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(apiKey))
        {
            Endpoint = endpoint
        };

        string imageUrl = AppConfiguration.azureBlobStorageImageUrl;
        try
        {
            var faces = await DetectFaces(client, imageUrl);
            Console.WriteLine("Faces:");
            foreach (var face in faces)
            {
                Console.WriteLine($"- Face at location [Top: {face.FaceRectangle.Top}, Left: {face.FaceRectangle.Left}, Width: {face.FaceRectangle.Width}, Height: {face.FaceRectangle.Height}]");
            }
        }
        catch (ComputerVisionErrorResponseException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }

    static async Task<IList<FaceDescription>> DetectFaces(ComputerVisionClient client, string imageUrl)
    {
        var features = new List<VisualFeatureTypes?> { VisualFeatureTypes.Faces };

        var response = await client.AnalyzeImageAsync(imageUrl, visualFeatures: features);

        return response.Faces;
    }
}

