

This page describes the changes between plugins. The SVN change log contains further details.

### Changes ###

2.0.181 2013-09-22
  * Updated French Translation
  * Use previously selected directory as defualt directory when importing pictures and exporting to Google Earth.
  * Mark if GPS is derived from GPS track by displaying latitude/longitude in italics.
  * Set a red background if the original source for a picture does no longer exist.

2.0.168 2013-08-25

Major update.
This release reimplements a lot of the functionality, there are many changes.

See the updated plugin documentation for further description.

Album View:
  * Reduced image flickering while resizing
  * Reduced tooltip flickering
  * Replaced trackbar sliders with custom sliders to reduce flickering
  * Improved trackbar image size vs panel size behaviour
  * Improved video playback/stopping.
  * Saves Image size slider to settings
  * Improved behaviour of volume slider
  * Saves volume to settings.
  * Corrected scrolling behaviour when losing/regaining focus
  * Doubleclicked images now open in default windows image application. (ie. Picasa, Photoshop, etc)
  * Video thumbnail frame capture for avi files
  * Delayed tooltip so it isn't 'in your face'.
  * Keyboard navigation

ListView:
  * Doubleclicked images and videos open in windows default applications.
  * Resized button controls (size calculated @ runtime depending on culture)
  * Added sort glyph to column headers
  * Corrected date column sorting
  * Configurable columns via context menu. (Settings saved)
  * Copy data to clipboard
  * Added .tif, tiff and .mov to supported media types and corrected .mpg.

Import:
  * No longer shows hidden or system folders
  * Fixed bug in Folders View while drilling to an inaccessible folder.
  * Corrected sizing issues
  * Listview modes saved to settings
  * Improved Theme usage
  * Changed behaviour of progressbar.
  * Sped up file sorting during auto-import. Removed 400 file limit.
  * Removes thumbail files that were added but not saved to logbook.
  * Added ability to Refresh folder nodes.
  * Possible to migrate paths if i.e. the original source file have been moved.

Map:
  * Click on an image to send it to the back, i.e. view other images it covers.
  * Double click opens a picture/video (similar to doulble click in the list)

1.2.41 2011-02-27
  * Some minor translation issues fixed
  * Italian translation update

1.2.39 2011-02-20
  * Control size of image on map with "album scaler"
  * Picuture ration on map corrected
  * Use thumbnails instead of real source on the map to avoid out-of-memory exceptions
  * Pictures were not always listed when refreshing while opening in n image viewer. This can still occur, the risk is lower

1.2.36 2011-02-15
  * Tags were written/read incorrectly for pictures on west and south hemispere
  * Swedish translation
  * Display pictures of maps also in reports view
  * Mono adaptions (avoiding exceptions)

1.2.26 2011-02-15
  * First official ST3 version
  * Last planned ST2 version
  * Show map miniatures on the map
  * "Nervousnes" in album view minimized
  * Many GUI touchups to follow ST more closely
  * Many internal changes and adaptions to common translations

1.1.5 2008-04-11
  * doml's last private version

Old version history from http://www.zonefivesoftware.com/sporttracks/forums/viewtopic.php?f=35&t=3079

Version 1.1.3 (March 19, 2008)
Change log:
  * New feature: French language support (Thanks to Meven)
  * New feature:Import method is chosen automatically, depending on number of images
  * New feature: Export images to Google Earth now also accessible from Export menu => Images of all selected activities will be exported
  * See screenshots : Album view / List view / Import view / Export to Google Earth

Version 1.1.2 (March 7, 2008)
Change log:
  * New feature: Export selected activities to Google Earth (see images on the map and there them with friends!)
  * New feature: Modify time stamp of selected images
  * New feature: Ability to delete images within the import view
  * New feature: Completely new import control, which should be a lot more user-friendly now
  * Speed improvement for automatic import by applying intelligent routines
  * Major GUI changes

Version 1.0.7 (January 22, 2008)
Change log:
  * Bug fixes: Fixed error for pictures that have been imported before Version 1.0.6
  * Bug fix: GUI bugs with misplaced buttons, wrong theme backgrounds
  * Bug fix: GPS data was corrupted when Geotagging was made with different program. Should now work for all Tools (e.g.: Robogeo, Picopolo, Panorado).
  * Added German language support for the new features
  * When importing, Geotagging is not done automatically anymore. An extra button has been added in the Image Option group for this purpose. With this change it is possible to keep GeoTags that have already been created by other tools.

Version 1.0.6 (January 15, 2008)
Change log:
  * Implementation of Import functionality (see description)
  * Special characters ("������...") are supported now editing Title and Comment of a picture
  * The GPS format of the ST setting will be used for the GPS data property in the List view
  * The elevation unit of ST settings will be used for the altitude property in the List view
  * The dateTimeFormat of the CultureInfo will be used for the dateTime property in the List view

Version 1.0.5 (October 30, 2007)
Change log:
  * Fixed bug with EXIF GPS data for negative Longitutes (Thanks to Racefern!)
  * Enhanced video options: Added a slider for current position + volume control
  * Enhanced GUI functions: Click on video in Album view will play the video, DoubleClick on image will open the image with Windows standard viewer

Version 1.0.4 (October 25, 2007)
Change log:
  * Fixed bug when editing Titles and Comments for Video files
  * Added Column "Type" in the List view

Version 1.0.3 (October 24, 2007)
Change log:
  * Cosmetic GUI changes
  * Fixed problem is image directory does not exist
  * Implemented display of videos (avi and wmv)
Still in a very early stage, therefore kind of hidden, for testing it: Import selected -> choose **.avi or**.wmv files
In the album view, click on the black image with the string �video� inside to display it. Then, the video options should pop up, where it is possible to choose play, pause and stop. These options will be extended if there is a positive feedback. Please report errors.
One open point is a way to grab the first frame of the video as a .jpg image. I haven�t found a working solution so far. I would be glad if anyone could help!

Version 1.0.2 (October 18, 2007)
Change log:
  * Minor GUI changes
  * Selection of multiple files
  * Implemented Altidue

Version 1.0.1 (October 18, 2007)
Change log:
  * Fixed bug on opening picture page

Version 1.0 (October 17, 2007)

### Future plans ###
Some possible additions:
  * Remove last three list columns, use tooltip instead
  * Copy list text, including ref path?
  * Migration of paths for thumbnails from ST2 to ST3
  * Click on pictures in Route: open imageviewer?

### Feedback ###
Patches accepted.
For patches, bugreports or feature suggestions, use the Google Code issue list.
For other feedback please use the SportTracks forum or this wiki.