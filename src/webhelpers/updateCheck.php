<?php
$productName = 'Screen Recorder';
$action_download = 'DownloadRun';
$action_download = 'DownloadCopy';
$action_noaction = 'NoAction';
$action_visit = 'Visit';
$msg_visit = "New version of $productName is available! Would you like to visit $productName home page and download new version?";

$lastReleaseDate = '2015/02/21';
$lastUrl = 'http://chehraz.ir/projects/screenrecorder/';
$lastVersion = '1.2.5530.21464';
$action = $action_visit;
$msg = $msg_visit;

if (isset($_POST["version"]))  {
   $os = $_POST["os"];
   $version = $_POST["version"];
   $versionArray = explode('.', $version, 0);
   $lastVersionArray = explode('.', $lastVersion, 0);
   $count1 = count($versionArray);
   $count2 = count($lastVersionArray);
   $minCount = min($count1, $count2);
   $update = false;
   for ($i = 0; $i < $minCount; $i++) {
      if ($lastVersionArray[$i] > $versionArray[$i]) {
         $update = true;
         break;
      }
   }
   if ($update) {
        echo "UPDATECHECK:action=$action|version=$lastVersion|date=$lastReleaseDate|msg=$msg|url=" . urlencode($lastUrl);
   }
   else {
        echo "UPDATECHECK:action=$action_noaction";
   }
}
?>