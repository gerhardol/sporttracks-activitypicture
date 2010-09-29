/*
Copyright (C) 2008 Dominik Laufer

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 3 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library. If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using ActivityPicturePlugin.Helper;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Visuals;
using com.drew.metadata.exif;

namespace ActivityPicturePlugin.UI
{
    public partial class ImportControl : UserControl
    {
        public ImportControl()
        {
            InitializeComponent();
            //InitializeListViewDrive();
            onImagesComplete = new EventHandler(OnImagesComplete);
            treeViewActivities.TreeViewNodeSorter = new NodeSorter();
            this.SelectedNodes = new List<TreeNode>();
            this.ActivityNodes = new List<TreeNode>();
        }
        public void UpdateUICulture(System.Globalization.CultureInfo culture)
        {
            m_culture = culture;
            this.btnScan.Text = CommonResources.Text.ActionImport;
            this.btnExpandAll.Text = Resources.Resources.btnExpandAll_Text;
            this.btnCollapseAll.Text = Resources.Resources.btnCollapseAll_Text;
            this.btnViewAct.Text = Resources.Resources.ImportControl_changeView;
            this.btnViewFolder.Text = Resources.Resources.ImportControl_changeView;
            this.colImage.Text = Resources.Resources.thumbnailDataGridViewImageColumn_HeaderText;
            this.colDateTime.Text = CommonResources.Text.LabelDate;
            this.colGPS.Text = CommonResources.Text.LabelGPSLocation;
            this.colTitle.Text = Resources.Resources.titleDataGridViewTextBoxColumn_HeaderText;
            this.colDescription.Text = Resources.Resources.commentDataGridViewTextBoxColumn_HeaderText;

            this.colDImage.Text = Resources.Resources.thumbnailDataGridViewImageColumn_HeaderText;
            this.colDDateTime.Text = CommonResources.Text.LabelDate;
            this.colDGPS.Text = CommonResources.Text.LabelGPSLocation;
            this.colDTitle.Text = Resources.Resources.titleDataGridViewTextBoxColumn_HeaderText;
            this.colDDescription.Text = Resources.Resources.commentDataGridViewTextBoxColumn_HeaderText;
        }
        public void ThemeChanged(ZoneFiveSoftware.Common.Visuals.ITheme visualTheme)
        {
            this.BackColor = visualTheme.Control;
            this.ForeColor = visualTheme.ControlText;
        }
        public void LoadNodes()
        {
            numFilesImported = 0;
            files.Clear();
            lblProgress.Text = "";
            progressBar2.Visible = false;
            this.splitContainer1.Height = this.Height;

            UpdateUICulture(m_culture);

            if (!standardpathalreadyshown)
            {
                FillTreeViewImages();
            }
            if (!standardpathalreadyshown | this.showallactivities == false)
            {
                FillTreeViewActivities();
                FindImagesInActivities();
                standardpathalreadyshown = true;
            }
        }
        //private void InitializeListViewDrive()
        //    //copy columns of ListViewAct
        //    {
        //    if (this.listViewDrive.Columns.Count == 0)
        //        {
        //        ColumnHeader[] cols = new ColumnHeader[this.listViewAct.Columns.Count];
        //        for (int i = 0; i < this.listViewAct.Columns.Count; i++)
        //            {
        //            cols[i] = (ColumnHeader)(this.listViewAct.Columns[i].Clone());
        //            }
        //        this.listViewDrive.Columns.AddRange(cols);
        //        }
        //    }

        #region private variables
        private List<FileInfo> files = new List<FileInfo>(); //List of all images files of selected folders (for automatic import)
        private List<TreeNode> ActivityNodes = new List<TreeNode>();//List of all activities (used for comparing and organizing)
        private List<TreeNode> SelectedNodes = new List<TreeNode>();//Nodes that are selected in the Drive TreeView
        private bool showallactivities;
        private bool notfireevent;
        private bool standardpathalreadyshown;
        private int viewActivity = 0; //View of ListviewAct
        private int viewFolder = 0; //View of ListviewDrive
        private int NumDayNodes = 0; //needed for progress bar
        private int numFilesImported = 0;
        private System.Globalization.CultureInfo m_culture = System.Globalization.CultureInfo.CurrentUICulture;
        #endregion

        #region Public properties
        public bool ShowAllActivities
        {
            get { return showallactivities; }
            set { showallactivities = value; }
        }
        #endregion

        #region TreeViewImages
        private void SetCheck(TreeNode node, bool check)
        {
            if (node.Checked)
            {
                if (!this.SelectedNodes.Contains(node)) this.SelectedNodes.Add(node);
            }
            else
            {
                if (this.SelectedNodes.Contains(node)) this.SelectedNodes.Remove(node);
            }
            foreach (TreeNode n in node.Nodes)
            {
                notfireevent = true;
                n.Checked = check;
                notfireevent = false;
                if ((n.Tag is FileInfo) | (n.Tag is DirectoryInfo))
                {
                    if (n.Checked)
                    {
                        if (!this.SelectedNodes.Contains(n)) this.SelectedNodes.Add(n);
                    }
                    else
                    {
                        if (this.SelectedNodes.Contains(n)) this.SelectedNodes.Remove(n);
                    }
                }
                if (n.Nodes.Count != 0) SetCheck(n, check);
            }
        }
        private void GetSubDirectoryNodes(TreeNode parentNode)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(parentNode.FullPath);
                DirectoryInfo[] dirSubs = dir.GetDirectories();
                foreach (DirectoryInfo dirsub in dirSubs)
                {
                    TreeNode subNode = new TreeNode(dirsub.Name);
                    if (dirsub.GetDirectories().Length != 0) subNode.Nodes.Add("Dummy");
                    subNode.Tag = dirsub;
                    parentNode.Nodes.Add(subNode);
                    subNode.Checked = parentNode.Checked;
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }
        private void ShowFolderPics() //Adds the images of a directory to the ListViewDrive
        {
            try
            {
                files.Clear();
                files.InsertRange(0, DriveDir.GetFiles());

                listViewDrive.Items.Clear();

                ImageList lvImgL = new ImageList();
                lvImgL.ImageSize = new Size(100, 100);
                lvImgL.ColorDepth = ColorDepth.Depth32Bit;
                listViewDrive.LargeImageList = lvImgL;

                ImageList lvImgS = new ImageList();
                lvImgS.ImageSize = new Size(50, 50);
                lvImgS.ColorDepth = ColorDepth.Depth32Bit;
                listViewDrive.SmallImageList = lvImgS;


                Image bmp = (Bitmap)(Resources.Resources.image).Clone();
                for (int j = 0; j < files.Count; j++) //add images plus exif data
                {
                    ImageData.DataTypes dt = Functions.GetMediaType(files[j].Extension);
                    if (dt == ImageData.DataTypes.Nothing)
                    {
                        files.Remove(files[j]);
                        j--;
                    }
                    else
                    {
                        lvImgL.Images.Add(Functions.getThumbnailWithBorder(lvImgL.ImageSize.Width, bmp));
                        lvImgS.Images.Add(Functions.getThumbnailWithBorder(lvImgS.ImageSize.Width, bmp));
                        AddFileToListView(files[j], dt);
                    }
                }
                bmp.Dispose();


                for (int j = 0; j < files.Count; j++) //read images and add thumbnails
                {
                    AddImageToListView(lvImgL, lvImgS, j);
                    listViewDrive.Invalidate();
                }
            }
            catch (Exception)
            {
                // throw;
            }
        }
        private void AddImageToListView(ImageList lvImgL, ImageList lvImgS, int j)
        {
            try
            {
                ImageData.DataTypes dt = Functions.GetMediaType(files[j].Extension);
                if (dt == ImageData.DataTypes.Image)
                {
                    //Test: try showing thumbnail without opening whole image
                    //byte[] imgbyte = com.SimpleRun.ShowOneFileThumbnailData(file.FullName);
                    //MemoryStream stream = new MemoryStream(imgbyte.Length);
                    //stream.Write(imgbyte, 0, imgbyte.Length);
                    //Image img = Image.FromStream(stream);
                    //if (img != null) lvImg.Images.Add(img);

                    Image img = Image.FromFile(files[j].FullName);
                    lvImgL.Images[j] = Functions.getThumbnailWithBorder(lvImgL.ImageSize.Width, img);
                    lvImgS.Images[j] = Functions.getThumbnailWithBorder(lvImgS.ImageSize.Width, img);
                    img.Dispose();
                }
                else if (dt == ImageData.DataTypes.Video)
                {
                    Image bmp = (Bitmap)(Resources.Resources.video).Clone();
                    lvImgL.Images[j] = Functions.getThumbnailWithBorder(lvImgL.ImageSize.Width, bmp);
                    lvImgS.Images[j] = Functions.getThumbnailWithBorder(lvImgS.ImageSize.Width, bmp);
                    bmp.Dispose();
                }
                Application.DoEvents();
            }
            catch (Exception)
            {
                //throw;
            }
        }
        private void AddFileToListView(FileInfo file, ImageData.DataTypes dt)
        {
            //   try
            //     {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = file.Name;
            lvi.ImageIndex = files.IndexOf(file);
            lvi.Tag = file.FullName;

            if (dt == ImageData.DataTypes.Image)
            {
                string dts = "";
                try
                {
                    ExifDirectory ed = SimpleRun.ShowOneFileExifDirectory(file.FullName);
                    GpsDirectory gps = SimpleRun.ShowOneFileGPSDirectory(file.FullName);

                    string s = ed.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
                    IFormatProvider culture = new System.Globalization.CultureInfo("de-DE", true);
                    dts = DateTime.ParseExact(s, "yyyy:MM:dd HH:mm:ss", culture).ToString();
                    lvi.SubItems.Add(dts);

                    string latref = gps.GetDescription(GpsDirectory.TAG_GPS_LATITUDE_REF);
                    string latitude = gps.GetDescription(GpsDirectory.TAG_GPS_LATITUDE);
                    string longitude = gps.GetDescription(GpsDirectory.TAG_GPS_LONGITUDE);
                    string longref = gps.GetDescription(GpsDirectory.TAG_GPS_LONGITUDE_REF);
                    string gpsstr = latitude + " " + latref + ", " + longitude + " " + longref;
                    if (latitude != null) lvi.SubItems.Add(gpsstr);
                    else lvi.SubItems.Add("");

                    lvi.SubItems.Add((string)(ed.GetDescription(ExifDirectory.TAG_XP_TITLE)));
                    lvi.SubItems.Add((string)(ed.GetDescription(ExifDirectory.TAG_XP_COMMENTS)));
                }
                catch (Exception)
                {
                }

            }

            this.listViewDrive.Items.Add(lvi);
        }


        private void FillTreeViewImages()
        {
            try
            {
                this.treeViewImages.Nodes.Clear();
                this.SelectedNodes.Clear();

                string[] drives = Environment.GetLogicalDrives();
                DirectoryInfo startdir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));

                foreach (string rootDir in drives)
                {
                    DirectoryInfo dir = new DirectoryInfo(rootDir);
                    TreeNode ndRoot = new TreeNode(rootDir);
                    ndRoot.Tag = dir;

                    //Fill standard path
                    this.treeViewImages.Nodes.Add(ndRoot);

                    if (dir.Root.Name == startdir.Root.Name)
                    {
                        AddStandardPath(this.treeViewImages, startdir, ndRoot);
                    }
                    else
                    {
                        ndRoot.Nodes.Add("Dummy");
                    }
                }
            }
            catch (Exception)
            {
                // throw;
            }

        }
        private void AddStandardPath(TreeView tvw, DirectoryInfo startdir, TreeNode ndRoot)
        {
            try
            {
                TreeNode parentNode = null;
                List<DirectoryInfo> dirs = new List<DirectoryInfo>();

                while (true)
                {
                    if (startdir.Parent != null)
                    {
                        dirs.Add(startdir);
                        startdir = startdir.Parent;
                    }
                    else break;
                }
                dirs.Reverse();
                parentNode = ndRoot;

                foreach (DirectoryInfo subdir in dirs)
                {
                    TreeNode subNodeP = new TreeNode(subdir.Name);
                    subNodeP.Tag = subdir;
                    parentNode.Nodes.Add(subNodeP);

                    DirectoryInfo dir = new DirectoryInfo(parentNode.FullPath);
                    DirectoryInfo[] dirSubs = dir.GetDirectories();
                    foreach (DirectoryInfo dirsub in dirSubs)
                    {
                        DirectoryInfo dirP = (DirectoryInfo)(subNodeP.Tag);
                        if (dirsub.Name != dirP.Name)
                        //otherwise the folders of the standard path will be created twice
                        {
                            TreeNode subNode = new TreeNode(dirsub.Name);
                            int i = 0;
                            try
                            {
                                i = dirsub.GetDirectories().Length;
                            }
                            catch (Exception)
                            {
                                i = 0;
                            }
                            if (i != 0) subNode.Nodes.Add("Dummy");
                            subNode.Tag = dirsub;
                            parentNode.Nodes.Add(subNode);
                        }
                    }
                    parentNode = subNodeP;
                }

                DriveDir = new DirectoryInfo(parentNode.FullPath);
                //Thread td1 = new Thread(new ThreadStart(ShowFolderPics));
                //td1.Start();
                //td1.Join();
                ShowFolderPics();
                if (DriveDir.GetDirectories().Length != 0) parentNode.Nodes.Add("Dummy");
                notfireevent = true;
                parentNode.EnsureVisible();
                parentNode.TreeView.SelectedNode = parentNode;
                notfireevent = false;
            }
            catch (Exception)
            {
                //throw;
            }
        }
        DirectoryInfo DriveDir;
        #endregion

        #region TreeViewActivities
        private void btnExpandAll_Click(object sender, EventArgs e)
        {
            this.treeViewActivities.ExpandAll();
        }
        private void btnCollapsAll_Click(object sender, EventArgs e)
        {
            this.treeViewActivities.CollapseAll();
        }
        private void AddImagesToListViewAct(TreeNode tn, bool AddThumbs)
        {
            try
            {
                this.listViewAct.Items.Clear();
                IActivity act = (IActivity)(tn.Tag);
                ActivityPicturePlugin.UI.Activities.PluginData data = Helper.Functions.ReadExtensionData(act);

                if (data.Images.Count > 0)
                {
                    //change color
                    if (tn.BackColor != Color.Yellow)
                    {
                        tn.BackColor = Color.LightBlue;
                        if (tn.Parent != null)
                        {
                            if (tn.Parent.BackColor != Color.Yellow) tn.Parent.BackColor = Color.LightBlue;
                            if (tn.Parent.Parent != null & tn.Parent.Parent.BackColor != Color.Yellow) tn.Parent.Parent.BackColor = Color.LightBlue;
                        }
                    }

                    //Load imagedata
                    if (AddThumbs)
                    {
                        List<ImageData> il = data.LoadImageData(data.Images);
                        ImageList lil = new ImageList();
                        ImageList lis = new ImageList();
                        lil.ImageSize = new Size(100, 100);
                        lis.ImageSize = new Size(50, 50);
                        lil.ColorDepth = ColorDepth.Depth32Bit;
                        lis.ColorDepth = ColorDepth.Depth32Bit;
                        int j = 0;
                        foreach (ImageData id in il)
                        {
                            try
                            {
                                //List view items
                                ListViewItem lvi = new ListViewItem();
                                lvi.Text = id.PhotoSourceFileName;
                                lvi.Tag = id.ReferenceID;
                                lvi.ImageIndex = j;
                                //this.listViewAct.Items[j].ImageIndex = j;
                                lvi.SubItems.Add(id.DateTimeOriginal.Replace(Environment.NewLine, ", "));
                                lvi.SubItems.Add(id.ExifGPS.Replace(Environment.NewLine, ", "));
                                lvi.SubItems.Add(id.Title);
                                lvi.SubItems.Add(id.Comments);
                                this.listViewAct.Items.Add(lvi);

                                //images (large and small icons)
                                Image img = Image.FromFile(id.ReferenceIDPath);
                                lil.Images.Add(Functions.getThumbnailWithBorder(lil.ImageSize.Width, img));
                                lis.Images.Add(Functions.getThumbnailWithBorder(lis.ImageSize.Width, img));
                                img.Dispose();
                            }
                            catch (Exception)
                            {
                                //throw;
                            }
                            j++;
                        }

                        this.listViewAct.LargeImageList = lil;
                        this.listViewAct.SmallImageList = lis;
                    }
                    else
                    {
                        this.listViewAct.Items.Clear();
                    }
                }
                else
                {
                    this.listViewAct.Items.Clear();
                }
            }
            catch (Exception)
            {
                //throw;
            }
        }
        private void FillTreeViewActivities()
        {
            this.treeViewActivities.Nodes.Clear();
            this.ActivityNodes.Clear();
            this.NumDayNodes = 0;
            try
            {

                IEnumerable<IActivity> activities = GetActivities();
                TreeNode yearNode, monthNode, dayNode;
                dayNode = null;
                foreach (IActivity act in activities)
                {
                    //Year
                    string year = act.StartTime.ToLocalTime().Year.ToString();
                    TreeNode[] yearNodes = treeViewActivities.Nodes.Find(year, false);
                    if (yearNodes.Length == 0)
                    {
                        yearNode = new TreeNode(year);
                        yearNode.Name = year;
                        treeViewActivities.Nodes.Add(yearNode);
                    }
                    else yearNode = yearNodes[0];

                    //Month
                    string month = act.StartTime.ToLocalTime().ToString("MM");
                    TreeNode[] monthNodes = yearNode.Nodes.Find(month, false);
                    if (monthNodes.Length == 0)
                    {
                        monthNode = new TreeNode(act.StartTime.ToLocalTime().ToString("MMMM"));
                        monthNode.Name = month;
                        yearNode.Nodes.Add(monthNode);
                    }
                    else monthNode = monthNodes[0];

                    //Day
                    string day = act.StartTime.ToLocalTime().ToString("dd");

                    dayNode = new TreeNode(act.StartTime.ToLocalTime().ToString("dd, dddd, " +
                        System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern) + " "
                        + Resources.Resources.ImportControl_in + " " + act.Location);
                    dayNode.Name = act.StartTime.ToString("u");
                    dayNode.Tag = act;
                    this.ActivityNodes.Add(dayNode);
                    this.NumDayNodes += 1;
                    monthNode.Nodes.Add(dayNode);
                }
                this.treeViewActivities.Sort();
                this.treeViewActivities.CollapseAll();
                if (!this.ShowAllActivities) treeViewActivities.SelectedNode = dayNode;

            }
            catch (Exception)
            {
                throw;
            }
        }
        private IEnumerable<IActivity> GetActivities()
        {
            IEnumerable<IActivity> activities = new List<IActivity>();
            if (this.ShowAllActivities)
            { //add all activities
                activities = ActivityPicturePlugin.Plugin.GetIApplication().Logbook.Activities;
            }
            else
            { //add only current activity
                if (((ActivityPicturePlugin.UI.Activities.ActivityPicturePageControl)(this.Parent.Parent)).Activity != null)
                {
                    //TODO:
                    IList<IActivity> activities2 = new List<IActivity>();
                    activities2.Add(((ActivityPicturePlugin.UI.Activities.ActivityPicturePageControl)(this.Parent.Parent)).Activity);
                    activities = activities2;
                }
            }
            return activities;
        }
        private void FindImagesInActivities()
        {
            this.splitContainer1.Height = this.Height - 56;
            this.progressBar2.Visible = true;
            this.lblProgress.Visible = true;
            this.progressBar2.Minimum = 0;
            this.progressBar2.Maximum = NumDayNodes;
            this.progressBar2.Value = 0;
            this.progressBar2.Step = 1;
            this.listViewAct.Items.Clear();
            foreach (TreeNode year in this.treeViewActivities.Nodes)
            {
                foreach (TreeNode month in year.Nodes)
                {
                    foreach (TreeNode day in month.Nodes)
                    {
                        this.lblProgress.Text = Resources.Resources.ImportControl_scanning + " " + day.Text;
                        this.progressBar2.PerformStep();
                        Application.DoEvents();
                        //Show Thumbs if called from activity
                        if (day.Tag is IActivity) AddImagesToListViewAct(day, !this.showallactivities);
                    }
                }
            }

            this.progressBar2.Visible = false;
            this.lblProgress.Visible = false;
            this.splitContainer1.Height = this.Height;
        }
        #endregion
        private void FindAndImportImages(bool sorted)
        {
            try
            {
                NodeSorter ns = new NodeSorter();
                this.ActivityNodes.Sort(ns.Compare);

                DateTime FirstStart = new DateTime();
                DateTime LastEnd = new DateTime();
                IActivity FirstAct, LastAct;

                if (this.ActivityNodes[0].Tag is IActivity)
                {
                    FirstAct = (IActivity)(this.ActivityNodes[0].Tag);
                    FirstStart = FirstAct.StartTime.ToLocalTime();
                }

                if (this.ActivityNodes[this.ActivityNodes.Count - 1].Tag is IActivity)
                {
                    LastAct = (IActivity)(this.ActivityNodes[this.ActivityNodes.Count - 1].Tag);
                    LastEnd = GetActivityEndTime(LastAct);
                }


                IActivity CurrentActivity;
                int CurrentIndex = 0;
                CurrentActivity = (IActivity)(this.ActivityNodes[0].Tag);


                this.progressBar2.Style = ProgressBarStyle.Continuous;
                this.progressBar2.Minimum = 0;
                this.progressBar2.Maximum = 100;
                this.progressBar2.Value = 0;
                int i = 0;
                DateTime FileTime = new DateTime();
                foreach (FileInfo file in files)
                {

                    this.progressBar2.Value = (int)(100 * (double)(i) / (double)(files.Count));
                    this.lblProgress.Text = Resources.Resources.ImportControl_searchingActivity + " " + file.FullName;

                    Application.DoEvents();
                    i++;

                    FileTime = Functions.GetFileTime(file.FullName);

                    //{ //A valid EXIF metadata has been found                
                    if ((FileTime > FirstStart) &
                        (FileTime < LastEnd))
                    {//dateTime im picture is within the range of all activities

                        if (sorted) //only if number of files <500, importing with a prior sorting is faster. For large amounts of files, sortings gets very slow
                        {
                            if (FileTime > CurrentActivity.StartTime.ToLocalTime())
                            {
                                if (FileTime > GetActivityEndTime(CurrentActivity))
                                {
                                    //picture has been taken later => cycle to next activity
                                    while (CurrentIndex < this.ActivityNodes.Count)
                                    {
                                        CurrentIndex++;
                                        CurrentActivity = (IActivity)(this.ActivityNodes[CurrentIndex].Tag);
                                        if (FileTime < GetActivityEndTime(CurrentActivity)) break;
                                    }
                                    if (!(CurrentIndex < this.ActivityNodes.Count)) //cycled through all activities, no match found
                                    {
                                        break;//break foreach file loop
                                    }
                                }

                                if (FileTime > CurrentActivity.StartTime.ToLocalTime())
                                {
                                    //the picture has been taken during the activity
                                    ActivityPicturePlugin.UI.Activities.PluginData data = Helper.Functions.ReadExtensionData(CurrentActivity);

                                    //Check if Image does already exist in the current activity
                                    if (!ImageAlreadyExistsInActivity(file.Name, data))
                                    {
                                        this.ActivityNodes[CurrentIndex].BackColor = Color.Yellow;
                                        this.ActivityNodes[CurrentIndex].Parent.BackColor = Color.Yellow;
                                        this.ActivityNodes[CurrentIndex].Parent.Parent.BackColor = Color.Yellow; //activity node plus parents will be marked yellow to track acts to which images have been added
                                        ImageDataSerializable ids = GetImageDataSerializableFromFile(file.FullName);
                                        if (ids != null) data.Images.Add(ids);
                                        Functions.WriteExtensionData(CurrentActivity, data);
                                        numFilesImported++;
                                        continue; //proceed to next file
                                    }
                                }
                            }

                        }
                        else //importing without prior sorting
                        {
                            foreach (TreeNode tn in this.ActivityNodes)
                            {
                                IActivity act = (IActivity)(tn.Tag);

                                // picture has been taken before => abort search (list is sorted after datetime)
                                if (FileTime < act.StartTime.ToLocalTime()) break;

                                // picture has been taken after end => cycle to next activity
                                if (FileTime > GetActivityEndTime(act)) continue;

                                // if the code comes here, the picture should have been taken during the activity!
                                ActivityPicturePlugin.UI.Activities.PluginData data = Helper.Functions.ReadExtensionData(act);

                                //Check if Image does already exist in the current activity
                                if (!ImageAlreadyExistsInActivity(file.Name, data))
                                {
                                    tn.BackColor = Color.Yellow;
                                    tn.Parent.BackColor = Color.Yellow;
                                    tn.Parent.Parent.BackColor = Color.Yellow; //activity node plus parents will be marked yellow to track acts to which images have been added
                                    ImageDataSerializable ids = GetImageDataSerializableFromFile(file.FullName);
                                    if (ids != null) data.Images.Add(ids);
                                    Functions.WriteExtensionData(act, data);
                                    numFilesImported++;
                                }
                            }
                        }
                    }
                }

                this.lblProgress.Text = String.Format(Resources.Resources.ImportControl_scanDone,
                    +numFilesImported);
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private static bool ImageAlreadyExistsInActivity(string fileName, ActivityPicturePlugin.UI.Activities.PluginData data)
        {
            if (data.Images.Count != 0)
            {
                foreach (ImageDataSerializable ids in data.Images)
                {
                    FileInfo fi = new FileInfo(ids.PhotoSource);
                    if (fi.Name == fileName) return true;
                }
            }
            return false;
        }
        private static ImageDataSerializable GetImageDataSerializableFromFile(string file)
        {
            ImageDataSerializable ids = null;
            ImageData id = new ImageData();
            id.PhotoSource = file;
            id.ReferenceID = Guid.NewGuid().ToString();

            ImageData.DataTypes dt = Functions.GetMediaType(file);

            if (dt == ImageData.DataTypes.Image)
            {
                id.Type = ImageData.DataTypes.Image;
                id.SetThumbnail();
            }
            else if (dt == ImageData.DataTypes.Video)
            {
                id.Type = ImageData.DataTypes.Video;
                id.SetVideoThumbnail();
            }

            if (dt != ImageData.DataTypes.Nothing)
            {
                ids = new ImageDataSerializable();
                ids.PhotoSource = id.PhotoSource;
                ids.ReferenceID = id.ReferenceID;
                ids.Type = id.Type;
            }
            return ids;
        }

        private static DateTime GetActivityEndTime(IActivity Act)
        {
            TimeSpan Pauses = new TimeSpan();
            foreach (IValueRange<DateTime> pause in Act.TimerPauses)
            {
                Pauses += (pause.Upper - pause.Lower);
            }
            return Act.StartTime.ToLocalTime() + Act.TotalTimeEntered + Pauses;
        }
        private void ThreadGetImages()
        {
            foreach (TreeNode n in this.SelectedNodes)
            {
                GetImageFiles(n);
            }
            BeginInvoke(onImagesComplete, new object[] { this, EventArgs.Empty });
        }
        private EventHandler onImagesComplete;
        private void GetImageFiles(TreeNode node) //finds all images of the selected directory
        {
            try
            {
                if (node.Tag is DirectoryInfo)
                {
                    DirectoryInfo dir = (DirectoryInfo)(node.Tag);

                    //Check if directory has been expanded before
                    if (node.Nodes.Count != 0)
                    {
                        if (node.Nodes[0].Text == "Dummy")
                        {
                            //node is checked and has not been expanded before!
                            //==> check all paths below
                            DirectoryInfo[] dirSubs = dir.GetDirectories();
                            foreach (DirectoryInfo dirsub in dirSubs)
                            {
                                TreeNode subNode = new TreeNode(dirsub.Name);
                                subNode.Nodes.Add("Dummy");
                                subNode.Tag = dirsub;
                                GetImageFiles(subNode);
                            }
                        }
                    }

                    FileInfo[] dirfiles = dir.GetFiles();
                    foreach (FileInfo file in dirfiles)
                    {
                        if (Functions.GetMediaType(file.Extension) != ImageData.DataTypes.Nothing)
                        {
                            if (!files.Contains(file))
                            {
                                files.Add(file);
                                SetLabelText(Resources.Resources.ImportControl_addingFile + " " + file.Name);
                                Application.DoEvents();
                            }

                        }
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetLabelText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lblProgress.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetLabelText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.lblProgress.Text = text;
            }
            //Application.DoEvents();

        }
        delegate void SetTextCallback(string text);
        private void OnImagesComplete(object sender, EventArgs e)
        {
            //this.progressBar2.Style = ProgressBarStyle.Marquee;
            //SetLabelText("Sorting files");
            ////this.progressBar2.Style = ProgressBarStyle.Blocks;
            ////this.progressBar2.Maximum = 100;
            ////this.progressBar2.Value = 100;
            this.lblProgress.Text = "Sorting " + files.Count + " images";
        }
        private void AddSelectedImagesToActivity(ListViewItem[] lvisel)
        {
            try
            {
                IActivity act = (IActivity)(this.treeViewActivities.SelectedNode.Tag);
                ActivityPicturePlugin.UI.Activities.PluginData data = Helper.Functions.ReadExtensionData(act);

                //Check if Image does already exist in the current activity
                foreach (ListViewItem lvi in lvisel)
                {
                    string s = (string)(lvi.Tag);
                    FileInfo fi = new FileInfo(s);
                    if (!ImageAlreadyExistsInActivity(fi.Name, data))
                    {
                        ImageDataSerializable ids = GetImageDataSerializableFromFile(s);
                        if (ids != null) data.Images.Add(ids);
                        Functions.WriteExtensionData(act, data);
                    }
                }
                //to refresh the ListView
                AddImagesToListViewAct(this.treeViewActivities.SelectedNode, true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Eventhandlers
        private void ImportControl_Load(object sender, EventArgs e)
        {
            this.Size = this.Parent.Size;
        }
        #region treeViewImages
        private void treeViewImages_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (!notfireevent) //prevent recursive firing of events
            {
                TreeView tvw = (TreeView)(sender);
                TreeNode currentNode = e.Node;

                //Check if the node has been expanded before,
                // if not, add new nodes
                if (e.Node.Nodes.Count > 0)
                {
                    if (e.Node.Nodes[0].Text == "Dummy")
                    {
                        e.Node.Nodes.Clear();
                        GetSubDirectoryNodes(e.Node);
                    }
                }
            }
        }
        private void treeViewImages_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (!notfireevent) SetCheck(e.Node, e.Node.Checked);
        }
        private void treeViewImages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is DirectoryInfo)
            {
                DriveDir = new DirectoryInfo(e.Node.FullPath);
                //Thread td1 = new Thread(new ThreadStart(ShowFolderPics));
                //td1.Start();
                //td1.Join();

                ShowFolderPics();
                if (e.Node.Nodes.Count > 0)
                {
                    if (e.Node.Nodes[0].Text == "Dummy")
                    {
                        e.Node.Nodes.Clear();
                        GetSubDirectoryNodes(e.Node);
                    }
                }
            }
        }
        #endregion
        #region treeViewActivities
        private void treeViewActivities_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                if (e.Node.Tag is IActivity) //Activity is selected
                {
                    AddImagesToListViewAct(e.Node, true);
                    return;
                }
            }
            //if no activity is selected
            this.listViewAct.Items.Clear();
        }
        #endregion
        private void btnScan_Click(object sender, EventArgs e)
        {
            this.splitContainer1.Height = this.Height - 56;
            this.progressBar2.Style = ProgressBarStyle.Marquee;
            this.progressBar2.Visible = true;
            this.lblProgress.Visible = true;
            Application.DoEvents();
            files.Clear();
            //Thread td1 = new Thread(new ThreadStart(ThreadGetImages));
            //td1.Start();
            //td1.Join();
            ThreadGetImages(); //collects images of all selected paths
            if (files.Count < 400)
            {
                SortFiles();
                FindAndImportImages(true);
            }
            else FindAndImportImages(false);

            AddImagesToListViewAct(this.treeViewActivities.SelectedNode, true);
            this.progressBar2.Visible = false;
            this.lblProgress.Visible = false;
            this.splitContainer1.Height = this.Height;
        }
        private void SortFiles()
        {
            //start sorting
            Application.DoEvents();
            FileSorter fs = new FileSorter();
            files.Sort(fs.Compare);
        }
        private void btnViewFolder_Click(object sender, EventArgs e)
        {
            if (viewActivity == 4) viewActivity = 0;
            else viewActivity++;
            this.listViewDrive.View = (View)(viewActivity);
            this.listViewDrive.Invalidate();
        }
        private void btnViewAct_Click(object sender, EventArgs e)
        {
            if (viewFolder == 4) viewFolder = 0;
            else viewFolder++;
            this.listViewAct.View = (View)(viewFolder);
            this.listViewAct.Invalidate();
        }
        private void listViewDrive_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.A:
                        foreach (ListViewItem l in listViewDrive.Items)
                        {
                            l.Selected = true;
                        }
                        break;
                    case Keys.C:
                        break;
                }
            }
        }
        private void listViewDrive_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem[] l = new ListViewItem[this.listViewDrive.SelectedItems.Count];
            for (int i = 0; i < this.listViewDrive.SelectedItems.Count; i++)
            {
                l[i] = this.listViewDrive.SelectedItems[i];
            }
            DoDragDrop(l, DragDropEffects.Copy);
        }
        private void listViewDrive_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string file = (string)(listViewDrive.FocusedItem.Tag);
                ImageData.DataTypes dt = Functions.GetMediaType(file);
                if (dt == ImageData.DataTypes.Image)
                {
                    Functions.OpenImage(file, "");
                }
                else if (dt == ImageData.DataTypes.Video)
                {
                    Functions.OpenVideoInExternalWindow(file);
                }
                return;
            }
            catch (Exception)
            {
                //throw;
            }
        }
        private void listViewAct_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void listViewAct_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem[]", false))
                {
                    ListViewItem[] l = (ListViewItem[])(e.Data.GetData("System.Windows.Forms.ListViewItem[]"));
                    AddSelectedImagesToActivity(l);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void listViewAct_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                case Keys.Back:
                    //Delete selected images
                    IActivity act = (IActivity)(this.treeViewActivities.SelectedNode.Tag);
                    ActivityPicturePlugin.UI.Activities.PluginData data = Helper.Functions.ReadExtensionData(act);
                    List<string> idList = new List<string>();
                    foreach (ListViewItem lvi in this.listViewAct.SelectedItems)
                    {
                        string id = (string)(lvi.Tag);
                        foreach (ImageDataSerializable ids in data.Images)
                        {
                            if (ids.ReferenceID == id)
                            {
                                data.Images.Remove(ids);
                                idList.Add(id);
                                break;
                            }
                        }
                        lvi.Remove();
                    }
                    Functions.WriteExtensionData(act, data);
                    Functions.DeleteThumbnails(idList);
                    if (data.Images.Count == 0) this.treeViewActivities.SelectedNode.BackColor = Color.White;
                    CheckColorTreeNode(this.treeViewActivities.SelectedNode.Parent);
                    CheckColorTreeNode(this.treeViewActivities.SelectedNode.Parent.Parent);
                    break;
                case Keys.A:
                    foreach (ListViewItem l in listViewAct.Items)
                    {
                        l.Selected = true;
                    }
                    break;
                case Keys.V:
                    if (e.Control)
                    {
                        ListViewItem[] lvisel = new ListViewItem[this.listViewDrive.SelectedItems.Count];
                        this.listViewDrive.SelectedItems.CopyTo(lvisel, 0);
                        AddSelectedImagesToActivity(lvisel);
                    }
                    break;
            }
        }

        private void CheckColorTreeNode(TreeNode node)
        {
            bool foundimages = false;
            foreach (TreeNode n in node.Nodes)
            {
                IActivity a = (IActivity)(n.Tag);
                ActivityPicturePlugin.UI.Activities.PluginData d = Helper.Functions.ReadExtensionData(a);
                if (d.Images.Count != 0)
                {
                    foundimages = true;
                    break;
                }
            }
            if (!foundimages) node.BackColor = Color.White;
        }
        private void listViewAct_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string id = (string)(listViewAct.FocusedItem.Tag);
                string photosource = "";
                IActivity act = (IActivity)(this.treeViewActivities.SelectedNode.Tag);
                ActivityPicturePlugin.UI.Activities.PluginData data = Helper.Functions.ReadExtensionData(act);

                foreach (ImageDataSerializable ids in data.Images)
                {
                    if (ids.ReferenceID == id)
                    {
                        photosource = ids.PhotoSource;
                        if (ids.Type == ImageData.DataTypes.Image)
                        {
                            Functions.OpenImage(photosource, id);
                        }
                        else if (ids.Type == ImageData.DataTypes.Video)
                        {
                            Functions.OpenVideoInExternalWindow(photosource);
                        }
                        return;
                    }
                }
            }
            catch (Exception)
            {
                //throw;
            }

        }
        #endregion
    }

    public class NodeSorter : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            string strx, stry;

            if (tx.Tag is ImageData) strx = ((ImageData)(tx.Tag)).PhotoSourceFileName;
            else strx = tx.Name;

            if (ty.Tag is ImageData) stry = ((ImageData)(ty.Tag)).PhotoSourceFileName;
            else stry = ty.Name;

            return string.Compare(strx, stry);
        }
    }
    public class FileSorter : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            try
            {
                FileInfo tx = x as FileInfo;
                FileInfo ty = y as FileInfo;
                string strx, stry;
                try
                {
                    ExifDirectory ex = SimpleRun.ShowOneFileExifDirectory(tx.FullName);
                    strx = ex.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
                }
                catch (Exception)
                {
                    strx = "9999";
                }
                try
                {
                    ExifDirectory ey = SimpleRun.ShowOneFileExifDirectory(ty.FullName);
                    stry = ey.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
                }
                catch (Exception)
                {
                    stry = "9999";
                }
                return string.Compare(strx, stry);
            }
            catch (Exception)
            {
                return 0;
                //throw;
            }

        }
    }
}
