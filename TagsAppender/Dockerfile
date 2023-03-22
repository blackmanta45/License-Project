FROM python:3.8

WORKDIR /TagsAppender
ENV FLASK_RUN_HOST=0.0.0.0
ENV FLASK_APP=main.py
FROM openjdk:slim
COPY --from=python:3.8 / /


RUN pip install --upgrade pip

ARG COPY_FROM_PATH
COPY ${COPY_FROM_PATH} .

RUN pip install --default-timeout=10000 -r requirements.txt

EXPOSE 5004/tcp
EXPOSE 27017/tcp
EXPOSE 443/tcp
EXPOSE 1433/tcp
EXPOSE 9000/tcp

CMD ["python", "main.py"]