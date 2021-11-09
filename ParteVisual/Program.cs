using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Diagnostics;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;

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
            DetectFaceExtract(client, IMAGE_BASE_URL, RECOGNITION_MODEL4).Wait();
            

            sw.Stop(); // Detener la medición.
            Console.WriteLine("Tiempo: {0}", sw.Elapsed.ToString("hh\\:mm\\:ss\\.fff"));

            Console.WriteLine("Totales: ");
            Console.WriteLine("Cantidad de Caras: "+ cantCaras);
            Console.WriteLine("Cantidad de Accsesorios: " + accesorios);
            Console.WriteLine("Caras de Enfado: " + enfado);
            Console.WriteLine("Caras de Rebelde: " + rebelde);
            Console.WriteLine("Caras de Disgustado: " + disgustado);
            Console.WriteLine("Caras de Miedo: " + miedo);
            Console.WriteLine("Caras de Alegre: " + alegre);
            Console.WriteLine("Caras Neutro: " + neutro);
            Console.WriteLine("Caras Triste: " + triste);
            Console.WriteLine("Caras de Sorpresa: " + sorpresa);
            Console.WriteLine("Personas Femeninas: " + femenino);
            Console.WriteLine("Personas Masculinas: " + masculino);
            Console.WriteLine("Edad promedio de las personas: " + (edadPromedio/cantCaras));
            Console.WriteLine("Caras con Lentes: " + lentes);
            Console.WriteLine("Personas con Maquillaje: " + maquillaje);
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
        static int accesorios = 0;
        static int enfado = 0;
        static int rebelde = 0;
        static int disgustado = 0;
        static int miedo = 0;
        static int alegre = 0;
        static int neutro = 0;
        static int triste = 0;
        static int sorpresa = 0;
        static int femenino = 0;
        static int masculino = 0;
        static int edadPromedio = 0;
        static int lentes = 0;
        static int maquillaje = 0;

        //Detectar de forma secuencial = true o paralela = false.
        static bool secuencial = false;
       

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
                for (int i = 0; i < count; ++i) { 
                    accessoryArray[i] = accessoriesList[i].Type.ToString(); 
                    accesorios +=1;
                }
                accessory = string.Join(",", accessoryArray);
            }
            Console.WriteLine($"Accesorios : {accessory}");

            // Obtiene la emoción de la cara.
            string emotionType = string.Empty;
            double emotionValue = 0.0;
            Emotion emotion = face.FaceAttributes.Emotion;
            if (emotion.Anger > emotionValue) { emotionValue = emotion.Anger; emotionType = "Anger"; enfado += 1; }
            if (emotion.Contempt > emotionValue) { emotionValue = emotion.Contempt; emotionType = "Contempt"; rebelde += 1; }
            if (emotion.Disgust > emotionValue) { emotionValue = emotion.Disgust; emotionType = "Disgust"; disgustado += 1; }
            if (emotion.Fear > emotionValue) { emotionValue = emotion.Fear; emotionType = "Fear"; miedo += 1; }
            if (emotion.Happiness > emotionValue) { emotionValue = emotion.Happiness; emotionType = "Happiness"; alegre += 1; }
            if (emotion.Neutral > emotionValue) { emotionValue = emotion.Neutral; emotionType = "Neutral"; neutro +=1; }
            if (emotion.Sadness > emotionValue) { emotionValue = emotion.Sadness; emotionType = "Sadness"; triste += 1; }
            if (emotion.Surprise > emotionValue) { emotionType = "Surprise"; sorpresa += 1; }
            Console.WriteLine($"Emoción : {emotionType}");

            // Obtiene otros atributos de la cara
            if($"{face.FaceAttributes.Gender}" == "Female")
            {
                femenino += 1;
                Console.WriteLine("Género : Femenino");
            }
            if ($"{face.FaceAttributes.Gender}" == "Male")
            {
                masculino += 1;
                Console.WriteLine("Género : Masculino");
            }

            
            if($"{face.FaceAttributes.Glasses}" == "Glasses")
            {
                lentes += 1;
                Console.WriteLine("Lentes? : Sí");
            }
            Console.WriteLine($"Edad : {face.FaceAttributes.Age}");
            edadPromedio += Int32.Parse($"{face.FaceAttributes.Age}");
            if($"{string.Format("{0}", (face.FaceAttributes.Makeup.EyeMakeup || face.FaceAttributes.Makeup.LipMakeup) ? "Yes" : "No")}" == "Yes")
            {
                maquillaje += 1;
                Console.WriteLine("Maquillaje? : Sí");
            }
           
            Console.WriteLine();
        }

    }
}
