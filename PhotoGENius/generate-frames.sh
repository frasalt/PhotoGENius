for angle in $(seq 0 10); do
  # Angle with three digits, e.g. angle="1" → angleNNN="001"
  angleNNN=$(printf "%03d" $angle)
  dotnet run PhotoGENius demo --width 640 --height 480 --angle-deg $angle --camera-type perspective --png-output imgs_png/\img$angleNNN.png --pfm-output imgs_pfm/\img$angleNNN.pfm
done
     