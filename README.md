# ProyectoMultiprocesamiento-SO
## Implementación de Azure Services y multiprocesamiento 
### Introducción

Se realizará un análisis de los resultados obtenidos del multiprocesamiento aplicado en el uso de diversos servicios de Microsoft Azure. En donde se utilizará del servicio de voz el text to speech, así como speech translation. Se utilizaron los lenguajes de programación de Python y C# para la implementació de los servicios de Microsoft Azure. También se utilizará el servicio de visión, para la identificación de caras y características asociadas a esta, además, es fundamental tener o crearse una cuenta en esta página para conseguir el desarrollo de estas funciones. La implementación de estos servicios es sencilla, en los códigos fuentes proporcionados se detalla el funcionamiento de los servicios, pero de igual forma se pueden consultar los siguientes enlaces para conocer acerca de  cada servicio:

Text to Speech:
- https://azure.microsoft.com/es-es/services/cognitive-services/text-to-speech/#overview

Speech Translation:
- https://azure.microsoft.com/es-es/services/cognitive-services/speech-translation/#overview


Servicio de visión:
- https://azure.microsoft.com/es-es/services/cognitive-services/computer-vision/#overview

### Análisis de Resultados 

#### Servicio de Visión

En este apartado se realizaron pruebas con diferentes cantidades de fotos, entre ellas fueron 103, 127 y 195, la diferencia de los tiempos respecto al método secuencial y paralelo son variados, debido que con unas cantidades se muestra gran diferencia en la optimización de la duración, pero con otras cantidades no es muy significativo el cambio que se revela, a continuación se muestra un gráfico para visualizar mejor estos procesos:

![DuracionSecuencial](ImagenesResultadosProyecto/Captura.PNG)

En el gráfico se está representando las cantidades de fotos por el tiempo de duración del método tanto en secuencial como en paralelo. 
  
  * El el proceso con 103 fotos de manera secuencial duró 10minutos con 15 segundos y de forma paralela duró 6minutos con 33segundos, por lo que se logró reducir 4minutos aproximadamente.
  
  * El el proceso con 127 fotos de manera secuencial duró 9minutos con 40 segundos y de forma paralela duró 8minutos con 48segundos, por lo que se logró reducir un aproximado de apenas 1minuto.
  
  * El el proceso con 195 fotos de manera secuencial duró 17minutos con 54 segundos y de forma paralela duró 15minutos con 19segundos, por lo que se logró reducir un aproximado de casi 3minutos.

De acuerdo con los resultados, se muestra que se logra reducir los tiempo aunque no con grandes diferencias, pero esto también se debe a la calidad de Internet que se tenga en el momento u otros aspecto de procesamiento.

Además, para cada imagen se le obtuvieron distintas características y se analizaron con respecto al primer grupo de imagénes que eran 103 fotos:

![DuracionSecuencial](ImagenesResultadosProyecto/Captura1.PNG)

En este gráfico se muestra que del total que eran 103 fotos, se detectaron 291 caras, de esas caras o personas se identificó 26 con accesorios, y hubieron distintas emociones como una persona con enfado, otra persona con miedo y otra persona con disgusto, no se encontraron personas sorprendidas ni tristes, habían 284 que presentaban alegría, así también 32 personas rebeldes y 26 personas con un estado neutro, además, también se encontró que 225 personas eran femeninas y 66 personas masculinas, también se sabe que la edad promedio de todas esas personas es 24 años y 194 presentan maquillaje en la fotos.

#### Servicio de Voz 

Para este apartado se seleccionó los servicio de Text to Speech y Speech translation, en cual text to speech permite convertir archivos de voz a audios, además el speech translation brinda la traducción de archivos de audios a diversos idiomas, en este caso se realizó al idioma alemán. 

Los resultados al aplicar estos servicios serán un archivo de audio con extensión .wav, en la cual contiene el audio del texto que se desee traducir, además de un archivo de texto con la traducción en alemán. A continuación, se visualiza imagen de ejemplo del resultado al aplicar los servicios de voz. 

![Resultados](ImagenesResultadosProyecto/archivosResultado.png)

