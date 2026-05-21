[+]# Test-Coverage.NET

Test-Coverage.NET is a small, professional CLI utility for collecting code coverage from .NET projects and generating HTML reports. It is designed to integrate into developer workflows and CI pipelines to make coverage collection and reporting easy and repeatable.

Key features
- Simple commands to install required tools and collect coverage
- Generates Cobertura-compatible coverage data and an HTML report
- Works with any solution or project that can be tested by `dotnet test`
- Designed for modern .NET (targets .NET 8+)

Prerequisites
- .NET SDK 8 or later installed
- dotnet global tools: `dotnet-coverage` and `reportgenerator` (the CLI can check for these and will guide you)

Install

Install the CLI as a global .NET tool (optional — you can also run the project directly):

```bash
dotnet tool install --global Test-Coverage.NET
```

Install required coverage/reporting tools (installs global dotnet tools used by the CLI):

```bash
test-coverage install
```

Usage

Collect coverage for a solution or project and generate an HTML report:

```bash
test-coverage collect <path-to-solution-or-project> [--open] [--threshold <percent>]
```

Examples

- Collect coverage for the current directory and open the generated report in the default browser:

```bash
test-coverage collect . --open
```

- Collect coverage and enforce a minimum coverage threshold (exit with error if below threshold):

```bash
test-coverage collect . --threshold 80
```

What the command does
- Validates the workspace path
- Ensures required tools are available (`dotnet-coverage`, `reportgenerator`)
- Builds the target workspace
- Runs `dotnet test` under coverage collection (produces Cobertura XML)
- Generates an HTML report using ReportGenerator (output: CoverageReport/index.html)

CI integration
This tool is suitable for CI pipelines. Typical CI steps:
1. Install .NET SDK
2. Install this tool or run the project directly
3. Run `test-coverage collect` against the repository
4. Publish the generated CoverageReport as CI artifacts or use it for badge generation

Contributing
- Contributions and improvements are welcome. Please open issues or pull requests with clear descriptions and tests where appropriate.

License
See the LICENSE file for license details (if absent, add one such as MIT for permissive usage).

