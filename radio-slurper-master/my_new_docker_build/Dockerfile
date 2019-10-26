FROM python:3
ADD radioslurper.py /
ADD passwords.txt /
ADD RadioSlurper-3ee8ce50fa30.json /
ADD api_key.txt /
RUN pip install SpeechRecognition
RUN pip install googlevoice
RUN apt-get -y update
RUN apt-get -y upgrade
RUN apt-get install -y ffmpeg
RUN pip install ffmpeg-python
RUN pip install pygooglevoice
RUN pip install google-api-python-client
RUN pip install oauth2client

CMD [ "python", "./radioslurper.py" ]