#!/bin/sh
set -e

# a tiny script to manage FNA native libs
# it automatically downloads from 'FNA-XNA/fnalibs-dailies' github artifacts
# and keeps at obj/ to be included with the build
# if local FNA version changes, it'll update the libs accordingly

LIBS_PATH=$1
FNA_LIB_PATH=$1/fna
PLATFORM="lib64"

# get fna version
FNA_VERSION=$(cd ../FNA; git describe --tags --abbrev=0)
echo "FNA version: $FNA_VERSION"

# verify if need to update native libs
echo "Verifying FNA native libs..."

VERSION=$(
    cd "$FNA_LIB_PATH"

    if [ -f version ]
    then
        echo $(<version)
    else
        echo "undefined"
    fi
)

if [ "$VERSION" != "$FNA_VERSION" ]
then
    echo "Libs are outdated (current version: $VERSION)"
    echo "Updating FNA native libs..."

    # ensure git authentication is working
    echo "Checking git authentication"
    gh auth status -a
    if [ "$?" != "0" ]
    then
        echo "Error: There is a problem with github authentication."
        exit 1
    fi

    (
        # go to lib dir and download artifacts from github repo
        echo "Dowloading build from 'FNA-XNA/fnalibs-dailies'..."

        # recreate lib path
        rm -rd "$FNA_LIB_PATH"
        mkdir -p "$FNA_LIB_PATH"

        # download artifacts
        cd "$FNA_LIB_PATH"
        gh run download -n fnalibs --repo FNA-XNA/fnalibs-dailies

        # update version file
        echo "Updating version file"
        if [ -f version ]
        then
            rm version
        fi

        echo "$FNA_VERSION" >> version
    )

    echo "Done!"
else
    echo "Libs are updated! (version: $VERSION)"
fi

echo "FNA native libs successfully prepared!"
exit 0
