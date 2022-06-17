  ffmpeg -y -r 15 -f image2 -s 300x250 -i imgs_png/\img%03d.png \
      -vcodec libx264 -pix_fmt yuv420p \
      video/\animation.mp4
 