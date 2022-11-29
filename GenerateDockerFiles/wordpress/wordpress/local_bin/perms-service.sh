#!/bin/bash
#chmod -R 777 /var/www/wordpress
#chown -R nginx:nginx /var/www/wordpress
find /var/www/wordpress/* \( \! -user nginx -o \! -group nginx \) -a -exec chown nginx:nginx {} \;
find /var/www/wordpress/* \! -perm 777 -exec chmod 777 {} \;