# Note: doesn't need to cycle over image number, it's automatic

ffmpeg -y \
      -r 25 \
      -f image2 \
      -s 1080x820 \
      -i ../Media/imgs_png/\img%03d.png \
      -vcodec libx264 -pix_fmt yuv420p \
      ../Media/videos/\animation.mp4 
 