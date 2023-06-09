#!/bin/bash

export EDITOR_PATH=/Applications/Unity/Hub/Editor/2021.3.14f1/Unity.app/Contents/MacOS
export PROJECT_PATH="/Volumes/T7/Unity Projects/_StudentProjects/FYP_Group4_2D/Group4_FYP"
export BUILD_METHOD=BuildHelper.BuildPlayers

export PATH=$PATH:$EDITOR_PATH
echo $PWD/build_osx.log
Unity -batchMode \
 -skipMissingProjectID -skipMissingUPID \
 -buildTarget Standalone -logFile "$PWD/build_osx.log" \
 -projectPath "$PROJECT_PATH" \
 -executeMethod $BUILD_METHOD \
 -quit
read -s -n 1 -p "Press any key to continue..."
