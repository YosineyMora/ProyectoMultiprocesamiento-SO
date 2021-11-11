

from azure.cognitiveservices import speech
from azure.cognitiveservices.speech import AudioDataStream, SpeechConfig, SpeechSynthesizer, SpeechSynthesisOutputFormat
from azure.cognitiveservices.speech.audio import AudioOutputConfig

#Se crea una instancia de una configuración de voz con una clave de suscripción y una región de servicio especificadas.
speech_key, service_region = "7d5bf04beec74fa989bea71018d83ba9", "southcentralus"
speech_config = SpeechConfig(subscription=speech_key, region=service_region)

#Se crea un sintetizador de voz utilizando el altavoz predeterminado como salida de audio.
speech_synthesizer = speech.SpeechSynthesizer(speech_config=speech_config)


def generarvoz(text,cont):
    print(cont)
    # Sintetiza el texto recibido a voz.
    # Se espera que el discurso sintetizado se escuche en el altavoz con esta línea ejecutada.
    result = speech_synthesizer.speak_text_async(text).get()
    # Resultado de las comprobaciones.
    if result.reason == speech.ResultReason.SynthesizingAudioCompleted:
        speech_config.set_speech_synthesis_output_format(SpeechSynthesisOutputFormat["Riff24Khz16BitMonoPcm"])
        synthesizer = SpeechSynthesizer(speech_config=speech_config, audio_config=None)
        result = synthesizer.speak_text_async(text).get()
        stream = AudioDataStream(result)
        stream.save_to_wav_file("C:/Users/CPU/Documents/.2Sem2021/SO/Proyecto2/TextoAudio/"+str(cont)+"file.wav")
        
    elif result.reason == speech.ResultReason.Canceled:
        cancellation_details = result.cancellation_details
        print("Speech synthesis canceled: {}".format(cancellation_details.reason))
        if cancellation_details.reason == speech.CancellationReason.Error:
            if cancellation_details.error_details:
                print("Error details: {}".format(cancellation_details.error_details))
        print("Did you update the subscription info?")
    