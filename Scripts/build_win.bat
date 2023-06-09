@ECHO OFF

SET EDITOR_PATH=C:\Program Files\Unity\Hub\Editor\2021.3.14f1\Editor
SET PROJECT_PATH=F:\Unity Projects\_StudentProjects\FYP_Group4_2D\Group4_FYP
SET BUILD_METHOD=BuildHelper.BuildPlayers

SET PATH=%EDITOR_PATH%;%PATH%
Unity -batchMode^
 -skipMissingProjectID -skipMissingUPID^
 -buildTarget Standalone -logFile .\build_win.log^
 -projectPath "%PROJECT_PATH%"^
 -executeMethod %BUILD_METHOD%^
 -quit
pause
