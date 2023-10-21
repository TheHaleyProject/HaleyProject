@echo off
echo pulling all folders
rem for /D %%G in (..\*) do (echo %%G) && (cd %%G) && (git pull origin) && (cd ..)
for /D %%G in (..\*) do ((echo %%G) && (cd %%G) &&  (cd) && (git pull origin))
pause