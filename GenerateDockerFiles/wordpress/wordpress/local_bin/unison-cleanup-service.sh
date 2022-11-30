#!/bin/bash
touch /home/dev/unisoncleanup.log
echo "starting cleanup.."  >> /home/dev/unisoncleanup.log
date >> /home/dev/unisoncleanup.log
echo date >> >> /home/dev/unisoncleanup.log
find $WORDPRESS_HOME/ -name "*.unison.tmp" -mtime +1 -exec rm -rf {} 2> /dev/null \;
find $HOME_SITE_LOCAL_STG/ -name "*.unison.tmp" -mtime +1 -exec rm -rf {} 2> /dev/null \;
echo "completed cleanup.."  >> /home/dev/unisoncleanup.log
date >> /home/dev/unisoncleanup.log