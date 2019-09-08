# app-playlist-dotnet
 projeto para sugerir uma playlist baseado na temperatura de uma cidade

 1) Estrutura foi criada em .NET Utilizando C# -> WebAPI 02 Framework 4.6.1

 2) O Controller possui Outputcache de 5 minutos para melhorar a performance de muitos acessos.

 3) Para rodar a API, deverá realizar os seguintes passos:
    * Baixar os arquivos para sua maquina local
    * Clicar 2 vezes no arquivo "app-playlist-dotnet.sln" (.SLN). O arquivo estará na raíz do projeto
    * Ao abrir o projeto no visual studio, Clique no botão de start (IIS Express)
    * Ao rodar o projeto ele ira abrir em um browser com uma porta localhost
    * coloque a seguinte URL para mostrar o resultado http://localhost:{port}/api/Playlist/GetPlayListByCidade?cidade={nomeCidade}

4) Parav visualizar a documentação da API:
    * Ao rodar a API acesse no menu -> API : https://localhost:{port}/Help

5) Sugestão em abrir o projeto no VS 2019 ou 2017
