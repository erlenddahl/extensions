# ConsoleUtilities

A collection of utilities for building console applications in .NET. Provides a real-time information panel, ASCII progress bars, a command interpreter, and helpers for console configuration and text formatting.

---

## Table of Contents

- [ConsoleProgressBar](#consoleprogressbar)
- [ConsoleInformationPanel](#consoleinformationpanel)
  - [Info items](#info-items)
  - [Progress bars](#progress-bars)
  - [Unknown-progress bars](#unknown-progress-bars)
  - [Logging](#logging)
  - [Snapshots](#snapshots)
  - [Run extension](#run-extension)
- [ConsoleCommandInterpreter](#consolecommandinterpreter)
- [ConsoleConfigHelper](#consoleconfighelper)
- [ConsoleEx](#consoleex)

---

## ConsoleProgressBar

A single-line ASCII progress bar that animates in place. It displays percentage, item counts, elapsed time, and estimated time remaining.

```csharp
using (var pb = new ConsoleProgressBar("Processing files ...", max: 1000))
{
    for (var i = 0; i < 1000; i++)
    {
        DoWork(i);
        pb.Increment();
    }
} // Automatically finishes and prints the final state on Dispose
```

You can also drive it via `IProgress<double>` (value in the range 0.0–1.0):

```csharp
using (var pb = new ConsoleProgressBar("Downloading ...", max: 100))
{
    pb.Report(0.25); // 25 %
    pb.Report(0.50); // 50 %
    pb.Report(1.00); // Finishes the bar
}
```

Setting `Count` or `Max` directly recalculates the displayed progress:

```csharp
var pb = new ConsoleProgressBar("Building index ...", max: 500);
pb.Count = 250; // Updates to 50 %
pb.Max  = 1000; // Adjusts the denominator and recalculates
pb.Finish();
```

---

## ConsoleInformationPanel

`ConsoleInformationPanel` occupies the entire console window and refreshes it at a fixed interval (twice per second). It displays named items (counters, strings, progress bars) laid out automatically in columns and full-width rows.

```csharp
using var cip = new ConsoleInformationPanel("My Application");

cip.Set("Status", "Starting up");
cip.Set("Files processed", 0);

foreach (var file in files)
{
    ProcessFile(file);
    cip.Increment("Files processed");
    cip.Set("Status", "Working ...");
}
// Dispose finishes all active progress bars and draws the final state
```

### Info items

**Set** creates or updates a named value. Overloads accept `int`, `long`, `double`, and `string`.

```csharp
cip.Set("Items",   42);
cip.Set("Score",   3.14);
cip.Set("Message", "All good");
cip.Set("Detail",  "A longer description shown on its own line", fullWidth: true);
```

The optional `sequence` parameter controls the display order:

```csharp
cip.Set("Step 1", "Done",    sequence: 1);
cip.Set("Step 2", "Running", sequence: 2);
cip.Set("Step 3", "Pending", sequence: 3);
```

**Increment** adds to an existing numeric item, or creates a new `int` item starting from the given amount:

```csharp
cip.Increment("Errors");          // +1
cip.Increment("Bytes read", 512); // +512
```

**Max** keeps the highest value seen so far:

```csharp
foreach (var latency in measurements)
    cip.Max("Peak latency (ms)", latency);
```

**GetOrCreate** returns a typed item that can be updated directly:

```csharp
var counter = cip.GetOrCreate("Retries", defaultValue: 0);
counter.Value++;
```

**Remove** deletes an item from the panel:

```csharp
cip.Remove("Temporary status");
```

### Progress bars

`SetProgress` creates or updates a full-width progress bar. It shows percentage, item counts, elapsed time, and estimated time remaining.

```csharp
var pb = cip.SetProgress("Importing rows", max: totalRows);

foreach (var row in rows)
{
    Import(row);
    pb.Increment();
}

pb.Finish(); // or: cip.FinishProgress("Importing rows");
```

Using it in a `using` block calls `Finish` automatically on disposal:

```csharp
using (var pb = cip.SetProgress("Exporting", max: items.Count))
{
    foreach (var item in items)
    {
        Export(item);
        pb.Increment();
    }
}
```

You can track multiple operations simultaneously:

```csharp
var pbFiles   = cip.SetProgress("Files",   max: fileCount);
var pbRecords = cip.SetProgress("Records", max: recordCount);

foreach (var file in files)
{
    foreach (var record in file.Records)
    {
        Process(record);
        pbRecords.Increment();
    }
    pbFiles.Increment();
}
```

Completed bars are hidden automatically after 5 seconds when `HideOldProgressBars` is `true` (the default).

### Unknown-progress bars

When the total item count is not known in advance, use `SetUnknownProgress`. This displays a bouncing animation with elapsed time only.

```csharp
using (var pb = cip.SetUnknownProgress("Fetching data"))
{
    var data = await FetchFromApi();
}
```

### Logging

`Log` appends timestamped lines to a full-width, multi-line text item.

```csharp
cip.Log("Events", "Application started");

// Later:
cip.Log("Events", "Connected to database");
cip.Log("Events", "Processing complete");
```

Timestamps are prepended in `yyyy-MM-dd HH:mm:ss.fff` format by default.

### Snapshots

`GetSnapshot` captures the current panel state as a serialisable object. This is useful for persisting run summaries.

```csharp
var snapshot = cip.GetSnapshot();

foreach (var (key, value) in snapshot.Info)
    Console.WriteLine($"{key}: {value}");

foreach (var (key, progress) in snapshot.Progress)
    Console.WriteLine($"{key}: {progress.Current} / {progress.Max}");
```

### Run extension

The `ConsoleInformationPanelExtensions.Run` methods wrap a `foreach` loop, creating and updating a progress bar automatically.

With a known count:

```csharp
foreach (var item in cip.Run("Processing", items))
{
    Process(item);
}
```

With an explicit count (for lazy enumerables):

```csharp
foreach (var item in cip.Run("Processing", lazySource, count: 5000))
{
    Process(item);
}
```

Passing `null` for `cip` makes the method act as a plain enumerator, which is convenient for optional panels:

```csharp
ConsoleInformationPanel cip = verbose ? new ConsoleInformationPanel() : null;

foreach (var item in cip.Run("Step", items))
    Process(item);
```

**Timing** an operation and accumulating the total elapsed milliseconds into a named item:

```csharp
using (cip.Time("Total parse time (ms)"))
{
    Parse(data);
}
```

---

## ConsoleCommandInterpreter

Provides an interactive read-eval-print loop. Define commands by subclassing `ConsoleCommand`, then pass them to `ConsoleCommandInterpreter`.

```csharp
public class GreetCommand : ConsoleCommand
{
    public GreetCommand() : base("greet")
    {
        Description = "Prints a greeting.";
        AddParameter("name",  ConsoleCommandParameterType.String);
        AddParameter("times", ConsoleCommandParameterType.Integer);
    }

    public override void Run(List<string> parameters)
    {
        var name  = GetString(parameters,  "name");
        var times = GetInt(parameters, "times");

        for (var i = 0; i < times; i++)
            Console.WriteLine($"Hello, {name}!");
    }
}
```

Register commands and start the loop:

```csharp
var interpreter = new ConsoleCommandInterpreter(new GreetCommand());
interpreter.Listen();
```

The interpreter provides built-in `help` and `exit` commands. Typing `help greet` prints detailed parameter documentation; typing `help` lists all commands.

Use `AutoDetect` to register every `ConsoleCommand` subclass found in the calling assembly:

```csharp
var interpreter = new ConsoleCommandInterpreter();
interpreter.AutoDetect();
interpreter.Listen();
```

Pipe multiple commands on one line with `|`:

```
greet Alice 3 | greet Bob 1
```

Handle unexpected exceptions without crashing the loop:

```csharp
interpreter.CommandException = ex =>
{
    Console.WriteLine("Unexpected error: " + ex.Message);
};
```

Run a single command string programmatically:

```csharp
interpreter.RunCommand("greet \"John Doe\" 2");
```

---

## ConsoleConfigHelper

Designed for applications that accept one or more JSON config file paths as command-line arguments, parse each file, and run the resulting `IRunnable` objects in sequence.

Implement `IRunnable` in your config class:

```csharp
public class AppConfig : IRunnable
{
    public string InputFile  { get; set; }
    public string OutputFile { get; set; }

    public void Run()
    {
        // Application logic here
    }
}
```

Wire it up in `Program.cs`:

```csharp
new ConsoleConfigHelper(args)
    .AutoResize()                              // Optional: resize the console window to 180x40
    .Run("My App", path =>
        JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(path)))
    .PrintSummary();                           // Prints per-file success / failure report
```

Resize the window to a specific size without the fluent helper:

```csharp
ConsoleConfigHelper.SetWindowSize(width: 200, height: 50);
```