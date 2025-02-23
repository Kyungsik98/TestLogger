# Building the Project to .exe

1. Open Terminal in Visual Studio Code
2. Navigate to project directory
3. Run command:
```dotnet publish -c Release -r win-x64 --self-contained true```

The .exe will be created in:
```/bin/Release/net<version>/win-x64/publish/```