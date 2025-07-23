# Microsoft Certificate Installer

A simple C# console application to securely download and install a Microsoft certificate (PFX) on your system.

## Features
- Downloads a certificate from a secure Dropbox link
- Installs the certificate into the Windows certificate store
- Cleans up temporary files after installation
- User-friendly console interface with header/footer

## Project Structure
- `Microsoft IT.sln` – Visual Studio solution file
- `Microsoft IT/` – Main project directory
  - `Microsoft IT.csproj` – C# project file
  - `Program.cs` – Main application logic

## How It Works
1. Downloads a PFX certificate from a specified URL
2. Installs the certificate using the provided password
3. Cleans up the downloaded file

## Usage
1. Clone this repository
2. Open `Microsoft IT.sln` in Visual Studio
3. Build and run the project

## Certificate Details
- **Download URL:** (hardcoded in `Program.cs`)
- **Password:** (hardcoded in `Program.cs`)

## Author
Developed by Nur Mohammad Rafi

---
**Note:** This tool is for educational purposes. Do not use with sensitive or production certificates.
