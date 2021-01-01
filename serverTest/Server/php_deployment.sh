#!/bin/bash

dir_path='Server'

#/var/lib/jenkins/workspace/UnityServer/serverTest/Server
cd /var/www/html;
if [ -d $dir_path ]; then
    sudo rm -rv Server;
    #sudo cp -r ~/Server ./;
    echo "Removed dir";
else
    echo "File Maked";
fi

#sudo cp -r ~/Server ./;
#sudo cp -r /var/lib/jenkins/workspace/UnityServer/serverTest/Server ./;
sudo cp -r ~/workspace/UnityServer/serverTest/Server ./;
echo "Server Dir Copy and past";
exit 0