@echo off
setlocal

set "DOCKERHUB_USER=pedro007salo"
set "WEB_IMAGE=todolist-web"
set "API_IMAGE=todolist-api"

pushd "%~dp0"

REM ==== WEB ====
docker build -t %WEB_IMAGE%:latest -f web\Dockerfile web
docker tag %WEB_IMAGE%:latest %DOCKERHUB_USER%/%WEB_IMAGE%:latest
docker push %DOCKERHUB_USER%/%WEB_IMAGE%:latest

REM ==== API ====
docker build -t %API_IMAGE%:latest -f Api\Dockerfile .
docker tag %API_IMAGE%:latest %DOCKERHUB_USER%/%API_IMAGE%:latest
docker push %DOCKERHUB_USER%/%API_IMAGE%:latest

echo DONE
pause

popd
endlocal
