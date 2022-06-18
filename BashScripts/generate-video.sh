# Note: doesn't need to cycle over image number, it's automatic

ffmpeg -y '# overwrite video' \
      -r 15 '# frames per second' \
      -f image2 \
      -s 300x250 '# image width and hight' \
      -i ../Media/imgs_png/\img%03d.png '# input files: %03d reads a 3digit number (ascending order)' \
      -vcodec libx264 -pix_fmt yuv420p \
      ../Media/video/\animation.mp4 '# output path'
 