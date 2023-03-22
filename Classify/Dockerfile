FROM python:3.8

WORKDIR /Classify
ENV FLASK_RUN_HOST=0.0.0.0
ENV FLASK_APP=main.py
ARG COPY_FROM_PATH

RUN pip install --upgrade pip

RUN apt-get update \
 && apt-get install unixodbc -y \
 && apt-get install unixodbc-dev -y \
 && apt-get install freetds-dev -y \
 && apt-get install freetds-bin -y \
 && apt-get install tdsodbc -y \
 && apt-get install --reinstall build-essential -y

RUN echo "[FreeTDS]\n\
Description = FreeTDS unixODBC Driver\n\
Driver = /usr/lib/x86_64-linux-gnu/odbc/libtdsodbc.so\n\
Setup = /usr/lib/x86_64-linux-gnu/odbc/libtdsS.so" >> /etc/odbcinst.ini

COPY ${COPY_FROM_PATH} .

RUN pip install --default-timeout=10000 -r requirements.txt

EXPOSE 5000/tcp
EXPOSE 27017/tcp
EXPOSE 443/tcp
EXPOSE 8050/tcp
EXPOSE 1433/tcp

CMD ["python", "main.py"]