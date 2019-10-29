import speech_recognition as sr
from googlevoice import Voice
from googlevoice.util import input
import sys
import fileinput
import random
import time
import ffmpeg
# import pocketsphinx
import re

# JSON Functions
import json

def read_file(path):
    file = open(path, "r")
    data = file.read()
    file.close()
    return data

def read_json(path):
    while True:
        try:
            return json.loads(read_file(path))
        except:
            print("File Opening Failure")

def write_json(path, data):
    while True:
        try:
            return write_file(path, json.dumps(data))
        except:
            print("File Opening Failure")

def write_file(path, data):
    file = open(path, "w")
    file.write(str(data))
    file.close()
    return data
    
#Transcribe output file 
print("Applying Speech Recognition to audio clip.") 
AUDIO_FILE = ("output.flac") 
r = sr.Recognizer() 
with open("RadioSlurper-3ee8ce50fa30.json", "r") as file:
    credentials = file.read()
    # print (credentials)
with sr.AudioFile(AUDIO_FILE) as source: 
    audio = r.record(source)   
try: 
    with open('api_key.txt', 'r') as file:
        api_key = file.read()
    # clip = (r.recognize_google(audio))
    # clip = (r.recognize_google_cloud(audio, language = "en-us", credentials_json = credentials))
    clip = (r.recognize_google_cloud(audio, language = "en-us", credentials_json = credentials))
    # clip = (r.recognize_sphinx(audio))
    # clip = r.recognize_google_cloud (audio, language = "en-us", credentials_json = json.dumps(read_json('/API-0ad5acd538ed4.json')))
except sr.UnknownValueError: 
	print("Speech Recognition could not understand audio") 
except sr.RequestError as e:
    print("Could not request results from Speech Recognition service; {0}".format(e))

if (clip.find('code word') != -1): 
    print ("Contains the phrase 'the code word' ") 
    r = r"code\sword.*?(?=\w)(\w+)"
    secret_word = re.findall(r, clip)
    print("The code word is: ", secret_word)
else: 
    print ("Doesn't contain the phrase 'the code word' ")
    print ("Full translation:", clip)


#log into Google Voice
with open('passwords.txt', 'r') as file:
        passwords = file.read()
def login():
    username, password = "kantospam@gmail.com", passwords
    voice = Voice()
    client = voice.login(username, password)
    return client

    #send text to Google Voice number
    def run():
        voice = Voice()
        voice.login()
        phoneNumber = "2062406946"
        text = secret_word
        voice.send_sms(phoneNumber, text)
        print ("Sent "+ secret_word)