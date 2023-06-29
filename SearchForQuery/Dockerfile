
FROM openjdk:slim

WORKDIR /SearchForQuery
ENV FLASK_RUN_HOST=0.0.0.0
ENV FLASK_APP=App.py
ENV PYTHON_VERSION 3.8

#Set of all dependencies needed for pyenv to work on Ubuntu systems
RUN apt-get update \ 
        && apt-get install -y --no-install-recommends make build-essential libssl-dev zlib1g-dev libbz2-dev libreadline-dev libsqlite3-dev wget ca-certificates curl llvm libncurses5-dev xz-utils tk-dev libxml2-dev libxmlsec1-dev libffi-dev liblzma-dev mecab-ipadic-utf8 git


ENV PYENV_ROOT /root/.pyenv
ENV PATH $PYENV_ROOT/shims:$PYENV_ROOT/bin:$PATH


# Install pyenv
RUN set -ex \
    && curl https://pyenv.run | bash \
    && pyenv update \
    && pyenv install $PYTHON_VERSION \
    && pyenv global $PYTHON_VERSION \
    && pyenv rehash

RUN pip install --upgrade pip

ARG COPY_FROM_PATH
COPY ${COPY_FROM_PATH} .

RUN pip install --default-timeout=10000 -r requirements.txt

EXPOSE 5001/tcp
EXPOSE 443/tcp
EXPOSE 27017/tcp
EXPOSE 9000/tcp

CMD ["python", "App.py"]