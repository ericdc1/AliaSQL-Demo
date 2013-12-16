rem @echo off

set /p projectName="New Project Name: " %=%

@echo Renaming 'Demo' to '%projectName%'
::@echo %CD%
cd ../Source
call:renameFiles

ren Demo.Website %projectName%.Website
ren Demo.UnitTests %projectName%.UnitTests
ren Database.Demo  Database.%projectName%

cd ../
::@echo %CD%
::find/replace in root directory (no recursion to avoid the __Rename Utility__ directory)
"__Rename Utility__/fart.exe" -- * "Demo" %projectName%
::rename files in root directory (no recursion to avoid the __Rename Utility__ directory)
"__Rename Utility__/fart.exe" -f -- * "Demo" %projectName%
::need this for renaming directories

@echo.
@echo Done and done!!
pause

::pause
goto:eof
:: ----- Functions -----

:renameFiles
::echo %CD%
::find/replace in project subdirectories
"../__Rename Utility__/fart.exe" -r -- * "Demo" %projectName%

::rename files in project subdirectories
"../__Rename Utility__/fart.exe" -r -f -- * "Demo" %projectName%

goto:eof