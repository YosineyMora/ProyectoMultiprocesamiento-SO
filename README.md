# ProyectoMultiprocesamiento-SO
## Implementación de Azure Services y multiprocesamiento 
### Introducción

Se realizará un análisis de los resultados obtenidos del multiprocesamiento aplicado en el uso de diversos servicios de Microsoft Azure. En donde se utilizará del servicio de voz el text to speech, así como speech translation.


### Análisis de Resultados 

#### Servicio de Visión

En este apartado se realizaron pruebas con diferentes cantidades de fotos, entre ellas fueron 103, 127 y 195, la diferencia de los tiempos respecto al método secuencial y paralelo son variados, debido que con unas cantidades se muestra gran diferencia en la optimización de la duración, pero con otras cantidades no es muy significativo el cambio que se revela, a continuación se muestra un gráfico para visualizar mejor estos procesos:

![DuracionSecuencial](ImagenesResultadosProyecto/Captura.PNG)

En el gráfico se está representando las cantidades de fotos por el tiempo de duración del método tanto en secuencial como en paralelo. 
  El el proceso con 103 fotos de manera secuencial duró 10minutos con 15 segundos y de forma paralela duró 6minutos con 33segundos, por lo que se logró reducir 4minutos aproximadamente.
  El el proceso con 127 fotos de manera secuencial duró 9minutos con 40 segundos y de forma paralela duró 8minutos con 48segundos, por lo que se logró reducir un aproximado de apenas 1minuto.
  El el proceso con 195 fotos de manera secuencial duró 17minutos con 54 segundos y de forma paralela duró 15minutos con 19segundos, por lo que se logró reducir un aproximado de casi 3minutos.
De acuerdo con los resultados, se muestra que se logra reducir los tiempo aunque no con grandes diferencias, pero esto también se debe a la calidad de Internet que se tenga en el momento u otros aspecto de procesamiento.

#### Text to Speech y Speech translation

Con respecto al análisis de textos y audios con estos servicios de voz de forma secuencial los tiempos obtenidos con los siguientes:

![DuracionSecuencial](ImagenesResultadosProyecto/DuracionSecuencial.PNG)

Con respecto al análisis de textos y audios con estos servicios de voz de forma paralela los tiempos obtenidos con los siguientes:

![DuracionParalela](ImagenesResultadosProyecto/DuracionParalelo.PNG)
