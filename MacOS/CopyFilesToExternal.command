#!/bin/bash

shopt -s nullglob

picturesSource="/Users/daniel/Pictures/"
picturesDestination="/Volumes/Seagate BKP/PersonalMedia/Iphone Export Dani/"
#cp -R -n "$picturesSource/" "$picturesDestination"
rsync --ignore-existing --recursive --progress -v "$picturesSource" "$picturesDestination"
