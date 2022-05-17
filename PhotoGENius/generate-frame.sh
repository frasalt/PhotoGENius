# Angle with three digits, e.g. angle="1" → angleNNN="001"

angleNNN=$(printf "%03d" $1)
dotnet run PhotoGENius --width 640 --height 480 --angle-deg $1 --camera-type perspective --png-output imgs/\img$angleNNN.png

     