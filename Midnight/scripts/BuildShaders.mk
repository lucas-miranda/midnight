include Rules.mk

# OUTPUT_PATH: where compiled files will be placed
ifeq ($(origin OUTPUT_PATH), undefined)
	$(error Missing OUTPUT_PATH required argument Remember, it must be relative to the Makefile caller.)
endif

# INPUT_PATH: where to find hlsl files to compile
ifeq ($(origin INPUT_PATH), undefined)
	$(error Missing INPUT_PATH required argument. Remember, it must be relative to the Makefile caller.)
endif

# wine prefix path where dx 2010 (with fxc) should be installed
WINE_PREFIX_NAME := x86
WINE_PREFIX_PATH := $(HOME)/.local/share/wineprefixes/$(WINE_PREFIX_NAME)

# fxc full path
FXC := $(WINE_PREFIX_PATH)/drive_c/Program Files (x86)/Microsoft DirectX SDK (June 2010)/Utilities/bin/x86/fxc.exe
FXC_FLAGS := /nologo /WX /Ges /T fx_2_0
FXC_DEBUG_FLAGS := /Zi

# ensure wine prefix exists
ifeq ("$(wildcard $(WINE_PREFIX_PATH))", "")
	$(error Wine prefix wasn't found at: $(WINE_PREFIX_PATH))
endif

# ensure fxc is installed
ifeq ("$(shell which '$(FXC)')", "")
	$(error Effect-Compiler Tool wasn't found at expected path: $(FXC))
	# It can be installed on linux using:
	# $ winetricks prefix=x86 dxsdk_jun2010
endif

# start point, require all the needed recipes
build: $(OUTPUT_PATH)/ $(patsubst $(INPUT_PATH)/%.fx, $(OUTPUT_PATH)/%.fxc, $(wildcard $(INPUT_PATH)/*.fx))
.PHONY: build

# ensure $OUTPUT_PATH/ exists
$(OUTPUT_PATH)/:
> mkdir -p $(OUTPUT_PATH)

# compile every .fx file at $INPUT_PATH
# and outputs them at $OUTPUT_PATH as .fxc
$(OUTPUT_PATH)/%.fxc: $(INPUT_PATH)/%.fx
> @echo -e "\e[1;97m|\e[0m Compiling: \e[90m$<\e[0m"
> @echo -e "\e[1;97m|-\e[0m  Output: \e[90m$@\e[0m"
# go to input dir (fxc doesn't accept absolute paths)
> cd $(shell dirname $<)
# process input file (last arg) and output it using relative path to it's target location
> WINEPREFIX=$(WINE_PREFIX_PATH) wine "$(FXC)" $(FXC_FLAGS) /Fo "$(shell realpath --relative-to=$(shell dirname $<) $@)" "$(shell basename $<)"
> @echo -e "\e[1;97mDone!\e[0m"
