/* This class has been written by
 * Corinna John (Hannover, Germany)
 * cj@binary-universe.net
 * 
 * You may do with this code whatever you like,
 * except selling it or claiming any rights/ownership.
 * 
 * Please send me a little feedback about what you're
 * using this code for and what changes you'd like to
 * see in later versions. (And please excuse my bad english.)
 * 
 * WARNING: This is experimental code.
 * Please do not expect "Release Quality".
 * */

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AviFile {

	public class Avi{

		public static int PALETTE_SIZE = 4*256; //RGBQUAD * 256 colours

		public static readonly int streamtypeVIDEO = mmioFOURCC('v', 'i', 'd', 's');
		public static readonly int streamtypeAUDIO = mmioFOURCC('a', 'u', 'd', 's');
		public static readonly int streamtypeMIDI = mmioFOURCC('m', 'i', 'd', 's');
		public static readonly int streamtypeTEXT = mmioFOURCC('t', 'x', 't', 's');
		
		public const int OF_SHARE_DENY_WRITE = 32;
		public const int OF_READ = 0;		//Just a guess
		public const int OF_WRITE = 1;
		public const int OF_READWRITE = 2;
		public const int OF_CREATE			 = 4096;

		public const int BMP_MAGIC_COOKIE = 19778; //ascii string "BM"

		public const int AVICOMPRESSF_INTERLEAVE = 0x00000001;    // interleave
		public const int AVICOMPRESSF_DATARATE = 0x00000002;    // use a data rate
		public const int AVICOMPRESSF_KEYFRAMES = 0x00000004;    // use keyframes
		public const int AVICOMPRESSF_VALID = 0x00000008;    // has valid data
		public const int AVIIF_KEYFRAME = 0x00000010;

		public const UInt32 ICMF_CHOOSE_KEYFRAME = 0x0001;	// show KeyFrame Every box
		public const UInt32 ICMF_CHOOSE_DATARATE = 0x0002;	// show DataRate box
		public const UInt32 ICMF_CHOOSE_PREVIEW  = 0x0004;	// allow expanded preview dialog

        //macro mmioFOURCC
        public static Int32 mmioFOURCC(char ch0, char ch1, char ch2, char ch3) {
            return ((Int32)(byte)(ch0) | ((byte)(ch1) << 8) |
                ((byte)(ch2) << 16) | ((byte)(ch3) << 24));
        }

		#region structure declarations

		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct RECT{ 
			public UInt32 left; 
			public UInt32 top; 
			public UInt32 right; 
			public UInt32 bottom; 
		} 		

		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct BITMAPINFOHEADER {
			public Int32 biSize;
			public Int32 biWidth;
			public Int32 biHeight;
			public Int16 biPlanes;
			public Int16 biBitCount;
			public Int32 biCompression;
			public Int32 biSizeImage;
			public Int32 biXPelsPerMeter;
			public Int32 biYPelsPerMeter;
			public Int32 biClrUsed;
			public Int32 biClrImportant;
		}

		[StructLayout(LayoutKind.Sequential)] 
		public struct PCMWAVEFORMAT {
			public short wFormatTag;
			public short nChannels;
			public int nSamplesPerSec;
			public int nAvgBytesPerSec;
			public short nBlockAlign;
			public short wBitsPerSample;
			public short cbSize;
		}

		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct AVISTREAMINFO {
			public Int32    fccType;
			public Int32    fccHandler;
			public Int32    dwFlags;
			public Int32    dwCaps;
			public Int16    wPriority;
			public Int16    wLanguage;
			public Int32    dwScale;
			public Int32    dwRate;
			public Int32    dwStart;
			public Int32    dwLength;
			public Int32    dwInitialFrames;
			public Int32    dwSuggestedBufferSize;
			public Int32    dwQuality;
			public Int32    dwSampleSize;
			public RECT		rcFrame;
			public Int32    dwEditCount;
			public Int32    dwFormatChangeCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=64)]
			public UInt16[]    szName;
		}
		[StructLayout(LayoutKind.Sequential, Pack=1)]
		public struct BITMAPFILEHEADER{
			public Int16 bfType; //"magic cookie" - must be "BM"
			public Int32 bfSize;
			public Int16 bfReserved1;
			public Int16 bfReserved2;
			public Int32 bfOffBits;
		}

				
		[StructLayout(LayoutKind.Sequential, Pack=1)]
			public struct AVIFILEINFO{
			public Int32 dwMaxBytesPerSecond;
			public Int32 dwFlags;
			public Int32 dwCaps;
			public Int32 dwStreams;
			public Int32 dwSuggestedBufferSize;
			public Int32 dwWidth;
			public Int32 dwHeight;
			public Int32 dwScale;
			public Int32 dwRate;
			public Int32 dwLength;
			public Int32 dwEditCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst=64)]
			public char[] szFileType;
		}

		[StructLayout(LayoutKind.Sequential, Pack=1)]
			public struct AVICOMPRESSOPTIONS {
			public UInt32   fccType;
			public UInt32   fccHandler;
			public UInt32   dwKeyFrameEvery;  // only used with AVICOMRPESSF_KEYFRAMES
			public UInt32   dwQuality;
			public UInt32   dwBytesPerSecond; // only used with AVICOMPRESSF_DATARATE
			public UInt32   dwFlags;
			public IntPtr   lpFormat;
			public UInt32   cbFormat;
			public IntPtr   lpParms;
			public UInt32   cbParms;
			public UInt32   dwInterleaveEvery;
		}

		/// <summary>AviSaveV needs a pointer to a pointer to an AVICOMPRESSOPTIONS structure</summary>
		[StructLayout(LayoutKind.Sequential, Pack=1)]
			public class AVICOMPRESSOPTIONS_CLASS {
			public UInt32   fccType;
			public UInt32   fccHandler;
			public UInt32   dwKeyFrameEvery;  // only used with AVICOMRPESSF_KEYFRAMES
			public UInt32   dwQuality;
			public UInt32   dwBytesPerSecond; // only used with AVICOMPRESSF_DATARATE
			public UInt32   dwFlags;
			public IntPtr   lpFormat;
			public UInt32   cbFormat;
			public IntPtr   lpParms;
			public UInt32   cbParms;
			public UInt32   dwInterleaveEvery;

			public AVICOMPRESSOPTIONS ToStruct(){
				AVICOMPRESSOPTIONS returnVar = new AVICOMPRESSOPTIONS();
				returnVar.fccType = this.fccType;
				returnVar.fccHandler = this.fccHandler;
				returnVar.dwKeyFrameEvery = this.dwKeyFrameEvery;
				returnVar.dwQuality = this.dwQuality;
				returnVar.dwBytesPerSecond = this.dwBytesPerSecond;
				returnVar.dwFlags = this.dwFlags;
				returnVar.lpFormat = this.lpFormat;
				returnVar.cbFormat = this.cbFormat;
				returnVar.lpParms = this.lpParms;
				returnVar.cbParms = this.cbParms;
				returnVar.dwInterleaveEvery = this.dwInterleaveEvery;
				return returnVar;
			}
		}
		#endregion structure declarations

		#region method declarations
	
		//Initialize the AVI library
		[DllImport("avifil32.dll")]
        internal static extern void AVIFileInit();    //CA1401

		//Open an AVI file
		[DllImport("avifil32.dll", PreserveSig=true)]
		internal static extern int AVIFileOpen(
			ref int ppfile,
			String szFile,
			int uMode,
            int pclsidHandler ); //CA1401

		//Get a stream from an open AVI file
		[DllImport("avifil32.dll")]
		internal static extern int AVIFileGetStream(
			int pfile,
			out IntPtr ppavi,  
			int fccType,
            int lParam );    //CA1401

		//Get the start position of a stream
		[DllImport("avifil32.dll", PreserveSig=true)]
        internal static extern int AVIStreamStart( int pavi );    //CA1401

		//Get the length of a stream in frames
		[DllImport("avifil32.dll", PreserveSig=true)]
        internal static extern int AVIStreamLength( int pavi ); //CA1401

		//Get information about an open stream
		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamInfo(
			IntPtr pAVIStream,
			ref AVISTREAMINFO psi,
            int lSize ); //CA1401

		//Get a pointer to a GETFRAME object (returns 0 on error)
		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamGetFrameOpen(
			IntPtr pAVIStream,
            ref BITMAPINFOHEADER bih );  //CA1401

		//Get a pointer to a packed DIB (returns 0 on error)
		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamGetFrame(
			int pGetFrameObj,
            int lPos );  //CA1401

		//Create a new stream in an open AVI file
		[DllImport("avifil32.dll")]
		internal static extern int AVIFileCreateStream(
			int pfile,
			out IntPtr ppavi,
            ref AVISTREAMINFO ptr_streaminfo );  //CA1401

        //Create an editable stream
        [DllImport("avifil32.dll")]
        internal static extern int CreateEditableStream(
            ref IntPtr ppsEditable,
            IntPtr psSource
        );  //CA1401

        //Cut samples from an editable stream
        [DllImport("avifil32.dll")]
        internal static extern int EditStreamCut(
            IntPtr pStream,
            ref Int32 plStart,
            ref Int32 plLength,
            ref IntPtr ppResult
        );  //CA1401

        //Copy a part of an editable stream
        [DllImport("avifil32.dll")]
        internal static extern int EditStreamCopy(
            IntPtr pStream,
            ref Int32 plStart,
            ref Int32 plLength,
            ref IntPtr ppResult
        );  //CA1401

        //Paste an editable stream into another editable stream
        [DllImport("avifil32.dll")]
        internal static extern int EditStreamPaste(
            IntPtr pStream,
            ref Int32 plPos,
            ref Int32 plLength,
            IntPtr pstream,
            Int32 lStart,
            Int32 lLength
        );  //CA1401

        //Change a stream's header values
        [DllImport("avifil32.dll")]
        internal static extern int EditStreamSetInfo(
            IntPtr pStream,
            ref AVISTREAMINFO lpInfo,
            Int32 cbInfo
        );  //CA1401

        [DllImport("avifil32.dll")]
        internal static extern int AVIMakeFileFromStreams(
            ref IntPtr ppfile,
            int nStreams,
            ref IntPtr papStreams
        );  //CA1401

        //Set the format for a new stream
		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamSetFormat(
			IntPtr aviStream, Int32 lPos,
            ref BITMAPINFOHEADER lpFormat, Int32 cbFormat ); //CA1401
		
		//Set the format for a new stream
		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamSetFormat(
			IntPtr aviStream, Int32 lPos,
            ref PCMWAVEFORMAT lpFormat, Int32 cbFormat );    //CA1401
		
		//Read the format for a stream
		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamReadFormat(
			IntPtr aviStream, Int32 lPos,
			ref BITMAPINFOHEADER lpFormat, ref Int32 cbFormat
            );  //CA1401

		//Read the size of the format for a stream
		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamReadFormat(
			IntPtr aviStream, Int32 lPos,
			int empty, ref Int32 cbFormat
            );  //CA1401
		
		//Read the format for a stream
		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamReadFormat(
			IntPtr aviStream, Int32 lPos,
			ref PCMWAVEFORMAT lpFormat, ref Int32 cbFormat
            );  //CA1401

		//Write a sample to a stream
		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamWrite(
			IntPtr aviStream, Int32 lStart, Int32 lSamples, 
			IntPtr lpBuffer, Int32 cbBuffer, Int32 dwFlags,
            Int32 dummy1, Int32 dummy2 );    //CA1401

		//Release the GETFRAME object
		[DllImport("avifil32.dll")]
        internal static extern int AVIStreamGetFrameClose(  //CA1401
			int pGetFrameObj);

		//Release an open AVI stream
		[DllImport("avifil32.dll")]
        internal static extern int AVIStreamRelease( IntPtr aviStream );  //CA1401

		//Release an open AVI file
		[DllImport("avifil32.dll")]
		internal static extern int AVIFileRelease(int pfile);   //CA1401

		//Close the AVI library
		[DllImport("avifil32.dll")]
		internal static extern void AVIFileExit();    //CA1401

		[DllImport("avifil32.dll")]
		internal static extern int AVIMakeCompressedStream(
			out IntPtr ppsCompressed, IntPtr aviStream,
            ref AVICOMPRESSOPTIONS ao, int dummy );  //CA1401

		[DllImport("avifil32.dll")]
		internal static extern bool AVISaveOptions(
			IntPtr hwnd,
			UInt32 uiFlags,              
			Int32 nStreams,                      
			ref IntPtr ppavi,
			ref AVICOMPRESSOPTIONS_CLASS plpOptions
            );  //CA1401

		[DllImport("avifil32.dll")]
		internal static extern long AVISaveOptionsFree(
			int nStreams,
			ref AVICOMPRESSOPTIONS_CLASS plpOptions
            );  //CA1401

		[DllImport("avifil32.dll")]
		internal static extern int AVIFileInfo(
			int pfile, 
			ref AVIFILEINFO pfi,
            int lSize ); //CA1401

		[DllImport("winmm.dll", EntryPoint="mmioStringToFOURCCA")]
		internal static extern int mmioStringToFOURCC(String sz, int uFlags);   //CA1401

		[DllImport("avifil32.dll")]
		internal static extern int AVIStreamRead(
			IntPtr pavi, 
			Int32 lStart,     
			Int32 lSamples,   
			IntPtr lpBuffer, 
			Int32 cbBuffer,   
			Int32  plBytes,  
			Int32  plSamples
            );  //CA1401

		[DllImport("avifil32.dll")]
		internal static extern int AVISaveV(
			String szFile,
			Int16 empty,
			Int16 lpfnCallback,
			Int16 nStreams,
			ref IntPtr ppavi,
			ref AVICOMPRESSOPTIONS_CLASS plpOptions
            );  //CA1401

		#endregion method declarations

	}
}
