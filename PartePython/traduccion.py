#Arreglo que almacenará las traducciónes del texto
todo = []
#Arreglo general de todos los textos 
listaTextos = []


import time
import os
from numpy import array

try:
    import azure.cognitiveservices.speech as speechsdk
except ImportError:
    print("""
    Importing the Speech SDK for Python failed.
    Refer to
    https://docs.microsoft.com/azure/cognitive-services/speech-service/quickstart-python for
    installation instructions.
    """)
    import sys
    sys.exit(1)
    

#Se crea una instancia de una configuración de voz con una clave de suscripción y una región de servicio especificadas.
speech_key, service_region =  "7d5bf04beec74fa989bea71018d83ba9", "southcentralus"



def translation_continuous(weatherfilename):
    

    # configurar parámetros de traducción: idioma de origen e idiomas de destino
    translation_config = speechsdk.translation.SpeechTranslationConfig(
        subscription=speech_key, region=service_region,
        speech_recognition_language='en-US',
        target_languages=('de', 'fr'), voice_name="de-DE-Hedda")
    audio_config = speechsdk.audio.AudioConfig(filename=weatherfilename)

    # Crea un reconocedor de traducción usando un archivo de audio como entrada.
    recognizer = speechsdk.translation.TranslationRecognizer(
        translation_config=translation_config, audio_config=audio_config)

    
    def result_callback(event_type, evt):
    
        textosGer=""
        for x in evt.result.translations.items()[0][1]:
            if x ==".":
                if evt.result.translations.items()[0][1].endswith(".") :
                    print("Resultado: "+evt.result.translations.items()[0][1])
                    textosGer = textosGer+evt.result.translations.items()[0][1]
                    print(textosGer)
                    todo.append(textosGer) 
            if x ==". ":
                textosGer = textosGer+evt.result.translations.items()[0][1]
                todo.append(textosGer) 
            
    done = False

    def stop_cb(evt):
        nonlocal done
        done = True

    # conecta las funciones de devolución de llamada a los eventos disparados por el reconocedor
    recognizer.session_started.connect(lambda evt: print('SESSION STARTED: {}'.format(evt)))

    recognizer.session_stopped.connect(lambda evt: print('SESSION STOPPED {}'.format(evt)))
    # evento para resultados intermedios
    recognizer.recognizing.connect(lambda evt: result_callback('RECOGNIZING', evt))
    # evento para el resultado final
    recognizer.recognized.connect(lambda evt: result_callback('RECOGNIZED', evt))

  
    
    # evento de cancelación
    recognizer.canceled.connect(lambda evt: print('CANCELED: {} ({})'.format(evt, evt.reason)))

    # detener el reconocimiento continuo en eventos de sesión detenidos o cancelados
    recognizer.session_stopped.connect(stop_cb)
    recognizer.canceled.connect(stop_cb)


    # iniciar la traducción
    recognizer.start_continuous_recognition()
    
    while not done:
        time.sleep(.5)


    recognizer.stop_continuous_recognition()
    

#Funcion para almacenar los audios traducidos. 
def guardar(lista,cont):
    print("Lista es: ")
    print(lista)
    for con in lista:
        os.file = open("C:/Users/CPU/Documents/.2Sem2021/SO/Proyecto2/TextosTraducidos/file"+str(cont)+".txt", "a")
        os.file.write(con)
    os.file.close()