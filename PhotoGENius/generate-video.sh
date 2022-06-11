# -r 25: Number of frames per second

for frame_index in $(seq 1 17); do
  #frameNNN=$(printf "%02d" $frame_index)

  #ffmpeg -y -r 25 -f image2 -s 640x480 -i imgs_png/\img%03d.png \
  #  -vcodec libx264 -pix_fmt yuv420p \
  #  video/\animation.mp4
  
  ffmpeg -y -r 5 -f image2 -s 300x200 -i %02d.png \
    -vcodec libx264 -pix_fmt yuv420p \
    video/\animation.mp4
done