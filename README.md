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
### Easy
File `memorial.pfm` in the same directory of the executable.

Use alpha=10 and gamma=0,01 : 
```bash
 .\PhotoGENius.exe memorial.pfm 10 0,01 prova.png
 ```
![](img/prova.png)

Use alpha=0,001 and gamma=2 : 
```bash
 .\PhotoGENius.exe memorial.pfm 0,001 2 prova2.png
 ```
![](img/prova2.png)


### Medium
### Advanced

Full documentation available in the [UserManual](UserManual).

## Requirements

## Installation

## Licence
GPU3.

Read the whole licence [here](LICENCE).
