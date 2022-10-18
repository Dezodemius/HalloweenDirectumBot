job("Example") {
    requirements {
        workerType = WorkerTypes.SPACE_CLOUD_UBUNTU_LTS_REGULAR
    }

    host("Run script") {
        shellScript {
            content = "echo Hello World!"
        }
    }
}
job(".NET Core desktop. Build, test, publish"){
     container(image = "mcr.microsoft.com/dotnet/sdk:6.0"){
         shellScript {
             content = """
                 echo Run build...
                 dotnet build HalloweenDirectumBot.sln
             """
         }
     }
 }