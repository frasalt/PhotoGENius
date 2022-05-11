
     # -r 25: Number of frames per second
     start ffmpeg -y -r 25 -f image2 -s 640x480 -i imgs/\img%03d.png \
         -vcodec libx264 -pix_fmt yuv420p \
         imgs/\animation.mp4