Con respecto al análisis de textos y audios con estos servicios de voz de forma secuencial los tiempos obtenidos con los siguientes:

![DuracionSecuencial](ImagenesResultadosProyecto/Duracion-Secuencial.PNG)

De la imagen anterior se puede observar la duración de ejecución de forma secuencial en diversos datos, como lo son archivos de texto, los cuales contienen entre contienen entre 141 a 180 palabras, además de utilizar audios, cabe destacar que estas cantidades fueron definidas a partir de la cantidad máxima que se logró ejecutar sin presentar errores de forma secuencial. Es así como a continuación se detalla la información. 

- En el proceso con 8 textos de manera secuencial duró 19 minutos. 

- En el proceso con 12 textos de manera secuencial duró 24 minutos.

- En el proceso con 14 textos de manera secuencial duró 31 minutos.  

- En el proceso con 16 textos de manera secuencial falló al ejecutarse, el tiempo desde la ejecución al momento de fallar fue de 43 minutos. 
    
 
Con respecto al análisis de textos y audios con estos servicios de voz de forma paralela los tiempos obtenidos con los siguientes:

![DuracionParalela](ImagenesResultadosProyecto/Duracion-Paralelo.PNG)

De la imagen anterior se puede observar la duración de ejecución de forma paralela en diversos datos, las cantidades fueron definidas a partir de la cantidad máxima que se logró ejecutar sin presentar errores de forma secuencial, para realizar la comparativa en tiempo y datos analizados. Es así como a continuación se detalla la información. 

- En el proceso con 8 textos de manera paralela duró 15 minutos.  

- En el proceso con 12 textos de manera paralela duró 19 minutos. 

- En el proceso con 14 textos de manera paralela duró 26 minutos. 
 
- En el proceso con 16 textos de manera paralela duró 29 minutos. 

Por lo cual a partir de las pruebas con diferentes cantidades de datos, a continuación se muestra una comparativa entre ambos métodos de ejecución, la diferencia de los tiempos respecto al método secuencial y paralelo son variados, no obstante, se puede observar una mejoría en el paralelo al disminuir entre 4 a 11 minutos del método secuencial.

![DuracionParalela](ImagenesResultadosProyecto/comparativa.PNG)

En la ejecución de estos servicios se presentaron diversos factores causando problemas al realizar el análisis, la inestabilidad del ancho de banda fue una problemática en la cual el tiempo de ejecución de los servicios de voz se ve incrementada, como se muestra a continuación:

![DuracionParalela](ImagenesResultadosProyecto/Variante-Factor.PNG)

De la ilustración anterior se refleja el impacto en tiempo en donde se obtiene de 12 datos un tiempo en procesamiento paralelo de 19 minutos al ejecutarse en un rango aproximado de horas entre las 4:00pm y 5:00pm, no obstante, la misma cantidad de datos al ejecutarse aproximadamente 1 hora después incrementa a 27 minutos.  De igual forma al realizar las pruebas con 14 datos se da un cambio significativo en los tiempos, dado que de forma paralela en se pasó de 26 minutos a 48 minutos, lo cual muestra un incremento de 22 minutos. 

Además, las transacciones que se permiten realizar en el servicio de voz en relación con el límite permitido generan problemas ya que al implementar el multiprocesamiento de forma directa a las funciones de reconocimiento, se generan errores. 

#### Conclusiones

Azure Microsoft brindan la facilidad de diversos servicios, no obstante, es importante considerar las limitantes en las transacciones que se pueden realizar en ellos, ya que este afecta al aplicar paralelismo en los archivos a análizar.

Al implementar los métodos paralelos se brinda la optimización de tiempos, en comparación al secuencial. Además se debe mencionar el impacto que ocasiona el internet por lo cual se debe analizar las horas factibles para ejecutar estos métodos ya que el tiempo aún con paralelismo aplicado puede aumentar. 

Además, esta herramienta es de gran utilidad y sencilla de implementar de manera básica, aunque se le podrían dar grandes usus con mayor investigación y programas más desarrollados, ya que el reconocimiento de imágenes como la traducción de idiomas son herramientas fundamentales que la gran cantidad de aplicaciones utilizan.
