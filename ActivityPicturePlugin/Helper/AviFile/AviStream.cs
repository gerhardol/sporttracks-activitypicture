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

namespace AviFile
{
	public abstract class AviStream
	{
		protected int aviFile;
		protected IntPtr aviStream;
		protected IntPtr compressedStream;
		protected bool writeCompressed;

        /// <summary>Pointer to the unmanaged AVI file</summary>
        internal int FilePointer {
            get { return aviFile; }
        }

        /// <summary>Pointer to the unmanaged AVI Stream</summary>
        internal virtual IntPtr StreamPointer {
            get { return aviStream; }
        }

        /// <summary>Flag: The stream is compressed/uncompressed</summary>
        internal bool WriteCompressed {
            get { return writeCompressed; }
        }

        /// <summary>Close the stream</summary>
        public virtual void Close(){
			if(writeCompressed){
				Avi.AVIStreamRelease(compressedStream);
			}
			Avi.AVIStreamRelease(StreamPointer);
		}

        /// <summary>Export the stream into a new file</summary>
        /// <param name="fileName"></param>
		public abstract void ExportStream(String fileName);
		
	}
}
