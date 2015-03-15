#summary Features of this Plugin.
#labels Featured



# Description of the plugin #
The plugin is made for accessing pictures and videos that have been taken during an activity in an easy way. The plugin contains the following capabilities:
  * Automatic import of pictures into an activity.
  * Automatic Geotagging based on the GPS track of an activity.
  * Three different views are implemented (Album View, List View, and Import View).
  * Showing thumbnails on the SportTracks maps.
![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/AlbumMaps.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/AlbumMaps.jpg)

# Album View #

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album.jpg)

In the Album View, only the images and videos are displayed.
It has the following functions:
  * When double-clicking on an image, it will be opened using your default application for the medium
  * When clicking on a video, it will be displayed. Navigation within the video is possible using the _Video_ toolstrip.
  * When hovering over an image, a tooltip will show the date and time, as well as the GPS location and the picture title.
  * The size of the images can be changed using the Photo Size slider bar.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-FitContext.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-FitContext.jpg)

The Picture toolstrip allows you to scale the size of the images displayed for the activity. You will be prompted to Save upon closing SportTracks.

Right-clicking on the slider control will bring up a context menu allowing you to choose whether the maximum size of the images should be locked to the size of the view.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-Groups.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-Groups.jpg)

The _Video_ toolstrip allows for video playback, pausing, snapshot taking, panning, and volume controlling.

  * Perform basic video playback functionality. The volume setting is saved upon closing SportTracks.
Note on video playback: An appropriate codec must be installed on your system to view videos. If an appropriate codec could not be found, an attempt is made to open the video in your default application for the media type.  If you are unsure which codecs are required, try downloading a codec pack, ie. K-Lite Codec Pack.

Snapshot:

The snapshot button will attempt to take an image capture of the current frame and use this image as the thumbnail. _Video_ is stamped in the upper left corner of the thumbnail to distinguish it from the other images. The snapshot functionality is still somewhat experimental.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-VideoContext.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-VideoContext.jpg)

Right-clicking on a video will bring up a context menu allowing you to:
  * Open Containing Folder - Opens the folder where video is located.
  * Reset Snapshot - Resets the snapshot to the default _Video_ image.
  * Remove - Deletes the clip from the activity. You will be prompted to Save upon closing SportTracks.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-PhotoContext.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-PhotoContext.jpg)

Right-clicking on a picture will bring up a contet menu allowing you to:
  * Open Containing Folder - Opens the folder where the picture is located.
  * Remove - Deletes the picture from the activity. You will be prompted to Save upon closing SportTracks.

# List View #

In the List View, thumbnails &amp; metadata of the image are displayed. It has the following features:

  * Display of Exif data: Title, Date/Tme, Camera Model, GPS Location, Altitude, ...
  * Some of the data is editable and will be saved with the image file: Title, Comment.
  * When double-clicking on a thumbnail, it will be opened using the default application for the media.
  * There are three buttons in the toolstrip: Geotag, Time offset, and Export to Google Earth.
  * If the original location for a picture/video no longer exist, the photo source cell is set to red.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/List-Toolstrip.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/List-Toolstrip.jpg)

The List View toolstrip menu has the following options:
  * Geotag - Will geotag all selected images and videos based on the Timestamp and the track of the SportTracks activity.
  * Time offset - Offsets the Date/Time of the selected images and videos by the specified amount. This is useful if the dates and times of several images are off by the same amount.
  * Export to Google Earth - There are two options for the export.
    * Export as kmz: Will include all selected images in a compressed file which can be distributed and opened on any computer.
    * Export as kml: Will include only links to the local images, therefore, file size is much smaller. The _kml_ file is only useful for the local PC.

View kmz/kml:
Clicking the little globe icon brings up a context menu showing a list of kmz/kml files that were created for the activity.

Note: This option only exists if the, _Store kmz/kml file locations_ option found in the Settings Page is checked.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/List-GoogleContext.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/List-GoogleContext.jpg)

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/List-Context.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/List-Context.jpg)

Right-clicking the grid brings up a context menu with the following options:
  * Copy: Copies the details of the selected rows to the clipboard. Only details from the checked columns are copied to the clipboard.
  * Hide All Columns: Hides all columns except, _Thumbnail_.
  * &lt;Column&gt;: A list of columns available to be displayed. Only checked columns are shown.

Editing the Title and Comment fields is possible by either double-slow-clicking on a particular cell, or by pressing F2.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/SetTimeStamp.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/SetTimeStamp.jpg)

Double-clicking the date cell for an image brings up the Set Time Stamp dialog.
This dialog allows you to set the date and time for the image or video.

