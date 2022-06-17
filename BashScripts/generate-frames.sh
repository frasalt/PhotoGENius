for i in $(seq 0 39); do
  iNNN=$(printf "%03d" $i)
  dotnet run -- render \
        --width 150 --height 100 \
        --max-depth 2 \
        --lum-fac 0.6 \
        --gamma-fac 1.8 \
        --angle-deg 0 \
        --sample-per-pixel 4 \
        --camera-type perspective \
        --png-output ../Media/imgs_png/\img$iNNN.png \
        --file-name ../BashScripts/serial/input$iNNN.txt
done
     