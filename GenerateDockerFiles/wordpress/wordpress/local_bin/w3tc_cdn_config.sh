cdn_type="$1"

afd_update_site_url() {
        AFD_URL="\$http_protocol . \$_SERVER['HTTP_HOST']"
        AFD_DOMAIN=$WEBSITE_HOSTNAME

        if [[ $AFD_CUSTOM_DOMAIN ]]; then
            AFD_DOMAIN=$AFD_CUSTOM_DOMAIN
            AFD_URL="\$http_protocol . '$AFD_CUSTOM_DOMAIN'"
        elif [[ $AFD_ENDPOINT ]]; then
            AFD_DOMAIN=$AFD_ENDPOINT
            AFD_URL="\$http_protocol . '$AFD_ENDPOINT'"
        fi

        #Bug in wp-cli - cannot update 'siteurl' in db if its already configured in wp-config file
        wp config set WP_HOME "\$http_protocol . \$_SERVER['HTTP_HOST']" --raw --path=$WORDPRESS_HOME --allow-root
        wp config set WP_SITEURL "\$http_protocol . \$_SERVER['HTTP_HOST']" --raw --path=$WORDPRESS_HOME --allow-root

        wp config set WP_HOME "$AFD_URL" --raw --path=$WORDPRESS_HOME --allow-root
        wp config set WP_SITEURL "$AFD_URL" --raw --path=$WORDPRESS_HOME --allow-root
        wp option update siteurl "https://$AFD_DOMAIN" --path=$WORDPRESS_HOME --allow-root
        wp option update home "https://$AFD_DOMAIN" --path=$WORDPRESS_HOME --allow-root

        # There is an issue with AFD where $_SERVER['HTTP_HOST'] header is still pointing to <sitename>.azurewebsites.net instead of AFD endpoint.
        # This is causing database connection issue with multi-site WordPress because the main site domain (AFD endpoint) doesn't match the one in HTTP_HOST header.
        if [ -e "$WORDPRESS_HOME/wp-config.php" ]; then
            XFORWARD_HEADER_DETECTED=$(grep "^\s*\$_SERVER\['HTTP_HOST'\]\s*=\s*\$_SERVER\['HTTP_X_FORWARDED_HOST'\];" $WORDPRESS_HOME/wp-config.php)
            if [ ! $XFORWARD_HEADER_DETECTED ];then
                sed -i "/Using environment variables for memory limits/e cat $WORDPRESS_SOURCE/afd-header-settings.txt" $WORDPRESS_HOME/wp-config.php
            fi
        fi

        echo "${cdn_type}_CONFIGURATION_COMPLETE" >> $WORDPRESS_LOCK_FILE
}

#Configure CDN settings 
if [[ "$cdn_type" == "BLOB_CDN" ]] && [[ $CDN_ENDPOINT ]] && [ ! $(grep "BLOB_CDN_CONFIGURATION_COMPLETE" $WORDPRESS_LOCK_FILE) ] \
&& [[ $(curl --write-out '%{http_code}' --silent --output /dev/null {https://$CDN_ENDPOINT}) == "200" ]] \
&& wp plugin activate w3-total-cache --path=$WORDPRESS_HOME --allow-root \
&& wp w3-total-cache option set cdn.azure.cname $CDN_ENDPOINT --type=array --path=$WORDPRESS_HOME --allow-root \
&& wp w3-total-cache option set cdn.includes.enable true --type=boolean --path=$WORDPRESS_HOME --allow-root \
&& wp w3-total-cache option set cdn.theme.enable true --type=boolean --path=$WORDPRESS_HOME --allow-root \
&& wp w3-total-cache option set cdn.custom.enable true --type=boolean --path=$WORDPRESS_HOME --allow-root; then
    echo "BLOB_CDN_CONFIGURATION_COMPLETE" >> $WORDPRESS_LOCK_FILE
    service atd stop
    redis-cli flushall
elif [[ "$cdn_type" == "CDN" ]] && [[ $CDN_ENDPOINT ]] && [ ! $(grep "CDN_CONFIGURATION_COMPLETE" $WORDPRESS_LOCK_FILE) ] \
&& [[ $(curl --write-out '%{http_code}' --silent --output /dev/null {https://$CDN_ENDPOINT}) == "200" ]] \
&& wp plugin activate w3-total-cache --path=$WORDPRESS_HOME --allow-root \
&& wp w3-total-cache option set cdn.enabled true --type=boolean --path=$WORDPRESS_HOME --allow-root \
&& wp w3-total-cache option set cdn.engine "mirror" --path=$WORDPRESS_HOME --allow-root \
&& wp w3-total-cache option set cdn.mirror.domain $CDN_ENDPOINT --type=array --path=$WORDPRESS_HOME --allow-root; then
    echo "CDN_CONFIGURATION_COMPLETE" >> $WORDPRESS_LOCK_FILE
    service atd stop
    redis-cli flushall
elif [[ "$cdn_type" == "BLOB_AFD" ]] && [[ $AFD_ENDPOINT ]] && [ ! $(grep "BLOB_AFD_CONFIGURATION_COMPLETE" $WORDPRESS_LOCK_FILE) ] \
&& [[ $(curl --write-out '%{http_code}' --silent --output /dev/null {https://$AFD_ENDPOINT}) == "200" ]] \
&& wp plugin activate w3-total-cache --path=$WORDPRESS_HOME --allow-root \
&& wp w3-total-cache option set cdn.azure.cname $AFD_ENDPOINT --type=array --path=$WORDPRESS_HOME --allow-root; then
    afd_update_site_url
    service atd stop
    redis-cli flushall
elif [[ "$cdn_type" == "AFD" ]] && [[ $AFD_ENDPOINT ]] && [ ! $(grep "AFD_CONFIGURATION_COMPLETE" $WORDPRESS_LOCK_FILE) ] \
&& [[ $(curl --write-out '%{http_code}' --silent --output /dev/null {https://$AFD_ENDPOINT}) == "200" ]]; then
    afd_update_site_url
    service atd stop
    redis-cli flushall
else
    service atd start
    echo "bash /usr/local/bin/w3tc_cdn_config.sh $cdn_type" | at now +5 minutes
fi
