## Examples

### PFM to PGN convertion
Alpha=10 and gamma=0.1 : 
```bash
dotnet run PhotoGENius pfm2png --input-pfm memorial.pfm --factor 10 --gamma 0.1 --output-png prova1.png
 ```
![](PhotoGENius/prova1.png)

Alpha=0.01 and gamma=2 : 
```bash
dotnet run PhotoGENius pfm2png --input-pfm memorial.pfm --factor 0.01 --gamma 2 --output-png prova2.png
 ```
![](PhotoGENius/prova2.png)

### Generate a brief animation

To **generate frames**, type
```bash
./generate-frames.sh
```
You can easily adapt the script to your needs and make it executable (chmod +x on Linux and MacOS).

To **assemble video**, after installing [ffmpeg](https://www.ffmpeg.org/download.html), type
```bash
./generate-video.sh
```
This is the result with 90 frames at angles from 0° to 89°:


https://user-images.githubusercontent.com/98329317/169061205-ffaa3a56-441c-4f3c-b908-f2402b0916b9.mp4


![](https://github.com/frasalt/PhotoGENius/blob/fd2d075096b5d7245f448f3c12d129136049598c/PhotoGENius/video/animation.mp4)

## Full documentation
See the [UserManual](UserManual).

## Requirements
PhotoGENius can be used on Windows, Linux and MacOS systems.\
It requires dotnet 6.0 to run.\
To assemble generated frames in a mp4 video via out script (generate-video.sh) you need 
to install ffmpeg (for example in the same directory as dotnet).

## Installation

## Licence
GPU3.

Read the whole licence [here](LICENCE).
