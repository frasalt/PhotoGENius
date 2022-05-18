# PhotoGENius - _Your wish, my duty_
### Photorealistic images generation

![](logoPGEN.png) 

<!-- add here a funny but explanatory image, maybe one of a genius! -->

## Features

In its first release, PhotoGENius can:
- read a PFM file and convert it to a PNG file, given an output luminosity parameter `alpha` and a calibration factor `gamma`.

## Examples
Extremely easy to use: chose your PFM file to convert and type
```bash
.\PhotoGENius.exe path_to\my_pfm_file_name.pfm <alpha> <gamma> my_png_gile_name.png
```
Pay attention that if your computer is setted on Italian language, you may need to write floating-point parameters with a comma instead of a dot (e.g. 1,3 instead of 1.3).

### Easy
File `memorial.pfm` in the same directory of the executable.

Use alpha=10 and gamma=0.01 : 
```bash
 .\PhotoGENius.exe memorial.pfm 10 0.01 prova.png
 ```
![](img/prova.png)

Use alpha=0.001 and gamma=2 : 
```bash
 .\PhotoGENius.exe memorial.pfm 0.001 2 prova2.png
 ```
![](img/prova2.png)


<!---
### Medium
### Advanced
--->

Full documentation available in the [UserManual](UserManual).

## Requirements
PhotoGENius can be used both on Windows and MacOS systems.
It requires dotnet 6.0 to run.
To assemble generated frames in a mp4 video via out script (generate-video.sh) you need 
to install ffmpeg (for example in the same directory as dotnet).

## Installation

## Licence
GPU3.

Read the whole licence [here](LICENCE).
