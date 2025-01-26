#File Paths
$MiddlewareSolution = ".\..\Services\MiddleWares\MiddleWares.sln";
$CleaveExceptionHandlerMiddleware = ".\..\Services\MiddleWares\GlobalExceptionHandler\bin\Debug\net8.0\Cleave.Middleware.ExceptionHandler.dll";
$DllDestination = "..\Dlls"

#Create Dlls folder if it does not exist
if(!(Test-Path $DllDestination)){
    Write-Host "DLLs folder Doesnot exist, creating DLLs folder";
    New-Item -ItemType Directory -Path $DllDestination;
    Write-Host "DLLs folder created";
}

#Build Solutions
try{
   dotnet clean $MiddlewareSolution
   dotnet build $MiddlewareSolution --configuration Debug;
   Copy-Item -Path $CleaveExceptionHandlerMiddleware -Destination $DllDestination;
   Write-Host "Cleave.Middleware.ExceptionHandler.dll file copied to DLLs folder";
}
catch{
   Write-Host "didn't copy the dll file to DLLs folder, some error occured while building or moving the file";
}

