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
using ActivityPicturePlugin.Settings;
using ZoneFiveSoftware.Common.Data.Fitness;
using ZoneFiveSoftware.Common.Data;
using ZoneFiveSoftware.Common.Visuals;
using com.drew.metadata.exif;

namespace ActivityPicturePlugin.UI
{
    public partial class ImportControl : UserControl
    {
        #region public methods
        public ImportControl()
        {
            InitializeComponent();

            InitComponent();
            //InitializeListViewDrive();
            onImagesComplete = new EventHandler( OnImagesComplete );
            treeViewActivities.TreeViewNodeSorter = new NodeSorter();
            this.m_SelectedNodes = new List<TreeNode>();
            this.m_ActivityNodes = new List<TreeNode>();
        }

        public void InitComponent()
        {
            m_viewActivity = ActivityPicturePlugin.Source.Settings.ActivityView; //View of ListviewAct
            m_viewFolder = ActivityPicturePlugin.Source.Settings.FolderView;	 //View of ListviewDrive
            this.listViewDrive.View = (View)( m_viewFolder );
            this.listViewAct.View = (View)( m_viewActivity );
        }

        // Used for file sorting
        public class FileInfoEx
        {
            public FileInfo fi;
            public string strDateTime;
        }

        public void UpdateUICulture( System.Globalization.CultureInfo culture )
        {
            m_culture = culture;

            using ( Graphics g = this.CreateGraphics() )
            {
                this.btnChangeFolderView.Text = Resources.Resources.ImportControl_changeView;
                this.btnChangeFolderView.Width = (int)g.MeasureString( this.btnChangeFolderView.Text, this.btnChangeFolderView.Font ).Width + 10; ;
                this.toolTip1.SetToolTip( this.btnChangeFolderView, Resources.Resources.ChangeFileListview_Text );
                this.toolTip1.SetToolTip( this.btnChangeActivityView, Resources.Resources.ChangeImageListview_Text );
                this.btnScan.Text = Resources.Resources.ActionImport_Text;
                this.toolTip1.SetToolTip( this.btnScan, Resources.Resources.AutoImportFromFolders_Text );
                this.btnScan.Width = (int)g.MeasureString( this.btnScan.Text, this.btnScan.Font ).Width + 10; ;
                this.btnScan.Left = btnChangeFolderView.Location.X + btnChangeFolderView.Width + 10;
                this.btnChangeActivityView.Text = Resources.Resources.ImportControl_changeView;
                this.btnChangeActivityView.Width = (int)g.MeasureString( this.btnChangeActivityView.Text, this.btnChangeActivityView.Font ).Width + 10; ;

                if ( m_showallactivities )
                {
                    this.btnExpandAll.Text = Resources.Resources.btnExpandAll_Text;
                    this.btnExpandAll.Left = btnChangeActivityView.Location.X + btnChangeActivityView.Width + 10;
                    this.btnCollapseAll.Text = Resources.Resources.btnCollapseAll_Text;
                    this.btnExpandAll.Visible = true;
                    this.btnCollapseAll.Visible = true;
                }
                else
                {
                    // btnExpandAll and btnCollapseAll are not very useful
                    // in Detail View and just add clutter to an already small
                    // display.  Either change their text or make them invisible
                    // Decided to do both.
                    this.btnExpandAll.Text = "+";
                    this.toolTip1.SetToolTip( this.btnExpandAll, Resources.Resources.btnExpandAll_Text );
                    this.btnCollapseAll.Text = "-";
                    this.toolTip1.SetToolTip( this.btnCollapseAll, Resources.Resources.btnCollapseAll_Text );
                    this.btnExpandAll.Visible = false;
                    this.btnCollapseAll.Visible = false;
                }

                this.btnExpandAll.Width = (int)g.MeasureString( this.btnExpandAll.Text, this.btnExpandAll.Font ).Width + 10; ;
                this.btnCollapseAll.Width = (int)g.MeasureString( this.btnCollapseAll.Text, this.btnCollapseAll.Font ).Width + 10; ;
                this.btnCollapseAll.Left = btnExpandAll.Location.X + btnExpandAll.Width + 10;

                this.splitContainer1.Panel1MinSize = btnScan.Width + btnScan.Left + 10;
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

                this.toolStripMenuAdd.Text = CommonResources.Text.ActionAdd;
                this.toolStripMenuCopyToClipboard.Text = CommonResources.Text.ActionCopy;
                this.toolStripMenuRemove.Text = CommonResources.Text.ActionRemove;
                this.toolStripMenuRefresh.Text = CommonResources.Text.ActionRefresh;
                this.toolStripMenuOpenFolder.Text = Resources.Resources.OpenContainingFolder_Text;
            }
        }

        public void ThemeChanged( ZoneFiveSoftware.Common.Visuals.ITheme visualTheme )
        {
            this.BackColor = visualTheme.Control;
            this.ForeColor = visualTheme.ControlText;
            this.progressBar2.BackColor = visualTheme.Control;
            this.progressBar2.ForeColor = visualTheme.Selected;
            if ( this.Enabled )
            {
                treeViewImages.BackColor = visualTheme.Window;
                treeViewActivities.BackColor = visualTheme.Window;
                listViewDrive.BackColor = visualTheme.Window;
                listViewAct.BackColor = visualTheme.Window;
            }
            else
            {
                treeViewImages.BackColor = visualTheme.Control;
                treeViewActivities.BackColor = visualTheme.Control;
                listViewDrive.BackColor = visualTheme.Control;
                listViewAct.BackColor = visualTheme.Control;
            }
        }
        #endregion

        #region Callbacks
        delegate void SetTextCallback( string text );
        private EventHandler onImagesComplete;
        #endregion

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
        private const string gDummyFolder = "*****";

        private List<FileInfo> m_files = new List<FileInfo>(); //List of all images files of selected folders (for automatic import)
        private List<TreeNode> m_ActivityNodes = new List<TreeNode>();//List of all activities (used for comparing and organizing)
        private List<TreeNode> m_SelectedNodes = new List<TreeNode>();//Nodes that are selected in the Drive TreeView
        private DirectoryInfo m_DriveDir;
        private bool m_showallactivities;
        private bool m_notfireevent;
        private bool m_standardpathalreadyshown;
        private int m_viewActivity = 0; //View of ListviewAct
        private int m_viewFolder = 0; //View of ListviewDrive
        private int m_NumDayNodes = 0; //needed for progress bar
        private int m_numFilesImported = 0;
        private System.Globalization.CultureInfo m_culture = System.Globalization.CultureInfo.CurrentUICulture;
        #endregion

        #region Public properties
        public bool ShowAllActivities
        {
            get { return m_showallactivities; }
            set { m_showallactivities = value; }
        }
        #endregion

        #region private methods

        #region TreeViewImages
        public void LoadNodes()
        {
            m_numFilesImported = 0;
            m_files.Clear();
            ResetProgressBar();

            UpdateUICulture( m_culture );

            if ( !m_standardpathalreadyshown )
            {
                FillTreeViewImages();
            }
            if ( !m_standardpathalreadyshown | this.m_showallactivities == false )
            {
                FillTreeViewActivities();
                if ( this.ShowAllActivities ) FindImagesInActivities();

                m_standardpathalreadyshown = true;
            }
        }

        private void SetCheck( TreeNode node, bool check )
        {
            if ( node.Checked )
            {
                if ( !this.m_SelectedNodes.Contains( node ) ) this.m_SelectedNodes.Add( node );
            }
            else
            {
                if ( this.m_SelectedNodes.Contains( node ) ) this.m_SelectedNodes.Remove( node );
            }
            foreach ( TreeNode n in node.Nodes )
            {
                m_notfireevent = true;
                n.Checked = check;
                m_notfireevent = false;
                if ( ( n.Tag is FileInfo ) | ( n.Tag is DirectoryInfo ) )
                {
                    if ( n.Checked )
                    {
                        if ( !this.m_SelectedNodes.Contains( n ) ) this.m_SelectedNodes.Add( n );
                    }
                    else
                    {
                        if ( this.m_SelectedNodes.Contains( n ) ) this.m_SelectedNodes.Remove( n );
                    }
                }
                if ( n.Nodes.Count != 0 ) SetCheck( n, check );
            }
        }

