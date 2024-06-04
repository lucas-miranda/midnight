#!/bin/sh
set -e

OUTPUT_PATH=$1

# ansi codes
MAGENTA="\e[35m"
WHITE_BOLD="\e[1;97m"
CLEAR_CODE="\e[0m"

# build shaders
echo -e "$MAGENTA>$CLEAR_CODE Shaders"
make --file scripts/BuildShaders.mk OUTPUT_PATH="$(pwd)/$OUTPUT_PATH" INPUT_PATH="$(pwd)/src/assets/graphic/shader/hlsl" #--trace -d

exit 0