# Import View #

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import.jpg)

  * The upper left corner contains a treeview which shows the drives and olders found on the system.
  * The lower left corner contains a listview which shows the pictures and videos found in the selected folder.
  * The upper right corner contains a treeview showing the currently selected activities, grouped by year and month.
  * The lower right corner contains a listview showing the pictures and videos currently assigned to the selected activity.

The import view has two ways of adding images and videos to your activity.

## Auto Import ##
![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import-AutoImport.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import-AutoImport.jpg)

Clicking the _Auto Import_ button will automatically scan the checked folders for images and videos with time taken during the selected activities.

If you have selected several activities with ctrl-click or a week or month (in Daily Activity) or in Reports View, you can import to several activities simultaneusly. Click year or month to import to.

Note: Checking or unchecking a folder will automatically check or uncheck all child folders.

## Manual Import ##
![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import-ListDriveContext.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import-ListDriveContext.jpg)

The second method for adding images to an activity is by manually selecting images to a single activity.

To manually import images:
  * Select an activity from the upper right pane.
  * Select the folder containing the images and videos you wish to import.
  * Select the images and videos in the lower left pane.
  * Right click on the files you wish to add, and click _Add_. You can also drag and drop from the lower left pane to the lower right pane.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import-ListActivitiesContext.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import-ListActivitiesContext.jpg)

Removing images from an activity can be done by selecting the images in the lower right pane either by clicking the delete key, or by right-clicking and selecting _Remove_ from the context menu.

Right clicking on an item in the lower right view brings up a context menu with the following options:
  * Open - Opens the original file in the default application for the media.
  * Open Containing Folder - Opens the folder where the image or video is located.
  * Open Thumbnail - Opens the thumbnail image of the image or video.
  * Remove - Removes the image from the activity.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import-TreeDriveContext.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Import-TreeDriveContext.jpg)

Right clicking on a folder or a drive in the folder view will bring up a context menu allowing you to:
  * Refresh - Refreshes the list of child folders.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-ActivityTreeContext.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Album-ActivityTreeContext.jpg)

Right clicking on a node in the activity tree will bring up a context menu allowing you to:
  * Copy Copies details of the images and videos assigned to the activity to the clipboard. The following information is copied: Name, Photo Source, Date, GPS location, Title, Comment, ReferenceID, File Path, and Thumbnail.
  * Migrate Paths... Allows you to update the location of the source images in case their location has changed since they were added to SportTracks.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/MigrateSourcePath.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/MigrateSourcePath.jpg)

To change the location the Activity Picture Plugin searches for the original images, set _Source_ to the original location, _Destination_ to the current location, and click OK.

The import control supports five listview views. The views can be changed by clicking the, _Change View_ buttons. The views supported are, Detail, Large Icon, Small Icon, Tile, and List.

# Settings Page #

The settings page has much of the same functionality as found in the Activity Detail Page with a few differences.

![http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Settings.jpg](http://sporttracks-activitypicture.googlecode.com/svn/wiki/Images/Settings.jpg)

## Import ##
Differences from Activity Detail Page:
The upper right pane now contains a tree of all the activities found in the logbook, both in My Activities and My Friends Activities. This is similar to selecting the summary line in Reports View. Nodes highlighted in blue indicate that a child activity has a picture or movie assigned.

Notes: Please see [Import View](Features#Import_View.md) for more details on how to import images and videos into activities.

## Additional Settings ##
The top of the Settings Page also has a number of additional settings:
  * Google Earth Size: Slider allows for the changing of the image sizes exported to Google Earth through the &quot;Export to Google Earth&quot; button found in the List view.
  * Google Earth Quality: Slider allows for the changing of the quality of the images exported to Google Earth through the &quot;Export to Google Earth&quot; button found in the List view.
  * Open in Google Earth when Created: Automatically launches Google Earth after creating the kmz/kml files.
  * Store kmz/kml file locations: Stores the location of the created kmz/kml folder and assigns them to the activity.

Notes: Please see [List View](Features#List_View.md) for more details on creating kmz/kml files.

# Other #
## Time from images ##
The plugin selects the first of the following time that is in the activity:
  * Exif time taken (if available)
  * Image creation time
  * Image modification

It is therefore possible that an image matches more than one activity when importing.

## GPS ##
If the image has GPS in Exif, that is used. Otherwise the GPS position is estimated from the time an GPS track. The latitude/longitude is then in italics.

## Localization ##
Currently the languages English, German, French, Spanish, Swedish, and Dutch are supported, but all are not completete. Further languages will be added on request. See link in the Developers wiki page for the procedure to update the online spreadsheet.

## Data storage ##

When a picture is added, a unique referenceID will be created and an thumbnail image of reduced size (&lt;150kb) will be saved.

## Possible enhancements ##
  * All translations are not complete
  * It could be possible to import to several activities if more than one selected in Activity Detail/Reports.
  * It could be possible to import pictures automatically after importing activities.
