# ViscaLibrary
Implementation of VISCA serial protocol in C# as .NET Library

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [TODO](#todo)

## General info
Currently covers 70% of the Visca protocol commands, mostly operational perspective.
Project target was to create C# OOP library for use in Crestron based control systems.
	
## Technologies
Project is created with:
* VisualStudio 2019: targeting .NET 4.7.2 or higher
* VisualStudio 2008: targeting .NET CompactFramework with Crestron S#Pro libraries as dependancy
	
## Setup
To use run this library as dependancy for your projects, pull compiled release using nuget:
- `nuget install .\packages.config -OutputDirectory .\packages -excludeVersion`.

Nuget.exe is available at [nuget.org](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe).

## TODO
- [ ] Address setup commands
- [ ] Model query commands
- [ ] Vendor specific commands

