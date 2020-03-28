@echo off
echo Running server...
pushd  %0\..\DemoAPI.server
dotnet restore
start cmd /k dotnet run

echo Running client...
pushd  %0\..\DemoAPI.Client
dotnet restore
start cmd /k dotnet run

echo FINISHED
