﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Blocks.Locking;
using AuthorIntrusion.Common.Commands;
using NUnit.Framework;

namespace AuthorIntrusion.Common.Tests
{
	[TestFixture]
	public class ReplaceTextBlockCommandTests
	{
		#region Methods

		[Test]
		public void TestCommand()
		{
			// Arrange
			var project = new Project();
			var context = new BlockCommandContext(project);
			ProjectBlockCollection blocks = project.Blocks;
			Block block = blocks[0];
			using (block.AcquireBlockLock(RequestLock.Write))
			{
				block.SetText("abcd");
			}
			int blockVersion = block.Version;
			BlockKey blockKey = block.BlockKey;

			// Act
			var command = new ReplaceTextCommand(
				new BlockPosition(blockKey, 2), 1, "YES");
			project.Commands.Do(command, context);

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[0], 5), project.Commands.LastPosition);

			const int index = 0;
			Assert.AreEqual("abYESd", blocks[index].Text);
			Assert.AreEqual(blockVersion + 2, blocks[index].Version);
		}

		[Test]
		public void TestUndoCommand()
		{
			// Arrange
			var project = new Project();
			var context = new BlockCommandContext(project);
			ProjectBlockCollection blocks = project.Blocks;
			Block block = blocks[0];
			using (block.AcquireBlockLock(RequestLock.Write))
			{
				block.SetText("abcd");
			}
			int blockVersion = block.Version;
			BlockKey blockKey = block.BlockKey;

			var command = new ReplaceTextCommand(
				new BlockPosition(blockKey, 2), 1, "YES");
			project.Commands.Do(command, context);

			// Act
			project.Commands.Undo(context);

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[0], 3), project.Commands.LastPosition);

			const int index = 0;
			Assert.AreEqual("abcd", blocks[index].Text);
			Assert.AreEqual(blockVersion + 4, blocks[index].Version);
		}

		[Test]
		public void TestUndoRedoCommand()
		{
			// Arrange
			var project = new Project();
			var context = new BlockCommandContext(project);
			ProjectBlockCollection blocks = project.Blocks;
			Block block = blocks[0];
			using (block.AcquireBlockLock(RequestLock.Write))
			{
				block.SetText("abcd");
			}
			int blockVersion = block.Version;
			BlockKey blockKey = block.BlockKey;

			var command = new ReplaceTextCommand(
				new BlockPosition(blockKey, 2), 1, "YES");
			project.Commands.Do(command, context);
			project.Commands.Undo(context);

			// Act
			project.Commands.Redo(context);

			// Assert
			Assert.AreEqual(1, blocks.Count);
			Assert.AreEqual(
				new BlockPosition(blocks[0], 5), project.Commands.LastPosition);

			const int index = 0;
			Assert.AreEqual("abYESd", blocks[index].Text);
			Assert.AreEqual(blockVersion + 6, blocks[index].Version);
		}

		#endregion
	}
}