        private void GetSubDirectoryNodes( TreeNode parentNode )
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo( parentNode.FullPath );
                DirectoryInfo[] dirSubs = dir.GetDirectories();
                foreach ( DirectoryInfo dirsub in dirSubs )
                {
                    if ( ( ( dirsub.Attributes & FileAttributes.System ) == FileAttributes.System ) ||
                        ( ( dirsub.Attributes & FileAttributes.Hidden ) == FileAttributes.Hidden ) )
                        continue;	// Do not display hidden or system folders

                    TreeNode subNode = new TreeNode( dirsub.Name );
                    try
                    {
                        if ( dirsub.GetDirectories().Length != 0 ) subNode.Nodes.Add( gDummyFolder );
                        subNode.Tag = dirsub;
                        parentNode.Nodes.Add( subNode );
                        subNode.Checked = parentNode.Checked;
                    }
                    catch ( Exception )
                    { /* Probably an Access Denied error.  Move on to the next folder.*/ }
                }
            }
            catch ( Exception )
            {
                //throw;
            }
        }

        private void ShowFolderPics() //Adds the images of a directory to the ListViewDrive
        {
            ImageList lvImgL = null;
            ImageList lvImgS = null;
            try
            {
                m_files.Clear();
                m_files.InsertRange( 0, m_DriveDir.GetFiles() );

                listViewDrive.Items.Clear();

                lvImgL = new ImageList();
                lvImgL.ImageSize = new Size( 100, 100 );
                lvImgL.ColorDepth = ColorDepth.Depth32Bit;

                if ( listViewDrive.LargeImageList != null )
                {
                    listViewDrive.LargeImageList.Dispose();
                    listViewDrive.LargeImageList = null;
                }
                listViewDrive.LargeImageList = lvImgL;


                lvImgS = new ImageList();
                lvImgS.ImageSize = new Size( 50, 50 );
                lvImgS.ColorDepth = ColorDepth.Depth32Bit;

                if ( listViewDrive.SmallImageList != null )
                {
                    listViewDrive.SmallImageList.Dispose();
                    listViewDrive.SmallImageList = null;
                }
                listViewDrive.SmallImageList = lvImgS;

                progressBar2.Style = ProgressBarStyle.Continuous;
                timerProgressBar.Enabled = false;

                //Either speed up adding to listview or show progressbar
                using ( Image bmp = (Bitmap)( Resources.Resources.image ).Clone() )
                {
                    for ( int j = 0; j < m_files.Count; j++ ) //add images plus exif data
                    {
                        if ( progressBar2.Maximum != m_files.Count ) progressBar2.Maximum = m_files.Count;
                        progressBar2.Value = j;
                        ImageData.DataTypes dt = Functions.GetMediaType( m_files[j].Extension );
                        if ( dt == ImageData.DataTypes.Nothing )
                        {
                            m_files.Remove( m_files[j] );
                            j--;
                        }
                        else
                        {
                            lvImgL.Images.Add( Functions.getThumbnailWithBorder( lvImgL.ImageSize.Width, bmp ) );
                            lvImgS.Images.Add( Functions.getThumbnailWithBorder( lvImgS.ImageSize.Width, bmp ) );
                            AddFileToListView( m_files[j], dt );			//read images and add thumbnails
                            AddImageToListView( lvImgL, lvImgS, dt, j );	//read images and add thumbnails
                        }
                        lblProgress.Text = String.Format( Resources.Resources.FoundImagesInFolder_Text, j + 1 );
                    }
                    progressBar2.Value = progressBar2.Maximum;
                    timerProgressBar.Enabled = true;
                    //bmp.Dispose();
                }
            }
            catch ( Exception )
            {
                if ( lvImgS != null )
                    lvImgS.Dispose();
                lvImgS = null;

                if ( lvImgL != null )
                    lvImgL.Dispose();
                lvImgL = null;

                // Apparently we need to set these to null after 
                // disposing BOTH of the above imagelists.
                listViewDrive.SmallImageList = null;
                listViewDrive.LargeImageList = null;

                // throw;

            }
            

        }

        private void FillTreeViewImages()
        {
            try
            {
                this.treeViewImages.Nodes.Clear();
                this.m_SelectedNodes.Clear();

                string[] drives = Environment.GetLogicalDrives();
                DirectoryInfo startdir = new DirectoryInfo( Environment.GetFolderPath( Environment.SpecialFolder.MyPictures ) );

                foreach ( string rootDir in drives )
                {
                    DirectoryInfo dir = new DirectoryInfo( rootDir );
                    TreeNode ndRoot = new TreeNode( rootDir );
                    ndRoot.Tag = dir;

                    //Fill standard path
                    this.treeViewImages.Nodes.Add( ndRoot );
                    if ( dir.Root.Name == startdir.Root.Name )
                    {
                        AddStandardPath( this.treeViewImages, startdir, ndRoot );
                    }
                    else
                    {
                        ndRoot.Nodes.Add( gDummyFolder );
                    }
                }
            }
            catch ( Exception )
            {
                // throw;
            }

        }

        private void AddStandardPath( TreeView tvw, DirectoryInfo startdir, TreeNode ndRoot )
        {
            try
            {
                TreeNode parentNode = null;
                List<DirectoryInfo> dirs = new List<DirectoryInfo>();

                while ( true )
                {
                    if ( startdir.Parent != null )
                    {
                        dirs.Add( startdir );
                        startdir = startdir.Parent;
                    }
                    else break;
                }

                dirs.Reverse();
                parentNode = ndRoot;

                foreach ( DirectoryInfo subdir in dirs )
                {
                    TreeNode subNodeP = new TreeNode( subdir.Name );
                    subNodeP.Tag = subdir;
                    parentNode.Nodes.Add( subNodeP );

                    DirectoryInfo dir = new DirectoryInfo( parentNode.FullPath );
                    DirectoryInfo[] dirSubs = dir.GetDirectories();
                    foreach ( DirectoryInfo dirsub in dirSubs )
                    {
                        if ( ( ( dirsub.Attributes & FileAttributes.System ) == FileAttributes.System ) ||
                            ( ( dirsub.Attributes & FileAttributes.Hidden ) == FileAttributes.Hidden ) )
                            continue;	// Do not display hidden or system folders

                        DirectoryInfo dirP = (DirectoryInfo)( subNodeP.Tag );
                        if ( dirsub.Name != dirP.Name )
                        //otherwise the folders of the standard path will be created twice
                        {
                            TreeNode subNode = new TreeNode( dirsub.Name );
                            int i = 0;
                            try
                            {
                                i = dirsub.GetDirectories().Length;
                            }
                            catch ( UnauthorizedAccessException ex )
                            {
                                //Nothing to do
                                // Folder is inaccessible for whatever reason
                                // Don't add the node
                                i = 0;
                            }
                            catch ( Exception )
                            {
                                // Don't add the node
                                i = 0;
                            }
                            if ( i != 0 ) subNode.Nodes.Add( gDummyFolder );
                            subNode.Tag = dirsub;
                            parentNode.Nodes.Add( subNode );
                        }
                    }
                    parentNode = subNodeP;
                }

                m_DriveDir = new DirectoryInfo( parentNode.FullPath );
                ShowFolderPics();

                if ( m_DriveDir.GetDirectories().Length != 0 ) parentNode.Nodes.Add( gDummyFolder );
                m_notfireevent = true;
                parentNode.EnsureVisible();
                parentNode.TreeView.SelectedNode = parentNode;
                m_notfireevent = false;
            }
            catch ( Exception )
            {
                //throw;
            }
        }

        #endregion

        #region TreeViewActivities

        private void FillTreeViewActivities( bool bSelectCurrentActivity = true )
        {
            this.treeViewActivities.Nodes.Clear();
            this.m_ActivityNodes.Clear();
            this.m_NumDayNodes = 0;
            try
            {
                IEnumerable<IActivity> activities = GetActivities();
                TreeNode yearNode, monthNode, dayNode;
                dayNode = null;
                foreach ( IActivity act in activities )
                {
                    //Year
                    string year = act.StartTime.ToLocalTime().Year.ToString();
                    TreeNode[] yearNodes = treeViewActivities.Nodes.Find( year, false );
                    if ( yearNodes.Length == 0 )
                    {
                        yearNode = new TreeNode( year );
                        yearNode.Name = year;
                        treeViewActivities.Nodes.Add( yearNode );
                    }
                    else yearNode = yearNodes[0];

                    //Month
                    string month = act.StartTime.ToLocalTime().ToString( "MM" );
                    TreeNode[] monthNodes = yearNode.Nodes.Find( month, false );
                    if ( monthNodes.Length == 0 )
                    {
                        monthNode = new TreeNode( act.StartTime.ToLocalTime().ToString( "MMMM" ) );
                        monthNode.Name = month;
                        yearNode.Nodes.Add( monthNode );
                    }
                    else monthNode = monthNodes[0];

                    //Day
                    string day = act.StartTime.ToLocalTime().ToString( "dd" );

                    /*dayNode = new TreeNode( act.StartTime.ToLocalTime().ToString( "dd, dddd, " +
                        System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern ) + " "
                        + Resources.Resources.ImportControl_in + " " + act.Location );*/

                    dayNode = new TreeNode( act.StartTime.ToLocalTime().ToLongDateString() + " " +
                        act.StartTime.ToLocalTime().ToShortTimeString() + " "
                        + Resources.Resources.ImportControl_in + " " + act.Location );

                    dayNode.Name = act.StartTime.ToString( "u" );
                    dayNode.Tag = act;
                    this.m_ActivityNodes.Add( dayNode );
                    this.m_NumDayNodes += 1;
                    monthNode.Nodes.Add( dayNode );
                }
                this.treeViewActivities.Sort();
                this.treeViewActivities.CollapseAll();
                if ( bSelectCurrentActivity ) treeViewActivities.SelectedNode = dayNode;
                //if ( !this.ShowAllActivities ) treeViewActivities.SelectedNode = dayNode;

            }
            catch ( Exception )
            {
                throw;
            }
        }

        private void CheckColorTreeNode( TreeNode node )
        {
            bool foundimages = false;
            foreach ( TreeNode n in node.Nodes )
            {
                IActivity a = (IActivity)( n.Tag );
                PluginData d = Helper.Functions.ReadExtensionData( a );
                if ( d.Images.Count != 0 )
                {
                    foundimages = true;
                    break;
                }
            }
            if ( !foundimages ) node.BackColor = Color.White;
        }

        private TreeNode GetNodeFromPath( TreeView tv, string sPath )
        {
            TreeNode retNode = null;
            string sDelimiter = tv.PathSeparator;
            string[] sFolders = sPath.Split( sDelimiter.ToCharArray() );
            List<string> lFolders = new List<string>();

            for ( int i = 0; i < sFolders.Length; i++ )
            {
                if ( sFolders[i] != "" )
                    lFolders.Add( sFolders[i] );
            }

            foreach ( TreeNode tn in tv.Nodes )
            {
                if ( ( tn.Text == sFolders[0] ) || ( tn.Text == sFolders[0] + sDelimiter ) )
                {
                    retNode = GetNextNodeInPath( tn, lFolders, 1 );
                    break;
                }
            }
            return retNode;
        }

        // Recursively drill down the path until we find the node we're looking for
        // iDepth is the current index in lFolders that we're looking for.
        private TreeNode GetNextNodeInPath( TreeNode tn, List<string> lFolders, int iDepth )
        {
            TreeNode retNode = null;
            try
            {
                if ( tn.Nodes[0].Text == gDummyFolder )
                {
                    tn.Nodes.Clear();
                    GetSubDirectoryNodes( tn );
                }

                if ( ( tn != null ) && ( iDepth < lFolders.Count ) )
                {
                    foreach ( TreeNode n in tn.Nodes )
                    {
                        if ( n.Text == lFolders[iDepth] )
                        {
                            if ( iDepth + 1 < lFolders.Count )	// Need to drill deeper
                                retNode = GetNextNodeInPath( n, lFolders, ++iDepth );
                            else retNode = n;	// We struck oil
                            break;
                        }
                    }
                }
            }
            catch ( Exception )
            {
            }

            return retNode;
        }

        private IEnumerable<IActivity> GetActivities()
        {
            IEnumerable<IActivity> activities = new List<IActivity>();
            if ( this.ShowAllActivities )
            { //add all activities
                activities = ActivityPicturePlugin.Plugin.GetApplication().Logbook.Activities;
            }
            else
            { //add only current activity
                if ( ( (ActivityPicturePlugin.UI.Activities.ActivityPicturePageControl)( this.Parent.Parent ) ).Activity != null )
                {
                    //TODO:
                    IList<IActivity> activities2 = new List<IActivity>();
                    activities2.Add( ( (ActivityPicturePlugin.UI.Activities.ActivityPicturePageControl)( this.Parent.Parent ) ).Activity );
                    activities = activities2;
                }
            }
            return activities;
        }

        private void FindImagesInActivities()
        {
            this.progressBar2.Minimum = 0;
            this.progressBar2.Maximum = m_NumDayNodes;
            this.progressBar2.Value = 0;
            this.progressBar2.Step = 1;
            this.timerProgressBar.Enabled = false;
            this.listViewAct.Items.Clear();
            foreach ( TreeNode year in this.treeViewActivities.Nodes )
            {
                foreach ( TreeNode month in year.Nodes )
                {
                    foreach ( TreeNode day in month.Nodes )
                    {
                        this.lblProgress.Text = Resources.Resources.ImportControl_scanning + " " + day.Text;
                        this.progressBar2.PerformStep();
                        Application.DoEvents();
                        //Show Thumbs if called from activity
                        if ( day.Tag is IActivity ) AddImagesToListViewAct( day, !this.m_showallactivities );
                    }
                }
            }
            this.progressBar2.Value = this.progressBar2.Maximum;
            this.timerProgressBar.Enabled = true;
        }

        private void RemoveSelectedImagesFromActivity( ListViewItem[] lvisel )
        {
            //Delete selected images
            IActivity act = (IActivity)( this.treeViewActivities.SelectedNode.Tag );
            PluginData data = Helper.Functions.ReadExtensionData( act );
            List<string> idList = new List<string>();
            listViewAct.SuspendLayout();

            //foreach ( ListViewItem lvi in this.listViewAct.SelectedItems )
            foreach ( ListViewItem lvi in lvisel )
            {
                string id = (string)( lvi.Tag );
                foreach ( ImageDataSerializable ids in data.Images )
                {
                    if ( ids.ReferenceID == id )
                    {
                        data.Images.Remove( ids );
                        idList.Add( id );
                        break;
                    }
                }
                lvi.Remove();
            }
            listViewAct.ResumeLayout();
            Functions.WriteExtensionData( act, data );
            Functions.DeleteThumbnails( idList );
            if ( data.Images.Count == 0 ) this.treeViewActivities.SelectedNode.BackColor = Color.White;
            CheckColorTreeNode( this.treeViewActivities.SelectedNode.Parent );
            CheckColorTreeNode( this.treeViewActivities.SelectedNode.Parent.Parent );
        }

        private void AddSelectedImagesToActivity( ListViewItem[] lvisel )
        {
            try
            {
                IActivity act = (IActivity)( this.treeViewActivities.SelectedNode.Tag );
                //Contains the list of images as they're being added
                PluginData data = Helper.Functions.ReadExtensionData( act );

                //Contains the original list of images.  Used to prevent data.Images.Count! checks
                PluginData dataTmp = Helper.Functions.ReadExtensionData( act );

                int iCount = 0;
                progressBar2.Style = ProgressBarStyle.Continuous;
                progressBar2.Maximum = lvisel.Length;
                timerProgressBar.Enabled = false;

                //Check if Image does already exist in the current activity
                foreach ( ListViewItem lvi in lvisel )
                {
                    lblProgress.Text = String.Format( Resources.Resources.CheckingForDuplicates_Text, ++iCount, lvisel.Length );
                    progressBar2.Value = iCount;

                    string s = (string)( lvi.Tag );
                    FileInfo fi = new FileInfo( s );
                    if ( !ImageAlreadyExistsInActivity( fi.Name, dataTmp ) )
                    {
                        ImageDataSerializable ids = GetImageDataSerializableFromFile( s );
                        if ( ids != null ) data.Images.Add( ids );
                        Functions.WriteExtensionData( act, data );

                        using ( ImageData ID = new ImageData( ids ) )
                        {
                            ActivityPicturePlugin.Source.Settings.NewThumbnailsCreated += ID.ThumbnailPath + "\t";
                        }

                    }

                    Application.DoEvents();
                }
                this.progressBar2.Value = this.progressBar2.Maximum;
                timerProgressBar.Enabled = true;

                //to refresh the ListView
                AddImagesToListViewAct( this.treeViewActivities.SelectedNode, true );
            }
            catch ( Exception )
            {
                throw;
            }
        }

        private static ImageDataSerializable GetImageDataSerializableFromFile( string file )
        {
            ImageDataSerializable ids = null;
            using ( ImageData id = new ImageData() )
            {
                id.PhotoSource = file;
                id.ReferenceID = Guid.NewGuid().ToString();

                ImageData.DataTypes dt = Functions.GetMediaType( file );

                if ( dt == ImageData.DataTypes.Image )
                {
                    id.Type = ImageData.DataTypes.Image;
                    id.SetThumbnail();
                }
                else if ( dt == ImageData.DataTypes.Video )
                {
                    id.Type = ImageData.DataTypes.Video;
                    id.SetVideoThumbnail();
                }

                if ( dt != ImageData.DataTypes.Nothing )
                {
                    ids = new ImageDataSerializable();
                    ids.PhotoSource = id.PhotoSource;
                    ids.ReferenceID = id.ReferenceID;
                    ids.Type = id.Type;
                }
            }
            return ids;
        }

        //Drills from 'node' to find all child activities
        //Returns tab separated list of fields
        //Tab separated list of field values added to sFileData
        //New list item added to sFileData for each image found
        private string treeViewActivities_GetImageDataFromNodeChildren( TreeNode node, List<string> sFileData )
        {
            //Hardcoded fields and order at this time
            string sFields = Resources.Resources.photoSourceDataGridViewTextBoxColumn_HeaderText + "\t" +
                CommonResources.Text.LabelDate + "\t" +
                CommonResources.Text.LabelGPSLocation + "\t" +
                Resources.Resources.titleDataGridViewTextBoxColumn_HeaderText + "\t" +
                Resources.Resources.commentDataGridViewTextBoxColumn_HeaderText + "\t" +
                Resources.Resources.referenceIDDataGridViewTextBoxColumn_HeaderText + "\t" +
                Resources.Resources.FilePath_Text + "\t" +
                Resources.Resources.thumbnailDataGridViewImageColumn_HeaderText;

            if ( node.Nodes.Count > 0 )
            {
                foreach ( TreeNode tn in node.Nodes )
                    sFields = treeViewActivities_GetImageDataFromNodeChildren( tn, sFileData );
            }
            else
            {
                if ( node.Tag != null )
                {
                    if ( node.Tag is IActivity ) //Node is an Activity
                    {
                        IActivity act = (IActivity)( node.Tag );
                        PluginData data = Helper.Functions.ReadExtensionData( act );

                        List<ImageData> il = data.LoadImageData( data.Images );
                        foreach ( ImageData id in il )
                        {
                            try
                            {
                                string sFile = id.PhotoSourceFileName + "\t" +
                                    id.DateTimeOriginal.Replace( Environment.NewLine, ", " ) + "\t" +
                                    id.ExifGPS.Replace( Environment.NewLine, ", " ) + "\t" +
                                    id.Title + "\t" +
                                    id.Comments + "\t" +
                                    id.ReferenceID + "\t" +
                                    id.PhotoSource + "\t" +
                                    id.ThumbnailPath;

                                sFileData.Add( sFile );
                            }
                            catch ( Exception )
                            {
                                //throw;
                            }
                        }
                    }
                }
                else
                {
                    //noop
                }

            }

            return sFields;
        }
        #endregion

        #region listViewDrive
        private void AddImageToListView( ImageList lvImgL, ImageList lvImgS, ImageData.DataTypes dt, int ixImage )
        {
            try
            {
                //ImageData.DataTypes dt = Functions.GetMediaType( files[j].Extension );
                if ( dt == ImageData.DataTypes.Image )
                {
                    //Test: try showing thumbnail without opening whole image
                    //byte[] imgbyte = com.SimpleRun.ShowOneFileThumbnailData(file.FullName);
                    //MemoryStream stream = new MemoryStream(imgbyte.Length);
                    //stream.Write(imgbyte, 0, imgbyte.Length);
                    //Image img = Image.FromStream(stream);
                    //if (img != null) lvImg.Images.Add(img);

                    Image img = Image.FromFile( m_files[ixImage].FullName );
                    lvImgL.Images[ixImage] = Functions.getThumbnailWithBorder( lvImgL.ImageSize.Width, img );
                    lvImgS.Images[ixImage] = Functions.getThumbnailWithBorder( lvImgS.ImageSize.Width, img );
                    img.Dispose();
                }
                else if ( dt == ImageData.DataTypes.Video )
                {
                    Image bmp = (Bitmap)( Resources.Resources.video ).Clone();
                    lvImgL.Images[ixImage] = Functions.getThumbnailWithBorder( lvImgL.ImageSize.Width, bmp );
                    lvImgS.Images[ixImage] = Functions.getThumbnailWithBorder( lvImgS.ImageSize.Width, bmp );
                    bmp.Dispose();
                }
                Application.DoEvents();
            }
            catch ( Exception )
            {
                //throw;
            }
        }

        private void AddFileToListView( FileInfo file, ImageData.DataTypes dt )
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = file.Name;
            lvi.ImageIndex = m_files.IndexOf( file );
            lvi.Tag = file.FullName;

            if ( dt == ImageData.DataTypes.Image )
            {
                try
                {
                    if (Functions.IsExifFileExt(file))
                    {
                        ExifDirectory ed = SimpleRun.ShowOneFileExifDirectory(file.FullName);
                        GpsDirectory gps = SimpleRun.ShowOneFileGPSDirectory(file.FullName);

                        if (ed != null)
                        {
                            string s = ed.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
                            IFormatProvider culture = new System.Globalization.CultureInfo("de-DE", true);
                            if (!string.IsNullOrEmpty(s))
                            {
                                string dts = DateTime.ParseExact(s, "yyyy:MM:dd HH:mm:ss", culture).ToString();
                                lvi.SubItems.Add(dts);
                            }
                        }

                        if (gps != null)
                        {
                            string latref = gps.GetDescription(GpsDirectory.TAG_GPS_LATITUDE_REF);
                            string latitude = gps.GetDescription(GpsDirectory.TAG_GPS_LATITUDE);
                            string longitude = gps.GetDescription(GpsDirectory.TAG_GPS_LONGITUDE);
                            string longref = gps.GetDescription(GpsDirectory.TAG_GPS_LONGITUDE_REF);
                            string gpsstr = latitude + " " + latref + ", " + longitude + " " + longref;
                            if (latitude != null) lvi.SubItems.Add(gpsstr);
                            else lvi.SubItems.Add("");
                        }

                        if (ed != null)
                        {
                            string s = (string)(ed.GetDescription(ExifDirectory.TAG_XP_TITLE));
                            if (!string.IsNullOrEmpty(s))
                            {
                                lvi.SubItems.Add(s);
                            }
                            s = (string)(ed.GetDescription(ExifDirectory.TAG_XP_COMMENTS)); 
                            if (!string.IsNullOrEmpty(s))
                            {
                                lvi.SubItems.Add(s);
                            }
                        }
                    }
                }
                catch ( Exception )
                {
                }

            }
            this.listViewDrive.Items.Add( lvi );
        }

        #endregion

        #region listViewAct
        private void AddImagesToListViewAct( TreeNode tn, bool AddThumbs )
        {
            ImageList lil = null;
            ImageList lis = null;

            try
            {
                this.listViewAct.Items.Clear();
                IActivity act = (IActivity)( tn.Tag );
                PluginData data = Helper.Functions.ReadExtensionData( act );

                if ( data.Images.Count > 0 )
                {
                    //change color
                    if ( tn.BackColor != Color.Yellow )
                    {
                        tn.BackColor = Color.LightBlue;
                        if ( tn.Parent != null )
                        {
                            if ( tn.Parent.BackColor != Color.Yellow ) tn.Parent.BackColor = Color.LightBlue;
                            if ( tn.Parent.Parent != null & tn.Parent.Parent.BackColor != Color.Yellow ) tn.Parent.Parent.BackColor = Color.LightBlue;
                        }
                    }

                    //Load imagedata
                    if ( AddThumbs )
                    {
                        List<ImageData> il = data.LoadImageData( data.Images );
                        lil = new ImageList();
                        lis = new ImageList();
                        lil.ImageSize = new Size( 100, 100 );
                        lis.ImageSize = new Size( 50, 50 );
                        lil.ColorDepth = ColorDepth.Depth32Bit;
                        lis.ColorDepth = ColorDepth.Depth32Bit;
                        int j = 0;

                        // Dispose of the old imagelists if they exist
                        if ( this.listViewAct.LargeImageList != null )
                            this.listViewAct.LargeImageList.Dispose();
                        this.listViewAct.LargeImageList = null;

                        if ( this.listViewAct.SmallImageList != null )
                            this.listViewAct.SmallImageList.Dispose();
                        this.listViewAct.SmallImageList = null;

                        // assign the new imagelists
                        this.listViewAct.LargeImageList = lil;
                        this.listViewAct.SmallImageList = lis;

                        progressBar2.Style = ProgressBarStyle.Continuous;
                        progressBar2.Maximum = il.Count;
                        timerProgressBar.Enabled = false;
                        foreach ( ImageData id in il )
                        {
                            try
                            {
                                //List view items
                                ListViewItem lvi = new ListViewItem();
                                lvi.Text = id.PhotoSourceFileName;
                                lvi.Tag = id.ReferenceID;
                                lvi.ImageIndex = j;
                                //this.listViewAct.Items[j].ImageIndex = j;
                                lvi.SubItems.Add( id.DateTimeOriginal.Replace( Environment.NewLine, ", " ) );
                                lvi.SubItems.Add( id.ExifGPS.Replace( Environment.NewLine, ", " ) );
                                lvi.SubItems.Add( id.Title );
                                lvi.SubItems.Add( id.Comments );
                                this.listViewAct.Items.Add( lvi );

                                //images (large and small icons)
                                Image img = Image.FromFile( id.ThumbnailPath );
                                lil.Images.Add( Functions.getThumbnailWithBorder( lil.ImageSize.Width, img ) );
                                lis.Images.Add( Functions.getThumbnailWithBorder( lis.ImageSize.Width, img ) );
                                Application.DoEvents();
                                img.Dispose();
                                img = null;
                            }
                            catch ( Exception )
                            {
                                //throw;
                            }
                            j++;
                            progressBar2.Value = j;
                            lblProgress.Text = String.Format( Resources.Resources.FoundImagesInActivity_Text, j );

                        }
                        this.progressBar2.Value = this.progressBar2.Maximum;
                        timerProgressBar.Enabled = true;

                        //this.listViewAct.LargeImageList = lil;
                        //this.listViewAct.SmallImageList = lis;
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
            catch ( Exception )
            {
                if ( lil != null )
                    lil.Dispose();
                lil = null;

                if ( lis != null )
                    lis.Dispose();
                lis = null;

                listViewAct.LargeImageList = null;
                listViewAct.SmallImageList = null;
                //throw;
            }
        }

        private static bool ImageAlreadyExistsInActivity( string fileName, PluginData dataRef )
        {
            if ( dataRef.Images.Count != 0 )
            {
                foreach ( ImageDataSerializable ids in dataRef.Images )
                {
                    FileInfo fi = new FileInfo( ids.PhotoSource );
                    if ( fi.Name == fileName ) return true;
                }
            }
            return false;
        }
        #endregion

        private void EnableControl( bool bEnable )
        {
            this.treeViewActivities.Enabled = bEnable;
            this.treeViewImages.Enabled = bEnable;
            this.listViewAct.Enabled = bEnable;
            this.listViewDrive.Enabled = bEnable;
            this.btnChangeActivityView.Enabled = bEnable;
            this.btnChangeFolderView.Enabled = bEnable;
            this.btnChangeActivityView.Enabled = bEnable;
            this.btnCollapseAll.Enabled = bEnable;
            this.btnExpandAll.Enabled = bEnable;
            this.btnScan.Enabled = bEnable;
        }

        private void ResetProgressBar()
        {
            lblProgress.Text = "";
            progressBar2.Value = 0;
            timerProgressBar.Enabled = false;
        }

        private void SetLabelText( string text )
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if ( this.lblProgress.InvokeRequired )
            {
                SetTextCallback d = new SetTextCallback( SetLabelText );
                this.Invoke( d, new object[] { text } );
            }
            else
            {
                this.lblProgress.Text = text;
            }
            //Application.DoEvents();

        }

        private void FindAndImportImages()
        {
            try
            {
                NodeSorter ns = new NodeSorter();
                this.m_ActivityNodes.Sort(ns.Compare);

                DateTime FirstStart = new DateTime();
                DateTime LastEnd = new DateTime();
                IActivity FirstAct, LastAct;

                if (this.m_ActivityNodes[0].Tag is IActivity)
                {
                    FirstAct = (IActivity)(this.m_ActivityNodes[0].Tag);
                    FirstStart = FirstAct.StartTime.ToLocalTime();
                }

                if (this.m_ActivityNodes[this.m_ActivityNodes.Count - 1].Tag is IActivity)
                {
                    LastAct = (IActivity)(this.m_ActivityNodes[this.m_ActivityNodes.Count - 1].Tag);
                    LastEnd = GetActivityEndTime(LastAct);
                }

                IActivity CurrentActivity;
                int CurrentIndex = 0;
                CurrentActivity = (IActivity)(this.m_ActivityNodes[0].Tag);

                this.progressBar2.Style = ProgressBarStyle.Continuous;
                this.progressBar2.Minimum = 0;
                this.progressBar2.Maximum = 100;
                this.progressBar2.Value = 0;
                this.timerProgressBar.Enabled = false;
                int i = 0;
                DateTime FileTime = new DateTime();
                foreach (FileInfo file in m_files)
                {
                    Application.DoEvents();

                    i++;
                    this.progressBar2.Value = (int)(100 * (double)(i) / (double)(m_files.Count));
                    this.lblProgress.Text = Resources.Resources.ImportControl_searchingActivity + " " + file.FullName;

                    FileTime = Functions.GetFileTime(file.FullName);

                    //{ //A valid EXIF metadata has been found                
                    if ((FileTime > FirstStart) &
                        (FileTime < LastEnd))
                    {//dateTime im picture is within the range of all activities

                        if (FileTime > CurrentActivity.StartTime.ToLocalTime())
                        {
                            if (FileTime > GetActivityEndTime(CurrentActivity))
                            {
                                //picture has been taken later => cycle to next activity
                                while (CurrentIndex < this.m_ActivityNodes.Count)
                                {
                                    CurrentIndex++;
                                    CurrentActivity = (IActivity)(this.m_ActivityNodes[CurrentIndex].Tag);
                                    if (FileTime < GetActivityEndTime(CurrentActivity)) break;
                                }
                                if (!(CurrentIndex < this.m_ActivityNodes.Count)) //cycled through all activities, no match found
                                {
                                    break;//break foreach file loop
                                }
                            }

                            if (FileTime > CurrentActivity.StartTime.ToLocalTime())
                            {
                                //the picture has been taken during the activity
                                PluginData data = Helper.Functions.ReadExtensionData(CurrentActivity);

                                //Check if Image does already exist in the current activity
                                if (!ImageAlreadyExistsInActivity(file.Name, data))
                                {
                                    this.m_ActivityNodes[CurrentIndex].BackColor = Color.Yellow;
                                    this.m_ActivityNodes[CurrentIndex].Parent.BackColor = Color.Yellow;
                                    this.m_ActivityNodes[CurrentIndex].Parent.Parent.BackColor = Color.Yellow; //activity node plus parents will be marked yellow to track acts to which images have been added
                                    ImageDataSerializable ids = GetImageDataSerializableFromFile(file.FullName);
                                    if (ids != null) data.Images.Add(ids);
                                    Functions.WriteExtensionData(CurrentActivity, data);
                                    m_numFilesImported++;

                                    using (ImageData ID = new ImageData(ids))
                                    {
                                        ActivityPicturePlugin.Source.Settings.NewThumbnailsCreated += ID.ThumbnailPath + "\t";
                                    }

                                    continue; //proceed to next file
                                }
                            }
                        }
                    }
                }

                this.progressBar2.Value = this.progressBar2.Maximum;
                this.timerProgressBar.Enabled = true;
                this.lblProgress.Text = String.Format(Resources.Resources.ImportControl_scanDone,
                    m_numFilesImported);

            }
            catch (Exception)
            {
                //throw;
            }
        }

        private static DateTime GetActivityEndTime( IActivity Act )
        {
            ActivityInfo info = ActivityInfoCache.Instance.GetInfo(Act);
            return info.EndTime.ToLocalTime();
        }

        private void ThreadGetImages()
        {
            foreach ( TreeNode n in this.m_SelectedNodes )
            {
                GetImageFiles( n );
            }
            BeginInvoke( onImagesComplete, new object[] { this, EventArgs.Empty } );
        }

        private void GetImageFiles( TreeNode node ) //finds all images of the selected directory
        {
            try
            {
                if ( node.Tag is DirectoryInfo )
                {
                    DirectoryInfo dir = (DirectoryInfo)( node.Tag );

                    //Check if directory has been expanded before
                    if ( node.Nodes.Count != 0 )
                    {
                        if ( node.Nodes[0].Text == gDummyFolder )
                        {
                            //node is checked and has not been expanded before!
                            //==> check all paths below
                            DirectoryInfo[] dirSubs = dir.GetDirectories();
                            foreach ( DirectoryInfo dirsub in dirSubs )
                            {
                                TreeNode subNode = new TreeNode( dirsub.Name );
                                subNode.Nodes.Add( gDummyFolder );
                                subNode.Tag = dirsub;
                                GetImageFiles( subNode );
                            }
                        }
                    }

                    FileInfo[] dirfiles = dir.GetFiles();
                    foreach ( FileInfo file in dirfiles )
                    {
                        if ( Functions.GetMediaType( file.Extension ) != ImageData.DataTypes.Nothing )
                        {
                            if ( !m_files.Contains( file ) )
                            {
                                //TODO: Filter when importing
                                m_files.Add( file );
                                SetLabelText( Resources.Resources.ImportControl_addingFile + " " + file.Name );
                                Application.DoEvents();
                            }

                        }
                    }
                }
            }
            catch ( UnauthorizedAccessException ex )
            {
                //Nothing to do
                Console.WriteLine( ex.Message );
            }
            catch ( Exception )
            {
                throw;
            }
        }

        private void SortFiles()
        {
            //start sorting
            Application.DoEvents();
            FileExSorter fs = new FileExSorter();
            List<FileInfoEx> fiexs = new List<FileInfoEx>();
            string strx = "";
            int j = 0;
            progressBar2.Style = ProgressBarStyle.Continuous;
            progressBar2.Maximum = m_files.Count;
            progressBar2.BringToFront();
            timerProgressBar.Enabled = false;

            // Get exif data for all files once and only one (slow operation)
            foreach ( FileInfo fi in m_files )
            {
                lblProgress.Text = string.Format( Resources.Resources.SortingXofYImages, ++j, m_files.Count );
                progressBar2.Value = j;
                strx = "9999";
                if (Functions.IsExifFileExt(fi))
                {
                    try
                    {
                        ExifDirectory ex = SimpleRun.ShowOneFileExifDirectory(fi.FullName);
                        if (ex != null)
                        {
                            strx = ex.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                FileInfoEx fiex = new FileInfoEx();
                fiex.fi = fi;
                fiex.strDateTime = strx;
                fiexs.Add( fiex );

                Application.DoEvents();
            }

            this.progressBar2.Value = this.progressBar2.Maximum;
            timerProgressBar.Enabled = true;

            // Sort them
            fiexs.Sort( fs.Compare );

            // Reassign the files to the original m_files member.
            for ( int i = 0; i < m_files.Count; i++ )
                m_files[i] = fiexs[i].fi;

            fiexs.Clear();
        }

        private ListViewItemSorter GetListViewSorter( int columnIndex, ListView lv )
        {
            ListViewItemSorter sorter = (ListViewItemSorter)lv.ListViewItemSorter;
            if ( sorter == null )
                sorter = new ListViewItemSorter();

            sorter.ColumnIndex = columnIndex;

            string columnName = lv.Columns[columnIndex].Tag as string;
            switch ( columnName )
            {
                case "colDateTime":
                    sorter.ColumnType = ListViewItemSorter.ColumnDataType.DateTime;
                    break;
                case "colImage":
                case "colGPS":
                case "colTitle":
                case "colDescription":
                    sorter.ColumnType = ListViewItemSorter.ColumnDataType.String;
                    break;
                /*case "NUMERIC":
                    sorter.ColumnType = ListViewItemSorter.ColumnDataType.Decimal;
                    break;*/
                default:
                    sorter.ColumnType = ListViewItemSorter.ColumnDataType.String;
                    break;
            }

            if ( sorter.SortDirection == SortOrder.Ascending )
                sorter.SortDirection = SortOrder.Descending;
            else
                sorter.SortDirection = SortOrder.Ascending;

            return sorter;
        }

        #endregion

        #region Eventhandlers
        private void OnImagesComplete( object sender, EventArgs e )
        {
            //this.progressBar2.Style = ProgressBarStyle.Marquee;
            //SetLabelText("Sorting files");
            ////this.progressBar2.Style = ProgressBarStyle.Blocks;
            ////this.progressBar2.Maximum = 100;
            ////this.progressBar2.Value = 100;
            this.lblProgress.Text = String.Format( Resources.Resources.SortingXImages, m_files.Count );
        }

        #region treeViewImages
        private void treeViewImages_BeforeExpand( object sender, TreeViewCancelEventArgs e )
        {
            if ( !m_notfireevent ) //prevent recursive firing of events
            {
                TreeView tvw = (TreeView)( sender );
                TreeNode currentNode = e.Node;

                //Check if the node has been expanded before,
                // if not, add new nodes
                if ( e.Node.Nodes.Count > 0 )
                {
                    if ( e.Node.Nodes[0].Text == gDummyFolder )
                    {
                        e.Node.Nodes.Clear();
                        GetSubDirectoryNodes( e.Node );
                    }
                }
            }
        }
        private void treeViewImages_AfterCheck( object sender, TreeViewEventArgs e )
        {
            if ( !m_notfireevent ) SetCheck( e.Node, e.Node.Checked );
        }
        private void treeViewImages_AfterSelect( object sender, TreeViewEventArgs e )
        {
            ResetProgressBar();
            //lblProgress.Text = "";

            if ( e.Node.Tag is DirectoryInfo )
            {
                m_DriveDir = new DirectoryInfo( e.Node.FullPath );
                //Thread td1 = new Thread(new ThreadStart(ShowFolderPics));
                //td1.Start();
                //td1.Join();

                ShowFolderPics();
                if ( e.Node.Nodes.Count > 0 )
                {
                    if ( e.Node.Nodes[0].Text == gDummyFolder )
                    {
                        e.Node.Nodes.Clear();
                        GetSubDirectoryNodes( e.Node );
                    }
                }
            }
        }

        private void treeViewImages_EnabledChanged( object sender, EventArgs e )
        {
            //if ( this.Enabled )
            if ( treeViewImages.Enabled )
                treeViewImages.BackColor = Plugin.GetApplication().VisualTheme.Window;
            else
                treeViewImages.BackColor = Plugin.GetApplication().VisualTheme.Control;	// this.BackColor;
        }

        private void treeViewImages_MouseClick( object sender, MouseEventArgs e )
        {
            // Left mouse button already selects properly.
            // Need to make this distinction here due to issues with 
            // expanding nodes with left button, autoscroll, and GetNodeAt.
            if ( e.Button == System.Windows.Forms.MouseButtons.Right )
                treeViewImages.SelectedNode = treeViewImages.GetNodeAt( e.Location );
        }
        #endregion

        #region treeViewActivities
        private void treeViewActivities_AfterSelect( object sender, TreeViewEventArgs e )
        {
            //lblProgress.Text = "";
            ResetProgressBar();
            if ( e.Node.Tag != null )
            {
                if ( e.Node.Tag is IActivity ) //Activity is selected
                {
                    AddImagesToListViewAct( e.Node, true );
                    return;
                }
            }
            //if no activity is selected
            this.listViewAct.Items.Clear();
        }

        private void treeViewActivities_EnabledChanged( object sender, EventArgs e )
        {
            //if ( this.Enabled )
            if ( treeViewActivities.Enabled )
                treeViewActivities.BackColor = Plugin.GetApplication().VisualTheme.Window;
            else
                treeViewActivities.BackColor = Plugin.GetApplication().VisualTheme.Control;	// this.BackColor;
            //treeViewActivities.BackColor = this.BackColor;
        }

        private void treeViewActivities_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
        {
            // Left mouse button already selects properly.
            // Need to make this distinction here due to issues with
            // expanding nodes with left button, autoscroll, and GetNodeAt.
            if ( e.Button == System.Windows.Forms.MouseButtons.Right )
                treeViewActivities.SelectedNode = treeViewActivities.GetNodeAt( e.Location );
        }
        #endregion

        #region listViewDrive
        private void listViewDrive_KeyDown( object sender, KeyEventArgs e )
        {
            if ( e.Control )
            {
                switch ( e.KeyCode )
                {
                    case Keys.A:
                        foreach ( ListViewItem l in listViewDrive.Items )
                        {
                            l.Selected = true;
                        }
                        break;
                    case Keys.C:
                        break;
                }
            }
        }

        private void listViewDrive_ItemDrag( object sender, ItemDragEventArgs e )
        {
            ListViewItem[] l = new ListViewItem[this.listViewDrive.SelectedItems.Count];
            for ( int i = 0; i < this.listViewDrive.SelectedItems.Count; i++ )
            {
                l[i] = this.listViewDrive.SelectedItems[i];
            }
            DoDragDrop( l, DragDropEffects.Copy );
        }

        private void listViewDrive_DoubleClick( object sender, EventArgs e )
        {
            try
            {
                string file = (string)( listViewDrive.FocusedItem.Tag );
                ImageData.DataTypes dt = Functions.GetMediaType( file );
                if ( dt == ImageData.DataTypes.Image )
                {
                    Functions.OpenImage( file, "" );
                }
                else if ( dt == ImageData.DataTypes.Video )
                {
                    Functions.OpenVideoInExternalWindow( file );
                }
                return;
            }
            catch ( Exception )
            {
                //throw;
            }
        }

        private void listViewDrive_ColumnClick( object sender, ColumnClickEventArgs e )
        {
            ListViewItemSorter sorter = GetListViewSorter( e.Column, listViewDrive );

            listViewDrive.ListViewItemSorter = sorter;
            listViewDrive.Sort();
        }

        #endregion

        #region listViewAct
        private void listViewAct_DragEnter( object sender, DragEventArgs e )
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void listViewAct_DragDrop( object sender, DragEventArgs e )
        {
            try
            {
                if ( e.Data.GetDataPresent( "System.Windows.Forms.ListViewItem[]", false ) )
                {
                    ListViewItem[] l = (ListViewItem[])( e.Data.GetData( "System.Windows.Forms.ListViewItem[]" ) );
                    AddSelectedImagesToActivity( l );
                }
            }
            catch ( Exception )
            {
                throw;
            }
        }

        private void listViewAct_KeyDown( object sender, KeyEventArgs e )
        {
            switch ( e.KeyCode )
            {
                case Keys.Delete:
                case Keys.Back:
                    if ( MessageBox.Show( Resources.Resources.ConfirmDeleteLong_Text, Resources.Resources.ConfirmDeleteShort_Text,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation ) == DialogResult.Yes )
                    {
                        //Delete selected images
                        ListView.SelectedListViewItemCollection sel = listViewAct.SelectedItems;
                        ListViewItem[] lvis = new ListViewItem[sel.Count];
                        sel.CopyTo( (Array)lvis, 0 );
                        RemoveSelectedImagesFromActivity( lvis );

                        lblProgress.Text = String.Format( Resources.Resources.FoundImagesInActivity_Text, this.listViewAct.Items.Count );
                    }
                    break;
                case Keys.A:
                    foreach ( ListViewItem l in listViewAct.Items )
                    {
                        l.Selected = true;
                    }
                    break;
                case Keys.V:
                    if ( e.Control )
                    {
                        ListViewItem[] lvisel = new ListViewItem[this.listViewDrive.SelectedItems.Count];
                        this.listViewDrive.SelectedItems.CopyTo( lvisel, 0 );
                        AddSelectedImagesToActivity( lvisel );
                    }
                    break;
            }
        }

        private void listViewAct_DoubleClick( object sender, EventArgs e )
        {
            try
            {
                string id = (string)( listViewAct.FocusedItem.Tag );
                string photosource = "";
                IActivity act = (IActivity)( this.treeViewActivities.SelectedNode.Tag );
                PluginData data = Helper.Functions.ReadExtensionData( act );

                foreach ( ImageDataSerializable ids in data.Images )
                {
                    if ( ids.ReferenceID == id )
                    {
                        photosource = ids.PhotoSource;
                        if ( ids.Type == ImageData.DataTypes.Image )
                        {
                            Functions.OpenImage( photosource, id );
                        }
                        else if ( ids.Type == ImageData.DataTypes.Video )
                        {
                            Functions.OpenVideoInExternalWindow( photosource );
                        }
                        return;
                    }
                }
            }
            catch ( Exception )
            {
                //throw;
            }

        }

        private void listViewAct_ColumnClick( object sender, ColumnClickEventArgs e )
        {
            ListViewItemSorter sorter = GetListViewSorter( e.Column, listViewAct );

            listViewAct.ListViewItemSorter = sorter;
            listViewAct.Sort();
        }

        #endregion

        private void btnExpandAll_Click( object sender, EventArgs e )
        {
            this.treeViewActivities.ExpandAll();
        }

        private void btnCollapsAll_Click( object sender, EventArgs e )
        {
            this.treeViewActivities.CollapseAll();
        }

        private void ImportControl_Load( object sender, EventArgs e )
        {
            this.Size = this.Parent.Size;
        }

        private void btnScan_Click( object sender, EventArgs e )
        {
            this.UseWaitCursor = true;

            EnableControl( false );
            this.progressBar2.Style = ProgressBarStyle.Marquee;
            Application.DoEvents();
            m_files.Clear();

            ThreadGetImages(); //collects images of all selected paths
            SortFiles();
            FindAndImportImages();

            AddImagesToListViewAct( this.treeViewActivities.SelectedNode, true );
            EnableControl( true );

            this.UseWaitCursor = false;

            this.lblProgress.Text = String.Format( Resources.Resources.ImportControl_scanDone, m_numFilesImported );
        }

        private void btnChangeFolderView_Click( object sender, EventArgs e )
        {
            if ( m_viewFolder == 4 ) m_viewFolder = 0;
            else m_viewFolder++;
            this.listViewDrive.View = (View)( m_viewFolder );
            ActivityPicturePlugin.Source.Settings.FolderView = m_viewFolder;
            this.listViewDrive.Invalidate();
        }

        private void btnChangeActivityView_Click( object sender, EventArgs e )
        {
            if ( m_viewActivity == 4 ) m_viewActivity = 0;
            else m_viewActivity++;
            this.listViewAct.View = (View)( m_viewActivity );
            ActivityPicturePlugin.Source.Settings.ActivityView = m_viewActivity;
            this.listViewAct.Invalidate();
        }

        private void timerProgressBar_Tick( object sender, EventArgs e )
        {
            if ( this.progressBar2.Value == this.progressBar2.Maximum )
                ResetProgressBar();
        }

        private void toolStripMenuRemove_Click( object sender, EventArgs e )
        {
            if ( MessageBox.Show(Resources.Resources.ConfirmDeleteLong_Text , Resources.Resources.ConfirmDeleteShort_Text,
                 MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation ) == DialogResult.Yes )
            {
                ListView.SelectedListViewItemCollection sel = listViewAct.SelectedItems;
                ListViewItem[] lvis = new ListViewItem[sel.Count];
                sel.CopyTo( (Array)lvis, 0 );
                RemoveSelectedImagesFromActivity( lvis );
                lblProgress.Text = String.Format( Resources.Resources.FoundImagesInActivity_Text, this.listViewAct.Items.Count );
            }
        }

        private void toolStripMenuAdd_Click( object sender, EventArgs e )
        {
            ListView.SelectedListViewItemCollection sel = listViewDrive.SelectedItems;
            ListViewItem[] lvis = new ListViewItem[sel.Count];
            sel.CopyTo( (Array)lvis, 0 );
            AddSelectedImagesToActivity( lvis );
            lblProgress.Text = String.Format( Resources.Resources.FoundImagesInActivity_Text, this.listViewAct.Items.Count );
        }

        private void contextMenuListViewDrive_Opening( object sender, CancelEventArgs e )
        {
            if ( listViewDrive.SelectedItems.Count == 0 )
                e.Cancel = true;

        }

        private void contextMenuListViewAct_Opening( object sender, CancelEventArgs e )
        {
            if ( listViewAct.SelectedItems.Count == 0 )
                e.Cancel = true;
        }

        private void toolStripMenuRefresh_Click( object sender, EventArgs e )
        {
            TreeNode selNode = treeViewImages.SelectedNode;
            selNode.Nodes.Clear();
            GetSubDirectoryNodes( selNode );
        }

        private void splitContainer2_MouseDoubleClick( object sender, MouseEventArgs e )
        {
            if ( e.Button == System.Windows.Forms.MouseButtons.Left )
            {
                if ( this.splitContainer2.SplitterRectangle.Contains( e.X, e.Y ) )
                    this.splitContainer2.SplitterDistance = this.splitContainer3.SplitterDistance;
            }
        }

        private void splitContainer3_MouseDoubleClick( object sender, MouseEventArgs e )
        {
            if ( e.Button == System.Windows.Forms.MouseButtons.Left )
            {
                if ( this.splitContainer3.SplitterRectangle.Contains( e.X, e.Y ) )
                    this.splitContainer3.SplitterDistance = this.splitContainer2.SplitterDistance;
            }
        }

        private void toolStripMenuCopyToClipboard_Click( object sender, EventArgs e )
        {
            TreeNode selNode = this.treeViewActivities.SelectedNode;
            if ( selNode != null )
            {
                List<string> sFileInfos = new List<string>();
                string sClipboard = treeViewActivities_GetImageDataFromNodeChildren( selNode, sFileInfos );
                sClipboard += "\r\n";
                foreach ( string s in sFileInfos )
                    sClipboard += s + "\r\n";
                Clipboard.SetText( sClipboard );
            }
        }

        private void toolStripMenuOpenFolder_Click( object sender, EventArgs e )
        {
            try
            {
                IActivity act = (IActivity)( this.treeViewActivities.SelectedNode.Tag );
                PluginData data = Helper.Functions.ReadExtensionData( act );

                List<string> sFolders = new List<string>();
                foreach ( ListViewItem lvi in listViewAct.SelectedItems )
                {
                    string id = (string)( lvi.Tag );
                    foreach ( ImageDataSerializable ids in data.Images )
                    {
                        if ( ids.ReferenceID == id )
                        {
                            System.IO.FileInfo fi = new FileInfo( ids.PhotoSource );

                            if ( !sFolders.Contains( fi.DirectoryName ) )
                                sFolders.Add( fi.DirectoryName );
                        }
                    }
                }

                foreach ( string sFolder in sFolders )
                    Functions.OpenExternal( sFolder );

            }
            catch ( Exception )
            { }
        }

        #endregion
    }

    #region IComparer Implementations

    public class ListViewItemSorter : System.Collections.IComparer
    {
        public enum ColumnDataType
        {
            Decimal = 0,
            String,
            DateTime
        };

        private int columnIndex = 0;
        public int ColumnIndex
        {
            get { return ColumnIndex; }
            set { columnIndex = value; }
        }
        private ColumnDataType columnType;
        public ColumnDataType ColumnType
        {
            get { return columnType; }
            set { columnType = value; }
        }
        private SortOrder sortDirection = SortOrder.None;
        public SortOrder SortDirection
        {
            get { return sortDirection; }
            set { sortDirection = value; }
        }

        public int Compare( object x, object y )
        {
            ListViewItem tx = x as ListViewItem;
            ListViewItem ty = y as ListViewItem;
            string strx = ""; ;
            string stry = "";
            double dblx = double.MinValue;
            double dbly = double.MinValue;

            if ( tx.SubItems.Count > columnIndex ) strx = tx.SubItems[columnIndex].Text;
            if ( ty.SubItems.Count > columnIndex ) stry = ty.SubItems[columnIndex].Text;

            int iResult = 0;

            try
            {
                switch ( columnType )
                {
                    case ColumnDataType.DateTime:
                        DateTime datex = DateTime.MinValue;
                        DateTime datey = DateTime.MinValue;
                        if ( strx != "" ) datex = DateTime.Parse( strx );
                        if ( stry != "" ) datey = DateTime.Parse( stry );
                        iResult = DateTime.Compare( datex, datey );
                        break;
                    case ColumnDataType.Decimal:
                        if ( strx != "" ) dblx = Double.Parse( strx );
                        if ( stry != "" ) dbly = Double.Parse( stry );
                        if ( dblx < dbly ) iResult = -1;
                        else if ( dblx > dbly ) iResult = 1;
                        else iResult = 0;
                        break;
                    case ColumnDataType.String:
                        iResult = string.Compare( strx, stry );
                        break;
                    default:
                        break;
                }
            }
            catch ( Exception )
            { }

            if ( sortDirection == SortOrder.None ) return 0;
            else if ( sortDirection == SortOrder.Descending ) return -iResult;
            else return iResult;

        }
    }

    public class NodeSorter : System.Collections.IComparer
    {
        public int Compare( object x, object y )
        {
            TreeNode tx = x as TreeNode;
            TreeNode ty = y as TreeNode;
            string strx, stry;

            if ( tx.Tag is ImageData ) strx = ( (ImageData)( tx.Tag ) ).PhotoSourceFileName;
            else strx = tx.Name;

            if ( ty.Tag is ImageData ) stry = ( (ImageData)( ty.Tag ) ).PhotoSourceFileName;
            else stry = ty.Name;

            return string.Compare( strx, stry );
        }
    }

    public class FileSorter : System.Collections.IComparer
    {
        public int Compare( object x, object y )
        {
            try
            {
                FileInfo tx = x as FileInfo;
                FileInfo ty = y as FileInfo;
                string strx = "9999";
                string stry = "9999";
                if (Functions.IsExifFileExt(tx))
                {
                    try
                    {
                        ExifDirectory ex = SimpleRun.ShowOneFileExifDirectory(tx.FullName);
                        if (ex != null)
                        {
                            strx = ex.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                if (Functions.IsExifFileExt(ty))
                {
                    try
                    {
                        ExifDirectory ey = SimpleRun.ShowOneFileExifDirectory(ty.FullName);
                        if (ey != null)
                        {
                            stry = ey.GetDescription(ExifDirectory.TAG_DATETIME_ORIGINAL);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                return string.Compare( strx, stry );
            }
            catch ( Exception )
            {
                return 0;
                //throw;
            }

        }
    }

    public class FileExSorter : System.Collections.IComparer
    {
        public int Compare( object x, object y )
        {
            ImportControl.FileInfoEx tx = x as ImportControl.FileInfoEx;
            ImportControl.FileInfoEx ty = y as ImportControl.FileInfoEx;
            return string.Compare( tx.strDateTime, ty.strDateTime );
        }
    }

    #endregion

}