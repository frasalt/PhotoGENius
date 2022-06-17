# Angle with three digits, e.g. angle="1" → angleNNN="001"
angleNNN=$(printf "%03d" $1) # expects a number as argv
dotnet run -- render \
      --width 400 --height 300 \
      --max-depth 3 \
      --lum-fac 0.6 \
      --gamma-fac 1.8 \
      --angle-deg $1 \
      --sample-per-pixel 4 \
      --camera-type perspective \
      --file-name ../InputSceneFiles/SELF_EXPLAINED.txt \
      --png-output ../Media/imgs_png/\img$angleNNN.png \
      --pfm-output ../Media/imgs_pfm/\img$angleNNN.pfm 

     