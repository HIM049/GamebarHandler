# GamebarHandler
## What does this do?
Gets rid of those pesky 'open ms-gamebar' and 'open ms-gamingoverlay' popups, as seen below:

![image](https://github.com/user-attachments/assets/6819f8c6-5919-4444-9b20-75d612333c4f)

![image](https://github.com/user-attachments/assets/72e7e4d4-9452-4c56-b1b2-959d91728379)

## How does it work?
When you don't have the gamebar and Xbox gaming overlay installed, Windows still tries to call them do to various things (for some reason?). By running this program, you tell Windows "Hey, this program can handle those!". So then when Windows wants to open the gamebar or gaming overlay, it opens this program (instead of showing the popups above), which instantly (and silently!) closes. \ o /

## How do I use it?
Download the zip file from the [releases](https://github.com/Aida-Enna/GamebarHandler/releases) page, then extract it somewhere and run GamebarHandler.exe. <ins>**Please note that you will need to keep the exe file somewhere on your machine**</ins>, otherwise Windows will bring the popups back. If you ever move it, just run it again and it'll tell Windows where it is now.

## How do I know this is safe to use?
The source code is available, you can feel free to build it yourself if you like. Feel free to virus scan it with your favorite tool (like [virtustotal.com](http://virtustotal.com)!)
