using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting; // Add this namespace for IWebHostEnvironment
using System.IO;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ChatHub(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    public async Task SendMessage(string ipAddress, string message)
    {       
        // Log the message and IP address to a file
        await LogMessageToFile(ipAddress);

        // Send the message to all clients, including the IP address
        await Clients.All.SendAsync("ReceiveMessage", ipAddress, message);
    }

    private async Task LogMessageToFile(string ipAddress)
    {
        // Define the default file path relative to the application's content root
        var logDirectory = Path.Combine(_hostingEnvironment.ContentRootPath, "Logs");
        var filePath = Path.Combine(logDirectory, "chatlog.txt");

        // Ensure the directory exists
        if (!Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        // Read existing log entries
        IEnumerable<string> logEntries = Enumerable.Empty<string>();
        if (File.Exists(filePath))
        {
            logEntries = await File.ReadAllLinesAsync(filePath);
        }

        // Count the number of messages from each IP address
        var ipMessageCount = logEntries
            .Where(entry => entry.Contains($"IP: {ipAddress}"))
            .Count();

        // Increment message count
        ipMessageCount++;

        // Create new log entry
        var logEntry = $"{System.DateTime.Now} - IP: {ipAddress} - Count: {ipMessageCount}\n";

        // Write the updated log to the file
        await File.AppendAllTextAsync(filePath, logEntry);
    }


}
