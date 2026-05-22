# Test-Coverage.NET

### Modern CLI Utility for .NET Code Coverage and Reporting

[![.NET 8](https://img.shields.io/badge/.NET-8.0-blueviolet?logo=dotnet)](https://dotnet.microsoft.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Test-Coverage.Net.svg)](https://www.nuget.org/packages/Test-Coverage.Net)

**[Report a Bug](https://github.com/Milton-Ngala/Test-Coverage.NET/issues)** · **[Request a Feature](https://github.com/Milton-Ngala/Test-Coverage.NET/issues)**
---

## Overview

**Test-Coverage.NET** is a streamlined, professional command-line interface (CLI) utility built to automate code coverage collection and HTML report generation for .NET projects. It wraps complex background processes into simple, memorable developer commands.

The utility is designed to integrate seamlessly into both local developer inner-loops and automated CI/CD pipelines, making coverage tracking a repeatable, friction-free part of your software development lifecycle.

---

## Features

| Capability | Description |
|---|---|
| **Dependency Automation** | Installs and validates backend global tools (`dotnet-coverage`, `reportgenerator`) with a single command |
| **Workspace Validation** | Automatically checks target directory paths before starting intensive test loops |
| **Smart Coverage Runs** | Automatically executes `dotnet build` and runs tests under a Cobertura collection umbrella |
| **HTML Report Engine** | Compiles messy raw XML coverage outputs into human-readable, interactive HTML reports |
| **Cross-Platform Browser Launch** | Detects your host OS (Windows, Linux, macOS) to open finished reports automatically |
| **Pipeline-Ready** | Returns deterministic exit codes (`0` for success, `1` or error for failure) perfect for build scripts |

---

## Tool Mechanics
```
test-coverage collect 
├── 1. Path Verification & Validation
├── 2. Native Build (dotnet build)
├── 3. Coverage Analysis (dotnet-coverage collect --output-format cobertura)
└── 4. Report Transformation (reportgenerator -reporttypes:Html)
└── Final Output: CoverageReport/index.html ──> Auto-opens in system browser
```

---

## Getting Started

### Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) or later installed on your system host.

### Installation

Install the utility globally directly via the public NuGet registry:

```bash
dotnet tool install --global Test-Coverage.Net
```
Once installed, execute the onboarding workflow to download the necessary background global engines:

```Bash
test-coverage install
```
Usage
Collect test coverage metrics for any solution or project workspace using the core execution syntax:

```Bash
test-coverage collect <path-to-solution-or-project> [--open] [--threshold <percent>]
```
Examples
Standard Collection: Run coverage testing in the current directory and display the interactive HTML report in your default browser:

```Bash
  test-coverage collect . --open
  ```
Enforce Quality Thresholds: Run coverage testing and drop a failing status code if code coverage metrics drop below your target floor:

```Bash
  test-coverage collect . --threshold 80
  ```
### CI/CD Integration
This utility functions natively inside continuous integration automation runners. Below is a structured setup pattern for GitHub Actions workflows:

```YAML
steps:
  - name: Checkout Source Code
    uses: actions/checkout@v4

  - name: Setup .NET SDK
    uses: actions/setup-dotnet@v4
    with:
      dotnet-version: 8.0.x

  - name: Install Tool Ecosystem
    run: |
      dotnet tool install --global Test-Coverage.Net
      test-coverage install

  - name: Execute Coverage Analysis
    run: test-coverage collect . --threshold 80

  - name: Publish HTML Report Artifacts
    uses: actions/upload-artifact@v4
    with:
      name: code-coverage-report
      path: CoverageReport
 ```     
### Project Structure
```
Test-Coverage.NET
├── .github
│   └── workflows
│       └── publish.yml       # GitHub Actions automated semantic deployment pipeline
├── CoverageTool.csproj       # Global .NET Tool compilation metadata configurations
├── Program.cs                # App entrypoint, CLI route handling, and process orchestrations
└── README.md                 # Project documentation and platform guides
```
### Contributing
Contributions, feature requests, and optimizations are highly welcome! If you run into issues or want to submit enhancements:

1) Fork the repository.
2) Create a feature branch (`git checkout -b feature/AmazingFeature`).
3) Commit your changes with clear, descriptive testing data.
4) Open a pull request against the main branch.

### License
Distributed under the MIT License. See LICENSE for more details.

Copyright (c) 2026 Milton Ngala