# NuPhy Air75 Controller

Reverse-engineering and implementation of the NuPhy Air75 v1 keyboard control protocol.  
A C# (WinUI 3) application to configure RGB lighting, modes, brightness, speed, and other firmware parameters without vendor software.

---

## ✨ Features

- Change RGB lighting colors and effects.
- Adjust brightness and animation speed.
- Control mode-specific settings (static, neon, wave, etc.).
- Toggle and configure advanced options such as debounce.
- Works without installing any vendor drivers — uses standard HID.

---

## 🛠 Technical details

- Written in **C#** with **WinUI 3**.
- Communicates with the keyboard via **HID feature reports**.
- Fully documented byte structure for color frames.
- Supports sending raw HID packets to replicate and extend vendor software capabilities.

---

## 📋 Current protocol understanding

- **17 lighting modes** mapped.
- Two known color byte orders depending on mode:
  - `G, B, 0x00, R` (neon modes)
  - `B, G, 0x00, R` (static modes)
- Brightness and speed control bytes identified.
- Debounce parameter located but usage TBD.

---

## 🚧 Roadmap

- [ ] Full mapping of all protocol commands.
- [ ] Add per-key lighting editing.
- [ ] Export/import profiles.
- [ ] Cross-platform support (via .NET MAUI or Avalonia).
- [ ] Complete documentation of the protocol for the community.

---

## 📦 Build

1. Install **.NET 9 SDK**.
2. Clone the repository:
   ```sh
   git clone https://github.com/<your-username>/NuPhyAir75Controller.git
    ```
3. Open the solution in Visual Studio 2022.
4. Build and run the WinUI project.
---

## ⚠ Disclaimer

This project is not affiliated with or endorsed by NuPhy.
Use at your own risk — incorrect commands may cause the keyboard to reset or behave unexpectedly.
---