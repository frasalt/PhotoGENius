# PhotoGENius - _Your wish, my duty_
### Photorealistic images generation

![](logoPGEN.png) 

<!-- add here a funny but explanatory image, maybe one of a genius! -->

## Features

In its second release, PhotoGENius can:
- read a PFM file and convert it to a PNG file, given an output luminosity parameter `alpha` and a calibration factor `gamma`.
- generate demonstrative scene in both PNG and PFM format, given a set of options, including view angle.
- assemble different frames to generate a simple animation.

## Examples
Easy to use: go to PhotoGENius\PhotoGENius directory.

To **convert file**, type
```bash
dotnet run PhotoGENius pfm2png PFM_FILE_PATH/NAME <options>
```
Pay attention that if your computer is set on Italian language, you may need to write floating-point parameters with a comma instead of a dot (e.g. 1,3 instead of 1.3).

To **generate demo image**, type
```bash
dotnet run PhotoGENius demo <options>
```
Type anything as option to show further usage information.

To **generate frames**, type
```bash
./generate-frames.sh
```
You can easily adapt the script to your needs and make it executable (chmod +x on Linux and MacOS).

To **assemble video**, install ffmpeg and type
```bash
./generate-video.sh
```

### Easy
Use alpha=10 and gamma=0.1 : 
```bash
 dotnet run PhotoGENius pfm2png --input-pfm memorial.pfm --factor 10 --gamma 0.1 --output-png prova.png
 ```
![](PhotoGENius/prova.png)

Use alpha=0.01 and gamma=2 : 
```bash
 dotnet run PhotoGENius pfm2png --input-pfm memorial.pfm --factor 0.01 --gamma 2 --output-png prova2.png
 ```
![](PhotoGENius/prova2.png)


<!---
### Medium
### Advanced
--->

Full documentation available in the [UserManual](UserManual).

## Requirements
PhotoGENius can be used on Windows, Linux and MacOS systems.
It requires dotnet 6.0 to run.
To assemble generated frames in a mp4 video via out script (generate-video.sh) you need 
to install ffmpeg (for example in the same directory as dotnet).

## Installation

## Licence
GPU3.

Read the whole licence [here](LICENCE).
