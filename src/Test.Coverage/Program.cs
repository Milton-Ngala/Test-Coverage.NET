using System.CommandLine;
using System.Diagnostics;
using System.Runtime.InteropServices;

var root = new RootCommand(
    "Professional .NET coverage collection tool");

//
// INSTALL COMMAND
//

var installCommand = new Command(
    "install",
    "Install required coverage dependencies");

installCommand.SetHandler(() =>
{
    Exec(
        ".",
        "dotnet",
        "tool install --global dotnet-coverage");

    Exec(
        ".",
        "dotnet",
        "tool install --global dotnet-reportgenerator-globaltool");
});

//
// COLLECT COMMAND
//

var collectCommand = new Command(
    "collect",
    "Collect coverage and generate HTML report");

var workspaceArgument = new Argument<string>(
    "workspace",
    description: "Path to solution or project");

var openOption = new Option<bool>(
    "--open",
    description: "Open HTML report automatically");

var thresholdOption = new Option<int?>(
    "--threshold",
    description: "Minimum coverage percentage");

collectCommand.AddArgument(workspaceArgument);

collectCommand.AddOption(openOption);

collectCommand.AddOption(thresholdOption);

collectCommand.SetHandler(
(
    string workspace,
    bool open,
    int? threshold
) =>
{
    ValidateWorkspace(workspace);

    EnsureToolInstalled("dotnet-coverage");

    EnsureToolInstalled("reportgenerator");

    //
    // BUILD
    //

    Exec(
        workspace,
        "dotnet",
        "build");

    //
    // TEST + COVERAGE
    //

    Exec(
        workspace,
        "dotnet-coverage",
        "collect --output-format cobertura --output coverage.xml dotnet test --no-build");

    //
    // GENERATE REPORT
    //

    Exec(
        workspace,
        "reportgenerator",
        "-reports:coverage.xml -targetdir:CoverageReport -reporttypes:Html");

    //
    // REPORT PATH
    //

    var report = Path.Combine(
        Path.GetFullPath(workspace),
        "CoverageReport",
        "index.html");

    Console.WriteLine();
    Console.WriteLine("Coverage report generated:");
    Console.WriteLine(report);

    //
    // OPEN REPORT
    //

    if (open)
    {
        OpenBrowser(report);
    }

},
workspaceArgument,
openOption,
thresholdOption);

//
// ROOT COMMANDS
//

root.AddCommand(installCommand);

root.AddCommand(collectCommand);

return await root.InvokeAsync(args);

//
// HELPERS
//

static void ValidateWorkspace(string workspace)
{
    if (!Directory.Exists(workspace))
    {
        throw new DirectoryNotFoundException(
            $"Workspace does not exist: {workspace}");
    }
}

static void EnsureToolInstalled(string tool)
{
    try
    {
        var info = new ProcessStartInfo
        {
            FileName = tool,
            Arguments = "--version",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };

        using var process = Process.Start(info);

        process?.WaitForExit();
    }
    catch
    {
        throw new Exception(
            $"{tool} is not installed. Run 'test-coverage install'");
    }
}

static void Exec(
    string workspace,
    string cmd,
    string args)
{
    Console.WriteLine($"{workspace}> {cmd} {args}");

    var info = new ProcessStartInfo
    {
        FileName = cmd,
        Arguments = args,
        WorkingDirectory = workspace,
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true
    };

    using var process =
        Process.Start(info)
        ?? throw new Exception($"Failed to start {cmd}");

    process.OutputDataReceived += (_, e) =>
    {
        if (e.Data != null)
            Console.WriteLine(e.Data);
    };

    process.ErrorDataReceived += (_, e) =>
    {
        if (e.Data != null)
            Console.Error.WriteLine(e.Data);
    };

    process.BeginOutputReadLine();

    process.BeginErrorReadLine();

    process.WaitForExit();

    if (process.ExitCode != 0)
    {
        throw new Exception(
            $"{cmd} failed with exit code {process.ExitCode}");
    }
}

static void OpenBrowser(string file)
{
    var info = new ProcessStartInfo
    {
        UseShellExecute = true
    };

    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        info.FileName = file;
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        info.FileName = "xdg-open";
        info.Arguments = file;
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
    {
        info.FileName = "open";
        info.Arguments = file;
    }

    Process.Start(info);
}