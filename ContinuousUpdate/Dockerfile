FROM python:3.8

WORKDIR /ContinuousUpdate
ARG COPY_FROM_PATH

RUN pip install --upgrade pip

COPY ${COPY_FROM_PATH} .

RUN pip install --default-timeout=10000 -r requirements.txt

EXPOSE 7070/tcp
EXPOSE 5000/tcp
EXPOSE 5001/tcp

CMD ["python", "main.py"]