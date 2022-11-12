# COPIADOR DE ARQUIVOS - WINDOWS SERVICE
Windows Service que monitora uma pasta, e quando um arquivo é criado, copia o arquivo criado para uma pasta de destino

A pasta de origem (pasta monitorada) e a pasta de destino do arquivo, bem como quais arquivos devem ser monitorados, estão configurados no arquivo config.json.


## Publicar código do windows service
dotnet publish -c Release -o c:\CopyFileTo.WS

copiar arquivo config.json para c:\Windows\System32

## Criar um windows service
executar CMD como Administrador

sc create "CopyFileTo.WindowsService" binPath= c:\CopyFilesToWS\CopyFileToWindowsService.exe


## Fonte de informações
https://csharp.christiannagel.com/2022/03/22/windowsservice-2/
