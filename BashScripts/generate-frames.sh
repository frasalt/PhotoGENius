for i in $(seq 0 102); do
  iNNN=$(printf "%03d" $i)
  dotnet run --project ../PhotoGENius render \
        --algorithm pointlight \
        --width 1080 --height 810 \
        --max-depth 2 \
        --sample-per-pixel 16 \
        --num-of-rays 10 \
        --file-name ../InputSceneFiles/serial/input$iNNN.txt \
        --png-output ../Media/imgs_png/\img$iNNN.png
done
     