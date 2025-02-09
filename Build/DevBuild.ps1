#File Paths
$MiddlewareSolution = ".\..\Services\MiddleWares\MiddleWares.sln";
$ActionFilterSolution = ".\..\Services\Filters\Filters.sln";
$AuthenticationTestApiSolution = ".\..\Services\AuthenticationTestApi\AuthenticationTestApi.sln";
$UserManagerSolution = ".\..\Services\UserManagement\UserManager\UserManager.sln";
$NugetPackageDestination = "..\Cleave Packages";

#All Solutions and Nuget package paths
$Solutions = @(
   [SolutionDetails]::new($MiddlewareSolution, @(
      ".\..\Services\MiddleWares\GlobalExceptionHandler\bin\Debug\Cleave.Middleware.ExceptionHandler.1.0.0.nupkg",
      ".\..\Services\MiddleWares\Cleave.Middleware.Authentication\bin\Debug\Cleave.Middleware.Authentication.1.0.0.nupkg"
   )),
   [SolutionDetails]::new($ActionFilterSolution, @(
      ".\..\Services\Filters\Cleave.ActionFilter.Authorization\bin\Debug\Cleave.Filter.Authorization.1.0.0.nupkg"
   )),
   [SolutionDetails]::new($AuthenticationTestApiSolution, @()),
   [SolutionDetails]::new($UserManagerSolution, @())
);

#Create Cleave Packages folder if it does not exist
if(!(Test-Path $NugetPackageDestination)){
    Write-Host "Cleave Package folder Doesnot exist, creating DLLs folder";
    New-Item -ItemType Directory -Path $NugetPackageDestination;
    Write-Host "DLLs folder created";
}

#Build Solutions
foreach($solution in $Solutions){
   try{
      dotnet clean $solution.solutionPath;
      dotnet build $solution.solutionPath --configuration Debug;
      foreach($nugetPath in $solution.nugetPackages){
         if(Test-Path $nugetPath){
            try{
               Copy-Item -Path $nugetPath -Destination $NugetPackageDestination;
               Write-Host "Successfully copied : "+ $nugetPath
            }
            catch{
               Write-Host "Copy Failed : "+$nugetPath;
            }
         }
         else{
            Write-Host "Path does not exist : "+$nugetPath;
         }
      }
   }
   catch{
      Write-Host "didn't copy the nuget file to Cleave Package folder, some error occured while building or moving the file";
   }
}

class  SolutionDetails {
   [string] $solutionPath;
   $nugetPackages;

   SolutionDetails([string] $solutionPath, $nugetPackages){
       $this.solutionPath = $solutionPath;
       $this.nugetPackages = $nugetPackages;
   }
}

