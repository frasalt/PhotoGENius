for i in $(seq 0 39); do
  iNNN=$(printf "%03d" $i)
  dotnet run PhotoGENius render --width 300 --height 250 --png-output imgs_png/\img$iNNN.png --max-depth 2 --sample-per-pixel 4 --file-name ../../../Desktop/video/script$iNNN.txt
done
     