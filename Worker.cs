namespace CopyFileToWindowsService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly FileSystemWatcher _watcher;
    private readonly ConfigLoad _configLoad;
    private List<CopyModel> _filesToCopy;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
        _filesToCopy = new List<CopyModel>();

        _configLoad = new ConfigLoad();
        _configLoad.LoadJson();

        Directory.CreateDirectory(_configLoad.Model.CopyToPath);

        _watcher = new FileSystemWatcher(_configLoad.Model.WatchPath, _configLoad.Model.WatchFilter);
        Watch();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
           // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(5000, stoppingToken);
            CopyFiles();
        }

        _watcher.Dispose();
    }

    private void Watch()
    {
        _watcher.Created += (o, e) =>
        {
            var fileToCopy = new CopyModel 
            {
                FullSourcePath = e.FullPath
            };
            _filesToCopy.Add(fileToCopy);
        };
        // watcher.Deleted += (o, e) =>
        // {
        //     Console.WriteLine("Deleted:" + e.FullPath);
        // };
        // watcher.Renamed += (o, e) =>
        // {
        //     Console.WriteLine("Renamed:" + e.FullPath);
        // };
        // watcher.Changed += (o, e) =>
        // {
        //     Console.WriteLine("Changed:" + e.FullPath);
        // };
      
        _watcher.EnableRaisingEvents = true;
    }

    private void CopyFiles() {

        var files = _filesToCopy.Select(x => x).ToArray();
        foreach (var file in files)
        {
            try
            {
                var targetPath = Path.Combine(_configLoad.Model.CopyToPath, Path.GetFileName(file.FullSourcePath));
                File.Copy(file.FullSourcePath, targetPath);  
                _filesToCopy.Remove(file);  
            }
            catch (FileNotFoundException)
            {
                _filesToCopy.Remove(file); 
            }
            catch (IOException)
            {
                var targetPath = Path.Combine(_configLoad.Model.CopyToPath, Path.GetFileName(file.FullSourcePath));
                File.Delete(targetPath);
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation("Erro: {error} {time}", ex.Message, DateTimeOffset.Now);
            }
            
        }
    }
}
