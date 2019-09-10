Param(
  [parameter(Mandatory=$false)][string]$repo="https://api.nuget.org/v3/index.json",
  [parameter(Mandatory=$false)][bool]$push=$false,
  [parameter(Mandatory=$false)][string]$apikey,
  [parameter(Mandatory=$false)][bool]$build=$true
)

# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$slnPath = Join-Path $packFolder "../src"
$srcPath = Join-Path $packFolder "../src/Surging.Core"



$projects = (Get-Content "./Components")

function Pack($projectFolder,$projectName) {  
  Set-Location $projectFolder
  $releaseProjectFolder = (Join-Path $projectFolder "bin/Release")
  if (Test-Path $releaseProjectFolder)
  {
     Remove-Item -Force -Recurse $releaseProjectFolder
  }
  
   & dotnet msbuild /p:Configuration=Release /p:SourceLinkCreate=true
   & dotnet msbuild /t:pack /p:Configuration=Release /p:SourceLinkCreate=true
   if ($projectName) {
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $projectName + ".*.nupkg")
   }else {
    $projectName = $project -replace "Core","Cloud"
    $projectPackPath = Join-Path $projectFolder ("/bin/Release/" + $projectName + ".*.nupkg")
   }

   Move-Item -Force $projectPackPath $packFolder 
}

if ($build) {
  Set-Location $slnPath
  & dotnet restore Surging.sln

  foreach($project in $projects) {
    if (-not $project.StartsWith("#")){
      Pack -projectFolder (Join-Path $srcPath $project)
    }    
  }
  $dotnettyCodecDns = Join-Path $slnPath "DotNetty.Codecs/DotNetty.Codecs.DNS" 
  Pack -projectFolder $dotnettyCodecDns -projectName "Surging.Cloud.DotNetty.Codecs.DNS"


  $skyApmTransportGrpcProtocol = Join-Path $slnPath "Surging.Apm/SkyApm.Transport.Grpc.Protocol"
  Pack -projectFolder $skyApmTransportGrpcProtocol -projectName "Surging.Cloud.SkyApm.Transport.Grpc.Protocol"

  $surgingSkywalking = Join-Path $slnPath "Surging.Apm/Surging.Apm.Skywalking"
  Pack -projectFolder $surgingSkywalking -projectName "Surging.Cloud.Apm.Skywalking"
  
  $surgingWebSocektCore = Join-Path $slnPath "WebSocket/Surging.WebSocketCore"
  Pack -projectFolder $surgingWebSocektCore -projectName "Surging.Cloud.WebSocketCore"
  Set-Location $packFolder
}

if($push) {
    if ([string]::IsNullOrEmpty($apikey)){
        Write-Warning -Message "未设置nuget仓库的APIKEY"
		exit 1
	}
	dotnet nuget push *.nupkg -s $repo -k $apikey
}