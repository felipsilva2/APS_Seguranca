using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO;
using System;
using System.Text;

namespace APS_Seguranca
{
    class Program
    {
        // Chave de criptografia
        public const string SenhaChave = @"000102030405060708090A0B0C0D0E0F";
        
        /// args[0] - Endereço do arquivo a ser criptografado ou descriptografado
        /// args[1] - CRYPT ou DECRYPT (parâmetro para saber qual função executar)
        static void Main(string[] args)
        {
            try
            {
                // Validação se os argumentos foram imputados pelo usuário
                if (args != null)
                {
                    // Seleção da função escolhida pelo usuário
                    switch (args[1])
                    {
                        case "CRYPT": 
                            EncriptarArquivo(args[0]); // Função para encriptar um arquivo
                            break;
                        case "DECRYPT":
                            DesencriptarArquivo(args[0]); // Função para desencriptar um arquivo
                            break;
                        default: Console.WriteLine("Comando inválido");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Comando inválido");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }
        }
        
        private static void EncriptarArquivo(string inputFile)
        {
            // Printar status do andamento do processo no console
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Iniciando a criptografia do arquivo: " + inputFile);
            
            // Gerando bytes aleatóriamente para criptografia
            byte[] bytesAleatoriamente = GerarBytesAleatoriamente();

            // Criando o arquivo de saída 
            FileStream fsCrypt = new FileStream(inputFile + ".aes", FileMode.Create);

            // Convertendo a chave de string para array de bytes
            byte[] senhaBytes = System.Text.Encoding.UTF8.GetBytes(SenhaChave);

            // Definir o algoritmo de criptografia simétrica de Rijndael
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

            // Gerar um hash com a senha e bytes gerados
            var Chave = new Rfc2898DeriveBytes(senhaBytes, bytesAleatoriamente, 50000);
            AES.Key = Chave.GetBytes(AES.KeySize / 8);
            AES.IV = Chave.GetBytes(AES.BlockSize / 8);

            // Escrever os bytes gerados no início do arquivo a ser criptografado
            fsCrypt.Write(bytesAleatoriamente, 0, bytesAleatoriamente.Length);
            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);
            FileStream fsIn = new FileStream(inputFile, FileMode.Open);

            // Declaração de um buffer de 1 MB
            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                // Escrever o arquivo
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }

                // Finalizar processo de escrita do arquivo
                fsIn.Close();
                
                // Printar status do andamento do processo no console
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Arquivo criptografado com sucesso: " + inputFile);
            }
            catch (Exception ex)
            {
                // Printar status do andamento do processo no console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro na criptografia do arquivo: " + inputFile);
            }
            finally
            {
                // Finalizar processo total
                cs.Close();
                fsCrypt.Close();
            }
        }

        private static void DesencriptarArquivo(string inputFile)
        {
            // Convertendo a chave de string para array de bytes
            byte[] senhaBytes = System.Text.Encoding.UTF8.GetBytes(SenhaChave);
            
            // Declaração de um array de byte com 32 posições
            byte[] arrayBytes = new byte[32];

            // Início do processo de descriptografia
            FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);
            
            // Lendo os bytes do arquivo
            fsCrypt.Read(arrayBytes, 0, arrayBytes.Length);

            // Definir o algoritmo de descriptografia simétrica de Rijndael
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            
            // Gerando o hash para para descriptografia no formato abaixo
            var key = new Rfc2898DeriveBytes(senhaBytes, arrayBytes, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;

            // Escrever os bytes gerados no início do arquivo a ser descriptografia
            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateDecryptor(), CryptoStreamMode.Read);

            // Criando o arquivo de saída 
            FileStream fsOut = new FileStream(inputFile + ".decrypt", FileMode.Create);

            // Declaração de um buffer de 1 MB
            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                // Escrever o arquivo
                while ((read = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsOut.Write(buffer, 0, read);
                }
                
                // Printar status do andamento do processo no console
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Arquivo descriptografia com sucesso: " + inputFile);
            }
            catch (System.Security.Cryptography.CryptographicException ex_CryptographicException)
            {
                // Printar status do andamento do processo no console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro na descriptografia do arquivo: " + inputFile);
            }
            catch (Exception ex)
            {
                // Printar status do andamento do processo no console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro na descriptografia do arquivo: " + inputFile);
            }

            try
            {
                // Finalizar processo
                cs.Close();
            }
            catch (Exception ex)
            {
                // Printar status do andamento do processo no console
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erro na descriptografia do arquivo: " + inputFile);
            }
            finally
            {
                // Finalizar processo total
                fsOut.Close();
                fsCrypt.Close();
            }
        }
        
        // Gerar bytes aleatóriamente
        private static byte[] GerarBytesAleatoriamente()
        {
            // Declaração de um array de byte com 32 posições
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                // Loop para embaralhar os bytes
                for (int i = 0; i < 10; i++)
                {
                    // Preencher o provider RNGCryptoServiceProvider
                    rng.GetBytes(data);
                }
            }
            
            return data;
        }
    }
}
