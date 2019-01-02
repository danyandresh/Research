#!/bin/bash

shopt -s nullglob
shopt -s nocaseglob

ol=$(pwd)
pictures="/Users/daniel/Pictures"
#pictures="/Volumes/Seagate BKP/PersonalMedia/Iphone Export Dani"
cd "$pictures"
pwd

for d in *2017/ ; do
  echo "  deleted $d"
  rm -vrf "$d"
done

for d in */ ; do
  break;
  cd "$d"
  echo $d
  for d1 in */ ; do
    echo "  subfolder found $d1"
  done

  for file in *[^\)].*; do
    filename=$(basename -- "$file")
    extension="${filename##*.}"
    filename="${filename%.*}"
    extlower="$(echo $extension | tr '[A-Z]' '[a-z]')"
    #echo "extension $extlower"
    for duplicate in $filename\ \(*\).$extlower; do
      echo "  deleted $duplicate"
      #if cmp "$file" "$duplicate"; then
      #  echo $file " -> " $duplicate
      rm -vf "$duplicate"
      #fi
    done

    extupper="$(echo $extension | tr '[a-z]' '[A-Z]')"
    #echo "extension $extupper"
    for duplicate in $filename\ \(*\).$extupper; do
      echo "  deleted $duplicate"
      rm -vf "$duplicate"
    done


  done

  cd ..

done

cd $ol
