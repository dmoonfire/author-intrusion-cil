﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

namespace AuthorIntrusion.Common.Blocks
{
#if REMOVED
	/// <summary>
	/// The block type supervisor is a manager class responsible for maintaining
	/// the relationship between the various blocks based on their types.
	/// </summary>
	public class BlockStructureSupervisor
	{
		#region Properties

		/// <summary>
		/// Gets or sets the root block structure for the entire project. Typically
		/// the root block structure is the top-most element of a project, either a book
		/// or a story.
		/// </summary>
		public BlockStructure RootBlockStructure
		{
			get { return rootBlockStructure; }
			set
			{
				if (value == null)
				{
					throw new NullReferenceException(
						"Cannot assign a null to the RootBlockStructure.");
				}

				rootBlockStructure = value;
			}
		}

		protected Project Project { get; private set; }

		#endregion

		#region Methods

		/// <summary>
		/// Updates the blocks in the project and assign them to block types that fit
		/// the document structure. This will attempt to keep the current block type
		/// of a given block if it can fit into the structure.
		/// </summary>
		public void Update()
		{
			// If we are inside interactive processing, we need to skip updates
			// since this can be an expensive operation.
			if (Project.ProcessingState == ProjectProcessingState.Batch)
			{
				return;
			}

			// We need to get a write lock on the block since we'll be making changes
			// to all the blocks and their relationships. This will, in effect, also
			// ensure no block is being modified.
			using (Project.Blocks.AcquireLock(RequestLock.Write))
			{
				// Go through all the blocks in the list.
				ProjectBlockCollection blocks = Project.Blocks;

				for (int blockIndex = 0;
					blockIndex < Project.Blocks.Count;
					blockIndex++)
				{
					// Grab the block we're currently looking at.
					Block block = blocks[blockIndex];
					BlockType blockType = block.BlockType;

					// To figure out the block type, we go backwards until we find the
					// first block's structure that contains this block type. If we
					// can't find one, we will just use the first block in the list
					// regardless of type.
					BlockStructure newBlockStructure = null;
					Block newParentBlock = null;

					for (int searchIndex = blockIndex - 1;
						searchIndex >= 0;
						searchIndex--)
					{
						// Grab this block and structure.
						Block searchBlock = blocks[searchIndex];
						BlockStructure searchStructure = searchBlock.BlockStructure;

						// If the search structure includes the current block type,
						// then we'll use that and stop looking through the rest of the list.
						if (searchStructure.ContainsChildStructure(blockType))
						{
							newBlockStructure = searchStructure.GetChildStructure(blockType);
							newParentBlock = searchBlock;
							break;
						}
					}

					// Look to see if we assigned the parent block and structure. If we
					// haven't, then assign the parent to the first one (obvious not if
					// we are modifying the first one).
					if (newParentBlock == null
						&& blockIndex > 0)
					{
						newParentBlock = Project.Blocks[0];
					}

					if (newBlockStructure == null)
					{
						newBlockStructure = RootBlockStructure;
					}

					// Assign the new block structure and parent.
					block.SetParentBlock(newParentBlock);
					//block.SetBlockStructure(newBlockStructure);
				}
			}
		}

		#endregion

		#region Constructors

		public BlockStructureSupervisor(Project project)
		{
			// Save the members we need for later referencing.
			Project = project;

			// Set up the default structure which is just one or more paragraphs
			// and no structural elements.
			rootBlockStructure = new BlockStructure
			{
				BlockType = project.BlockTypes.Paragraph
			};
		}

		#endregion

		#region Fields

		private BlockStructure rootBlockStructure;

		#endregion
	}
#endif
}
