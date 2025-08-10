# NuPhy Air75 v1 Controller

A reverse-engineered controller and protocol parser for the NuPhy Air75 v1 mechanical keyboard.  
Allows full control over lighting modes, colors, brightness, speed, debounce, and more — all without proprietary software.

## Features

- Change and customize all **17 lighting modes**
- Adjust **brightness**, **mode speed**, and **debounce**
- Support for both **static** and **dynamic** color orders
- Send HID feature reports directly without vendor drivers
- Plan to fully parse the NuPhy Air75 v1 protocol
- WinUI 3 UI planned for easy control

## Technical Details

The program uses standard Windows HID API calls (`hid.dll`, `setupapi.dll`) via P/Invoke.  
It communicates directly with the keyboard’s HID interface (Feature Reports).  
No custom drivers or vendor software are required.

### Color Byte Order
Depending on the mode, the pixel color bytes may be in different orders:
- **Neon/Dynamic modes** → `G, B, reserved, R`
- **Static mode** → `B, G, reserved, R`

### Frame Structure (Static Color Example)
```
[ G ][ B ][ 00 ][ R ] x 251 pixels + [ 16 zero bytes padding ]
```
The first byte of the HID report is always the **Report ID** (0x06).

## Planned Roadmap

- [ ] Reverse-engineer all remaining HID commands
- [ ] Implement profile saving/loading
- [ ] WinUI 3 control panel for keyboard settings
- [ ] Cross-platform support via .NET MAUI

## Repository

GitHub: [NuPhy-Air75-v1-Controller](https://github.com/ElvenBless/NuPhy-Air75-v1-Controller)

---

### Disclaimer
This project is not affiliated with NuPhy.  
Use at your own risk — sending incorrect HID commands may cause the keyboard to behave unexpectedly.