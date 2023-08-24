<?php

/**
 * The admin-specific functionality of the plugin.
 *
 * @link       https://github.com/Azure/Wordpress-on-Linux-App-Service-plugins/tree/main/app_service_email/
 * @since      1.0.0
 *
 * @package    App_service_email
 * @subpackage App_service_email/admin
 */

/**
 * The admin-specific functionality of the plugin.
 *
 * Defines the plugin name, version, and two examples hooks for how to
 * enqueue the admin-specific stylesheet and JavaScript.
 *
 * @package    Azure_app_service_migration
 * @subpackage Azure_app_service_migration/admin
 * @author     Microsoft <wordpressdev@microsoft.com>
 */
class Azure_app_service_email_logger
{

    /**
     * The ID of this plugin.
     *
     * @since    1.0.0
     * @access   private
     * @var      string    $plugin_name    The ID of this plugin.
     */
    private $plugin_name;

    /**
     * The version of this plugin.
     *
     * @since    1.0.0
     * @access   private
     * @var      string    $version    The current version of this plugin.
     */
    private $version;

    public function email_logger_capture_emails($to, $subject, $message, $status, $error_message)
    {
        // Log the email in a custom database table
        global $wpdb;
        $table_name = $wpdb->prefix . 'email_logs';
        $wpdb->insert(
            $table_name,
            array(
                'to_email' => $to,
                'subject' => $subject,
                'message' => $message,
                'sent_date' => current_time('mysql'),
                'status' => $status,
                'error_message' => $error_message,

            ),
            array('%s', '%s', '%s', '%s', '%s', '%s')
        );
    }

    public function email_logger_create_table()
    {
        global $wpdb;
        $table_name = $wpdb->prefix . 'email_logs';
        $charset_collate = $wpdb->get_charset_collate();
        $sql = "CREATE TABLE $table_name (
        id int(11) NOT NULL AUTO_INCREMENT,
        to_email varchar(255) NOT NULL,
        subject text NOT NULL,
        message longtext NOT NULL,
        sent_date datetime NOT NULL,
		status varchar(20) NOT NULL,
        error_message text,
        PRIMARY KEY (id)
    ) $charset_collate;";

        require_once(ABSPATH . 'wp-admin/includes/upgrade.php');
        dbDelta($sql);
    }
}
