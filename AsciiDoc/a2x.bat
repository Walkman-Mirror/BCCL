@echo off
set FILENAME=%~nx1
set FILENAMENOEXT=%~n1
set DIR=%~pd1
set DIR=%DIR:\=/%
set DIR=%DIR::=%

C:
chdir C:\cygwin\bin
bash --login -i -c "cd /cygdrive/%DIR%;asciidoc -a lang=en -v -b docbook -d book %FILENAME%;dblatex -V -T db2latex %FILENAMENOEXT%.xml"
del %~pd1\%FILENAMENOEXT%.xml
pause