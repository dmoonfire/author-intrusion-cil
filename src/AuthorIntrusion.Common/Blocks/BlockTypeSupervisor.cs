﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System.Collections.Generic;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// A supervisor class that manages block types for a given project.
	/// </summary>
	public class BlockTypeSupervisor
	{
		#region Properties

		/// <summary>
		/// Gets the <see cref="BlockType"/> with the specified block type name.
		/// </summary>
		/// <value>
		/// The <see cref="BlockType"/>.
		/// </value>
		/// <param name="blockTypeName">Name of the block type.</param>
		/// <returns>The associated block type.</returns>
		public BlockType this[string blockTypeName]
		{
			get { return BlockTypes[blockTypeName]; }
		}

		/// <summary>
		/// Gets the block types associated with this Supervisor.
		/// </summary>
		public IDictionary<string, BlockType> BlockTypes { get; private set; }

		public BlockType Chapter
		{
			get { return this[ChapterName]; }
		}

		/// <summary>
		/// Gets the standard block type associated with epigraphs.
		/// </summary>
		public BlockType Epigraph
		{
			get { return this[EpigraphName]; }
		}

		/// <summary>
		/// Gets the standard block type for attributions.
		/// </summary>
		public BlockType EpigraphAttribution
		{
			get { return this[EpigraphAttributionName]; }
		}

		public BlockType Paragraph
		{
			get { return this[ParagraphName]; }
		}

		/// <summary>
		/// Gets or sets the project associated with this block type Supervisor.
		/// </summary>
		public Project Project { get; private set; }

		public BlockType Scene
		{
			get { return this[SceneName]; }
		}

		#endregion

		#region Methods

		/// <summary>
		/// Adds the specified block type to the collection.
		/// </summary>
		/// <param name="blockTypeName">Name of the block type.</param>
		/// <param name="isStructural">if set to <c>true</c> [is structural].</param>
		/// <param name="isSystem">if set to <c>true</c> [is system].</param>
		public void Add(
			string blockTypeName,
			bool isStructural,
			bool isSystem = false)
		{
			var blockType = new BlockType(this)
			{
				Name = blockTypeName,
				IsStructural = isStructural,
				IsSystem = isSystem
			};

			BlockTypes.Add(blockTypeName, blockType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="BlockTypeSupervisor"/> class.
		/// </summary>
		/// <param name="project">The project.</param>
		public BlockTypeSupervisor(Project project)
		{
			// Save the project so we can associated the Supervisor with its project.
			Project = project;

			// Create the standard project block types.
			var paragraph = new BlockType(this)
			{
				Name = ParagraphName,
				IsSystem = true,
				IsStructural = false,
			};

			var story = new BlockType(this)
			{
				Name = StoryName,
				IsSystem = true,
				IsStructural = true,
			};
			var book = new BlockType(this)
			{
				Name = BookName,
				IsSystem = true,
				IsStructural = true,
			};
			var chapter = new BlockType(this)
			{
				Name = ChapterName,
				IsSystem = true,
				IsStructural = true,
			};

			var scene = new BlockType(this)
			{
				Name = SceneName,
				IsSystem = true,
				IsStructural = true,
			};

			var epigraph = new BlockType(this)
			{
				Name = EpigraphName,
				IsSystem = true,
				IsStructural = true,
			};
			var attribution = new BlockType(this)
			{
				Name = EpigraphAttributionName,
				IsSystem = true,
				IsStructural = true,
			};

			// Initialize the collection of block types.
			BlockTypes = new Dictionary<string, BlockType>();
			BlockTypes[ParagraphName] = paragraph;
			BlockTypes[StoryName] = story;
			BlockTypes[BookName] = book;
			BlockTypes[ChapterName] = chapter;
			BlockTypes[SceneName] = scene;
			BlockTypes[EpigraphName] = epigraph;
			BlockTypes[EpigraphAttributionName] = attribution;
		}

		#endregion

		#region Fields

		public const string BookName = "Book";
		public const string ChapterName = "Chapter";
		public const string EpigraphAttributionName = "Epigraph Attribution";
		public const string EpigraphName = "Epigraph";
		public const string ParagraphName = "Paragraph";
		public const string SceneName = "Scene";
		public const string StoryName = "Story";

		#endregion
	}
}
