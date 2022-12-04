#!/bin/bash
date >> /home/dev/unisoncleanup.txt
find $WORDPRESS_HOME/ -name "*.unison.tmp" -mtime +1 -exec rm -rf {} 2> /dev/null \;
find $HOME_SITE_LOCAL_STG/ -name "*.unison.tmp" -mtime +1 -exec rm -rf {} 2> /dev/null \;