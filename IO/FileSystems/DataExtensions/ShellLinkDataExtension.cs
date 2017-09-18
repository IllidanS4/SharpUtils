﻿/* Date: 6.9.2017, Time: 16:22 */
using System;
using System.IO;

namespace IllidanS4.SharpUtils.IO.FileSystems.DataExtensions
{
	using DataUri = DataFileSystem.DataUri;
	
	/// <summary>
	/// Resolves links represented by the "application/x-ms-shortcut" MIME type.
	/// </summary>
	public class ShellLinkDataExtension : DataExtension
	{
		public ShellLinkDataExtension() : base("application/x-ms-shortcut")
		{
			
		}
		
		protected override FileAttributes GetAttributesInternal(DataUri dataUri)
		{
			throw new NotImplementedException();
		}
		
		protected override DateTime GetCreationTimeInternal(DataUri dataUri)
		{
			throw new NotImplementedException();
		}
		
		protected override DateTime GetLastAccessTimeInternal(DataUri dataUri)
		{
			throw new NotImplementedException();
		}
		
		protected override DateTime GetLastWriteTimeInternal(DataUri dataUri)
		{
			throw new NotImplementedException();
		}
		
		protected override Uri GetTargetInternal(DataUri dataUri)
		{
			return ShellFileSystem.Instance.LoadLinkTargetUri(dataUri.Data);
		}
		
		protected override ResourceInfo GetTargetResourceInternal(DataUri dataUri)
		{
			return ShellFileSystem.Instance.LoadLinkTargetResource(dataUri.Data);
		}
	}
}
