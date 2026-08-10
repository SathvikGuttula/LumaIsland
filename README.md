<div align="center">

#  LumaIsland

**A Dynamic Island experience for Windows — right at the top of your screen.**

[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/WPF-Desktop-0078D4?style=for-the-badge&logo=windows&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/License-MIT-A855F7?style=for-the-badge)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-Windows%2010%2F11-00ADEF?style=for-the-badge&logo=windows11&logoColor=white)](https://www.microsoft.com/windows)
[![Stars](https://img.shields.io/github/stars/YOUR_USERNAME/lumaIsland?style=for-the-badge&color=7C3AED)](https://github.com/YOUR_USERNAME/lumaIsland/stargazers)

<br/>

*A lightweight, always-on-top overlay that brings smart media detection, an interactive calendar, and contextual widgets to your Windows desktop — inspired by Apple's Dynamic Island.*

<br/>

[Features](#-features) · [Installation](#-installation) · [Build from Source](#-build-from-source) · [Configuration](#%EF%B8%8F-configuration) · [Roadmap](#-roadmap) · [Contributing](#-contributing)

<br/>

---

</div>

<br/>

## 📸 Preview

<div align="center">

| Collapsed State | Expanded State |
|:-:|:-:|
| ![Collapsed](https://via.placeholder.com/400x80/0A0A0C/7C3AED?text=Purple+Pill+%E2%80%A2+Collapsed) | ![Expanded](https://via.placeholder.com/600x200/0A0A0C/FFFFFF?text=Media+%7C+Calendar+%7C+Settings) |

| Onboarding | Settings |
|:-:|:-:|
| ![Onboarding](https://via.placeholder.com/400x300/4C1D95/FFFFFF?text=Welcome+to+LumaIsland) | ![Settings](https://via.placeholder.com/400x300/1B1B1F/FFFFFF?text=Dark+Settings+Panel) |

</div>

> **Note:** Replace the placeholder images above with actual screenshots of your running application.

<br/>

## ✨ Features

### 🎵 Smart Media Detection
- Automatically detects active media sessions on your system
- Works with **Spotify**, **Apple Music**, **YouTube** (Edge/Chrome), and any app exposing Windows Media Transport Controls
- Displays **album artwork**, **track title**, **artist name**
- Full playback controls — **play/pause**, **next**, **previous**

### 🗓 Interactive Calendar
- Compact **week strip** embedded directly in the island
- Click to **select any day** and view a summary
- Shows current month and selected date detail
- Ready for future **Outlook / Google Calendar** sync

### 💊 Collapsed Pill
- Minimal, black pill indicator when collapsed
- Centered at the top of your primary screen
- Subtle glow and gradient animation
- Hover to smoothly expand into the full island

### ⚙️ Fully Customizable
- **Enable/disable widgets** individually (Media, Calendar)
- **Adjust island width** (expanded and collapsed)
- **Launch at startup** toggle
- **Hover to expand** toggle
- **Accent color** configuration
- All settings persist in a local JSON file

### 🖥 Desktop-Native Experience
- **Always-on-top** transparent overlay
- **Hidden from Alt+Tab** — stays out of your way
- **System tray** integration with quick actions
- Smooth **expand/collapse animations** with cubic easing
- Lightweight — built with native WPF, no Electron bloat

### 🚀 First-Run Onboarding
- Beautiful purple gradient onboarding screen
- Feature highlights with glassmorphism cards
- Quick access to launch or configure before first use

<br/>

## 🏗 Tech Stack

| Layer | Technology |
|:--|:--|
| **Framework** | [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) |
| **UI** | [WPF](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/) (Windows Presentation Foundation) |
| **Architecture** | MVVM with [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) |
| **Media Detection** | [Windows Media Session API](https://learn.microsoft.com/en-us/uwp/api/windows.media.control) (`GlobalSystemMediaTransportControlsSessionManager`) |
| **Windows APIs** | [Microsoft.Windows.SDK.Contracts](https://www.nuget.org/packages/Microsoft.Windows.SDK.Contracts) |
| **System Tray** | WinForms `NotifyIcon` |
| **Settings** | `System.Text.Json` → local JSON file |
| **Startup** | Windows Registry (`CurrentVersion\Run`) |

<br/>

## 📦 Installation

### Prerequisites

- **Windows 10** (version 1904 / build 19041) or later
- **Windows 11** recommended for best visual experience
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (included if using self-contained build)

### Download Release

1. Go to the [**Releases**](https://github.com/YOUR_USERNAME/lumaIsland/releases) page
2. Download the latest `LumaIsland-vX.X.X-win-x64.zip`
3. Extract to any folder
4. Run `LumaIsland.exe`

> On first launch, the onboarding window will guide you through setup.

<br/>

## 🔨 Build from Source

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Windows 10 SDK (10.0.19041.0 or later)
- Visual Studio 2022 (recommended) or VS Code with C# extension

### Steps

```bash
# Clone the repository
git clone https://github.com/YOUR_USERNAME/lumaIsland.git
cd lumaIsland

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run
dotnet run
