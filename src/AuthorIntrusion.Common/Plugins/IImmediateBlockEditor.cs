﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;

namespace AuthorIntrusion.Common.Plugins
{
	/// <summary>
	/// Indicates a project plugin controller that performs immediate changes to the
	/// text while the user is editing.
	/// </summary>
	public interface IImmediateBlockEditor: IProjectPluginController
	{
		#region Methods

		/// <summary>
		/// Checks for immediate edits after the user makes a change to the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="textIndex">Index of the text.</param>
		void CheckForImmediateEdits(
			Block block,
			int textIndex);

		#endregion
	}
}
