using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Win32.SafeHandles;

namespace proyectoSO
{
    
    class Program 
    {
        // Conectarse al Face de Azure Portal con la "Clave 1" y "Extremo"
        const string SUBSCRIPTION_KEY = "abf3e9af11a640b3a282ab0f8de4ed9e";
        const string ENDPOINT = "https://alyson.cognitiveservices.azure.com/";

        static void Main()
        {
            
            // Verificar la conexión.
            IFaceClient client = Authenticate(ENDPOINT, SUBSCRIPTION_KEY);
            

            Stopwatch sw = new Stopwatch(); // Creación del Stopwatch.
            sw.Start(); // Iniciar la medición.

            // Detectar características del rostro
            try
            {
                DetectFaceExtract(client, IMAGE_BASE_URL, RECOGNITION_MODEL4).Wait();
            }catch(AggregateException ae)
            {
                Console.WriteLine(cantCaras);
                Console.WriteLine("----------------------------------------------------");
            }

            sw.Stop(); // Detener la medición.
            Console.WriteLine("Tiempo: {0}", sw.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));
        }

        

        public static IFaceClient Authenticate(string endpoint, string key)
        {
            return new FaceClient(new ApiKeyServiceClientCredentials(key)) { Endpoint = endpoint };
        }

        // URL de las imágenes
        const string IMAGE_BASE_URL = @"C:\Users\Dell\Desktop\Memories\";

        //Modelo a utilizar para el reconocimiento
        const string RECOGNITION_MODEL4 = RecognitionModel.Recognition04;

        //Cantidad de caras reconocidas
        static int cantCaras = 0;

        //Detectar de forma secuencial o paralela.
        static bool secuencial = true;
        private bool disposedValue;

        //Conviete un URL a Stream
        private static Stream GetStreamFromUrl(string url)
        {
            byte[] imageData = null;

            using (var wc = new System.Net.WebClient())
                imageData = wc.DownloadData(url);

            return new MemoryStream(imageData);
        }


        public static async Task DetectFaceExtract(IFaceClient client, string url, string recognitionModel)
        {
            Console.WriteLine("========Caras Detectadas========");
            Console.WriteLine();

            //Lista de los nombres de las imágenes que se van a reconocer
            List<string> imageFileNames = new List<string>();
            DirectoryInfo di = new DirectoryInfo(IMAGE_BASE_URL);

            foreach( var foto  in di.GetFiles())
            {
                imageFileNames.Add(foto.Name);
            }

            //Método de forma secuencial
            if (secuencial == true)
            {
                //Reconocimiento de cada imagen en la lista
                foreach (var imageFileName in imageFileNames)
                {

                    IList<DetectedFace> detectedFaces;

                    // Detecta todos los atributos de las caras en la imagen
                    detectedFaces = await client.Face.DetectWithStreamAsync(GetStreamFromUrl(Path.Combine(url, imageFileName)),
                    returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Accessories, FaceAttributeType.Age,
                        FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.Gender, FaceAttributeType.Glasses,
                        FaceAttributeType.Hair, FaceAttributeType.Makeup, FaceAttributeType.Smile },

                    // Se usa el modelo 1 porque se están recuperando atributos
                    detectionModel: DetectionModel.Detection01,
                    recognitionModel: recognitionModel);
                    //Validación para limpiar los hilos 
                    

                    cantCaras += detectedFaces.Count;
                    Console.WriteLine($"{detectedFaces.Count} cara(s) detectadas de la imagen: `{imageFileName}`.");

                    // Analiza e imprime los atributos del rostro
                    foreach (var face in detectedFaces)
                    {
                        Console.WriteLine($"Atributos para la imagen: {imageFileName}:");
                        extraerAtributos(face);
                    }
                    Thread.Sleep(2000);
                }
            }
            //Método de forma paralela
            else
            {
                int maxConcurrency = 8;
                using (SemaphoreSlim concurrencySemaphore = new SemaphoreSlim(maxConcurrency))
                {
                    List<Task> tasks = new List<Task>();
                    foreach (var imageFileName in imageFileNames)
                    {
                        concurrencySemaphore.Wait();

                        var t = Task.Factory.StartNew(async() =>
                        {
                            try
                            {
                                IList<DetectedFace> detectedFaces;

                                // Detecta todos los atributos de las caras en la imagen
                                detectedFaces = await client.Face.DetectWithStreamAsync(GetStreamFromUrl(Path.Combine(url, imageFileName)),
                                    returnFaceAttributes: new List<FaceAttributeType> { FaceAttributeType.Accessories, FaceAttributeType.Age,
                                FaceAttributeType.Emotion, FaceAttributeType.Exposure, FaceAttributeType.Gender, FaceAttributeType.Glasses,
                                FaceAttributeType.Hair, FaceAttributeType.Makeup, FaceAttributeType.Smile },
                                    // Se usa el modelo 1 porque se están recuperando atributos
                                    detectionModel: DetectionModel.Detection01,
                                    recognitionModel: recognitionModel);


                                cantCaras += detectedFaces.Count;
                                Console.WriteLine($"{detectedFaces.Count} cara(s) detectadas de la imagen: `{imageFileName}`.");

                                // Analiza e imprime los atributos del rostro
                                foreach (var face in detectedFaces)
                                {
                                    Console.WriteLine($"Atributos para la imagen: {imageFileName}:");
                                    extraerAtributos(face);
                                }
                            }
                            finally
                            {
                                concurrencySemaphore.Release();
                            }
                        });
                        tasks.Add(t);
                    }
                    await Task.WhenAll(tasks.ToArray());
                }
            }
            
        }

        public static void extraerAtributos(DetectedFace face)
        {
            // Obtiene los accesorios de la cara.
            List<Accessory> accessoriesList = (List<Accessory>)face.FaceAttributes.Accessories;
            int count = face.FaceAttributes.Accessories.Count;
            string accessory; string[] accessoryArray = new string[count];
            if (count == 0) { accessory = "Sin accesorios"; }
            else
            {
                for (int i = 0; i < count; ++i) { accessoryArray[i] = accessoriesList[i].Type.ToString(); }
                accessory = string.Join(",", accessoryArray);
            }
            Console.WriteLine($"Accesorios : {accessory}");

            // Obtiene la emoción de la cara.
            string emotionType = string.Empty;
            double emotionValue = 0.0;
            Emotion emotion = face.FaceAttributes.Emotion;
            if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; }
            if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; }
            if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; }
            if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; }
            if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; }
            if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; }
            if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; }
            if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; }
            Console.WriteLine($"Emoción : {emotionType}");

            // Obtiene otros atributos de la cara
            Console.WriteLine($"Género : {face.FaceAttributes.Gender}");
            Console.WriteLine($"Lentes : {face.FaceAttributes.Glasses}");
            Console.WriteLine($"Edad : {face.FaceAttributes.Age}");
            Console.WriteLine($"Maquillaje : {string.Format("{0}", (face.FaceAttributes.Makeup.EyeMakeup || face.FaceAttributes.Makeup.LipMakeup) ? "Yes" : "No")}");
            Console.WriteLine();
        }

        private string _logDirectory = null;
        private static Program _instance = null;

        private Program() : this(ConfigurationManager.AppSettings["LogDirectory"])
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        private Program(string logDirectory)
        {
        }

        public static Program Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Program();
                return _instance;
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Dispose();
        }



        public void Dispose()
        {
            // Dispose unmanaged resources
        }
    }
}
