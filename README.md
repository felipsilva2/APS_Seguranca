# APS de Segurança da informação

1) Tecnologia utilizada: .Net Core 2.2.
2) Método de criptografia: simétrica de Rijndael.
3) Chave utilizada para embaralhar os bytes: 000102030405060708090A0B0C0D0E0F

Componentes utilizado para o projeto consta no próprio .Net core.

Para executar o programa contemple os passos abaixos:

1) Instalação do .Net Core 2.2 ou superior.
2) Terminal de comando ou o prompt de comando instalados.

Para criptografar um arquivo siga os passos abaixos:

1) Abra o terminal ou prompt.
2) Inclua o arquivo que deseja criptografar no diretório: APS_Seguranca/Arquivos/
3) Insira o seguindo comando dentro do diretório "APS_Seguranca/bin/debug/netcoreapp.2.2/": 

dotnet APS_Seguranca.dll "Endereço do arquivo que deseja criptografar" CRYPT

Exemplo: dotnet APS_Seguranca.dll ~/Documents/Repository/APS_Seguranca/Arquivos/Teste.png CRYPT

Nesse exemplo será gerado um arquivo criptografado com a extensão .aes

Para descriptografar um arquivo siga os passos abaixos:

1) Abra o terminal ou prompt.
2) Inclua o arquivo que deseja descriptografar no diretório: APS_Seguranca/Arquivos/
3) Insira o seguindo comando dentro do diretório "APS_Seguranca/bin/debug/netcoreapp.2.2/": 

dotnet APS_Seguranca.dll "Endereço do arquivo que deseja descriptograr" DECRYPT

Exemplo: dotnet APS_Seguranca.dll ~/Documents/Repository/APS_Seguranca/Arquivos/Teste.png.aes DECRYPT

Nesse exemplo será gerado um arquivo descriptografado com a extensão .aes.decrypt

ATENÇÃO: Só será descriptografado arquivos com a extensão gerada pelo programa.
