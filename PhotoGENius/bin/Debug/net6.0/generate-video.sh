     for angle in $(seq 0 10); do
         # Angle with three digits, e.g. angle="1" → angleNNN="001"
         angleNNN=$(printf "%03d" $angle)
         start PhotoGENius --width 640 --height 480 --angle-deg $angle --camera-type perspective --png-output imgs/\img$angleNNN.png
     done
     
     # -r 25: Number of frames per second
     start ffmpeg -r 25 -f image2 -s 640x480 -i imgs/\img%03d.png \
         -vcodec libx264 -pix_fmt yuv420p \
         imgs/\spheres-perspective.mp4