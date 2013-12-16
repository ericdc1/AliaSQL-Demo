@echo "Run build.bat first!!!"
pause

start /d "C:\Program Files\Internet Explorer\iexplore.exe" http://localhost:8080

"c:\Program Files\IIS Express\iisexpress.exe" /path:%cd%\build\latest\web\
pause
