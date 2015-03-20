<?php
require_once('../../PHPMailer-5.2.8/class.phpmailer.php');
$max_email_length = 64;
$max_message_length = 2048;
$max_name_length = 64;
$max_report_length = 20 * 1024 * 1024; // 20 MB
$max_os_length = 64;
$max_subject_length = 128;
$max_version_length = 64;
$error = '';
$showThanks = false;

if (isset($_POST["submit"]))  {
	$name = htmlspecialchars($_POST["name"]);
	$email = htmlspecialchars( $_POST["email"]);
	$subject = htmlspecialchars($_POST["subject"]);
	$message = htmlspecialchars($_POST["message"]);
              $os = htmlspecialchars($_POST["os"]);
              $report = htmlspecialchars($_POST["report"]);
              $version = htmlspecialchars($_POST["version"]);                     
	if (empty($subject)) {
                  $error = "You need to enter a subject.";
               }
               else if (empty($message)) {
                  $error = "You need to provide a message.";
               } 
               else if (strlen($email) > $max_email_length) {
                  $error = "E-Mail is too long.";
               }
               else if (strlen($message) > $max_message_length) {
                  $error = "Message is too long.";
               }
	 else if (strlen($name) > $max_name_length) {
                  $error = "Name is too long.";
               }
	 else if (strlen($subject) > $max_subject_length) {
                  $error = "Subject is too long.";
               }
	 else if (strlen($os) > $max_os_length) {
                  $error = "OS is too long.";
               }
	 else if (strlen($version) > $max_version_length) {
                  $error = "Version is too long.";
               }
               else  {
	     if (!empty($report) && strlen($report) < $max_report_length) {
	         $report = $decodedReport = gzinflate(base64_decode($report));
	     } 
                   else {
                        unset($report);
                   }
	     $to = "mehrzady@gmail.com";
                   $message = "<body style=\"font-family:verdana, Sans serif;\">" .
                                      "<h3>Info</h3>" .
                                           "<table style=\"font-family:verdana, Sans serif;\">" . 
			  "<tr><td>Name:</td><td>$name</td></tr>" . 
                                            "<tr><td>From:</td><td>$email</td></tr>" . 
                                            "<tr><td>Version:</td><td>$version</td></tr></table>" .
                                            "<tr><td>OS:</td><td>$os</td></tr></table>" .
                                      "<h3>Message</h3>" .
                                      "<div>" . str_replace('\r\n', '<br/>', $message) . "</div>" . 
                                      "</body>";
                   $mail=new PHPMailer();
	     $mail->AddAddress($to);
	     $mail->CharSet = 'UTF-8';
                   $mail->SetFrom($email);
                   $mail->Subject = "Screen Recorder (Feedback): $subject";
                   $mail->MsgHTML($message);
	     $mail->IsSMTP(); // telling the class to use SMTP
	     $mail->SMTPDebug  = 0; 
	     $mail->SMTPAuth   = true;
	     $mail->SMTPSecure = "tls";
	     $mail->Host       = "smtp.gmail.com";
	     $mail->Port       = 25;
	     $mail->Username   = "mchehraz";
	     $mail->Password   = "1asdfghj";
                   if (!empty($report)) {
                      $mail->AddStringAttachment($report, 'report.txt', 'base64', 'text/plain');
                   }
                   $succeed = $mail->Send();
                   if ($succeed) {
                        echo "Succeed";                 
                        exit();
                   }
                   else {
                        $error = "Failed to submit feedback. An error is occured: " . $mail->ErrorInfo;
                   }
                 
              }
              echo $error;
              
}
?>