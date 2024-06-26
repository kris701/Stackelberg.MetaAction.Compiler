<p align="center">
    <img src="https://github.com/kris701/Stackelberg.MetaAction.Compiler/assets/22596587/50aed7eb-2091-4425-8e48-359d3e1c84a9" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/Stackelberg.MetaAction.Compiler/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/Stackelberg.MetaAction.Compiler/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/Stackelberg.MetaAction.Compiler)
![Nuget](https://img.shields.io/nuget/dt/Stackelberg.MetaAction.Compiler)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/Stackelberg.MetaAction.Compiler/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/Stackelberg.MetaAction.Compiler)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--8.0-green)

# Stackelberg Meta Action Compiler
This is a package that can take in a normal PDDL domain and problem as well as a meta action, and compile them into a Stackelberg Variant.
This Stackelberg Variant can then be used to verify the validity of the meta action. This is designed to work with the [Stackelberg-SLS](https://gitlab.com/atorralba_planners/stackelberg-planner-sls/-/tree/main?ref_type=heads) planner.
This is based on the paper [Can I Really Do That? Verification of Meta-Operators via Stackelberg Planning](https://doi.org/10.24963/ijcai.2023/602).
The project is also available as a package on the [Nuget Package Manager](https://www.nuget.org/packages/Stackelberg.MetaAction.Compiler/).
