for angle in $(seq 0 20); do
  # Angle with three digits, e.g. angle="1" → angleNNN="001"
  angleNNN=$(printf "%03d" $angle)
  dotnet run PhotoGENius render --width 200 --height 150 --angle-deg $angle --max-depth 2 --camera-type perspective --png-output imgs_png/\img$angleNNN.png --pfm-output imgs_pfm/\img$angleNNN.pfm
done
     