# Angle with three digits, e.g. angle="1" → angleNNN="001"

angleNNN=$(printf "%03d" $1)
dotnet run PhotoGENius render --width 200 --height 150 --max-depth 2 --angle-deg $1 --camera-type perspective --png-output imgs_png/\img$angleNNN.png --pfm-output imgs_pfm/\img$angleNNN.pfm 

     