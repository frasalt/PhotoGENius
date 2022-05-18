# -r 25: Number of frames per second
  ffmpeg -y -r 25 -f image2 -s 640x480 -i imgs_png/\img%03d.png \
    -vcodec libx264 -pix_fmt yuv420p \
    video/\animation.mp4