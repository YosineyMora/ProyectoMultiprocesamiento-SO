import reconocimientoVoz
import traduccion
import os,time
import concurrent.futures 
from multiprocessing import Process

#Se obtiene los textos a convertir en audio
listaTextos=[]
listaTexT=[]

#ruta donde de encuentran los audios con extensión .wav 
filenameA = "C:/Users/CPU/Documents/.2Sem2021/SO/Proyecto2/TextoAudio/"

#Ruta de los txt
sourceFolder= "C:/Users/CPU/Documents/.2Sem2021/SO/Proyecto2/Textos"

#Además en el archivo traduccion.py en la linea 98 se debe indicar la ruta donde se almacenaran las traducciones

#Variable para concatenar el texto que se extrae
text=""
files = []
filesA = []
#Se obtienen los archivos de texto
for filename in os.listdir(sourceFolder):
    files.append(sourceFolder+"/"+filename)


#Función para obtener el texto a partir de las rutas donde se encuentren los txt
def obtenerText():
    text=""
    for x in files:
        f = open(x, "r")
        while(True):
            linea = f.readline()
            text=text+linea
            if not linea:
                break
        listaTextos.append(text)
        text=""
        f.close()

def secuencial():
    start = time.perf_counter()
    # # Generar VOZ**********************************************************************
    cont=0
    for x in listaTextos:
        reconocimientoVoz.generarvoz(x,cont)
        cont=cont+1
        time.sleep(.5)
    # # FIN Generar VOZ******************************************************************
    
    #Se obtienen los archivos de audio .wav
    for fileA in os.listdir(filenameA):
        filesA.append(filenameA+fileA)

    #Generar TRADUCCIÓN****************************************************************
    cont=0
    for audio in filesA:
        traduccion.translation_continuous(audio)
        traduccion.guardar(traduccion.todo,cont)
        traduccion.todo.clear()
        cont=cont+1;
    # #FIN Generar TRADUCCIÓN************************************************************
    print(f'Duration: {time.perf_counter() - start}')

def paralelo():
    start = time.perf_counter()
    with concurrent.futures.ThreadPoolExecutor() as executor:
        executor.submit(secuencial)
    print(f'Duration: {time.perf_counter() - start}')


#--------------------------------------------Método 1---------------------------------------------

obtenerText()
print("Textos listos")
secuencial()
print("Listo Secuencial")
paralelo()

#-------------------------------------------------------------------------------------------------
#--------------------------------------------Método 2---------------------------------------------



def generarVozP():
    start = time.perf_counter()
    cont=0
    with concurrent.futures.ThreadPoolExecutor() as executor:
        for archiv in listaTextos:
            time.sleep(10)
            executor.submit(reconocimientoVoz.generarvoz,archiv,cont)
            cont=cont+1
            time.sleep(.75)
    print(f'Duration: {time.perf_counter() - start}')
   
def generarTraduccionP():
    start = time.perf_counter()
    cont=0
    with concurrent.futures.ThreadPoolExecutor() as executor:
        for audio in filesA:
            executor.submit(traduccion.translation_continuous(),audio)
            time.sleep(.75)
            executor.submit(traduccion.guardar(traduccion.todo,cont))
            time.sleep(.75)
            executor.submit(traduccion.todo.clear())
            cont=cont+1;

#Este método no funcionaron debido a las transacciones en los servicios
# obtenerText()
# print("Textos listos")
# generarVozP()
# print("VOZ lista")
# generarTraduccionP()
# print("TRADUCCIÓN lista")

#-------------------------------------------------------------------------------------------------
#--------------------------------------------Método 3---------------------------------------------


def secuencialFragmento1():
    start = time.perf_counter()
    #Se obtienen los archivos de audio .wav
    for fileA in os.listdir(filenameA):
        filesA.append(filenameA+fileA)
    # Generar VOZ**********************************************************************
    cont=0
    while cont < len(filesA)/2:
        print(filesA[cont])
        # Generar TRADUCCIÓN****************************************************************
        traduccion.translation_continuous(filesA[cont])
        traduccion.guardar(traduccion.todo,cont)
        traduccion.todo.clear()
        cont=cont+1;
        #FIN Generar TRADUCCIÓN************************************************************
        time.sleep(.5)
    # FIN Generar VOZ******************************************************************
    print(f'Duration: {time.perf_counter() - start}')

def secuencialFragmento2():
    start = time.perf_counter()
    #Se obtienen los archivos de audio .wav
    for fileA in os.listdir(filenameA):
        filesA.append(filenameA+fileA)
    # Generar VOZ**********************************************************************
    indice=int(len(filesA)/2)
    while indice < len(filesA):
        print(filesA[indice])
        # Generar TRADUCCIÓN****************************************************************
        traduccion.translation_continuous(filesA[indice])
        traduccion.guardar(traduccion.todo,indice)
        traduccion.todo.clear()
        indice=indice+1
        time.sleep(.5)
    print(f'Duration: {time.perf_counter() - start}')

#Este método no funcionaron debido a las transacciones en los servicios 
# obtenerText()
# if __name__ == '__main__':
#     p1 = Process(target=secuencialFragmento1)
#     p2 = Process(target=secuencialFragmento2)
#     p1.start()
#     p1.join()
#     p2.start()
#     p2.join()