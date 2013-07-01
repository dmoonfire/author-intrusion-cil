﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using MfGames.Commands;
using MfGames.Commands.TextEditing;

namespace AuthorIntrusion.Common.Commands
{
	/// <summary>
	/// A command to delete text from a single block.
	/// </summary>
	public class DeleteTextCommand: BlockPositionCommand
	{
		#region Properties

		public CharacterPosition End { get; set; }

		#endregion

		#region Methods

		protected override void Do(
			BlockCommandContext context,
			Block block)
		{
			// Save the previous text so we can restore it.
			previousText = block.Text;
			originalPosition = context.Position;

			// Figure out what the new text string would be.
			startIndex = BlockPosition.TextIndex.NormalizeIndex(
				block.Text, End, WordSearchDirection.Left);
			int endIndex = End.NormalizeIndex(
				block.Text, TextIndex, WordSearchDirection.Right);
			string newText = block.Text.Remove(startIndex, endIndex - startIndex);

			// Set the new text into the block. This will fire various events to
			// trigger the immediate and background processing.
			block.SetText(newText);

			// Set the position after the next text.
			if (UpdateTextPosition.HasFlag(DoTypes.Do))
			{
				context.Position = new BlockPosition(BlockKey, startIndex);
			}
		}

		protected override void Undo(
			BlockCommandContext context,
			Block block)
		{
			block.SetText(previousText);

			if (UpdateTextPosition.HasFlag(DoTypes.Undo))
			{
				context.Position = originalPosition;
			}
		}

		#endregion

		#region Constructors

		public DeleteTextCommand(
			BlockPosition begin,
			CharacterPosition end)
			: base(begin)
		{
			End = end;
		}

		public DeleteTextCommand(SingleLineTextRange range)
			: base(new TextPosition(range.Line, range.CharacterBegin))
		{
			// DREM ToTextPosition
			End = range.CharacterEnd;
		}

		#endregion

		#region Fields

		private BlockPosition? originalPosition;

		private string previousText;
		private int startIndex;

		#endregion
	}
}
