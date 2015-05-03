﻿// <copyright file="LoadInternalSingleRegionTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Buffers;
using AuthorIntrusion.IO;

using MfGames.HierarchicalPaths;

using Xunit;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Tests the loading of a single buffer with a single Internal region.
	/// </summary>
	public class LoadInternalSingleRegionTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies the state of the project.
		/// </summary>
		[Fact]
		public void VerifyProjectBuffer()
		{
			// Prepare the test.
			Project project = Setup();
			Region region1 = project.Regions["region-1"];

			Assert.Equal(
				1,
				project.Blocks.Count);
			Assert.Equal(
				BlockType.Region,
				project.Blocks[0].BlockType);
			Assert.Equal(
				region1,
				project.Blocks[0].LinkedRegion);
		}

		/// <summary>
		/// Verifies the state of region-1.
		/// </summary>
		[Fact]
		public void VerifyRegion1()
		{
			Project project = Setup();
			Region region1 = project.Regions["region-1"];

			Assert.Equal(
				1,
				region1.Blocks.Count);
			Assert.Equal(
				"Text in region 1.",
				region1.Blocks[0].Text);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets up the unit test.
		/// </summary>
		/// <returns>
		/// The loaded project.
		/// </returns>
		private Project Setup()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"# Region 1",
				string.Empty,
				"Text in region 1.");

			// Set up the layout.
			var projectLayout = new RegionLayout
			{
				Name = "Project",
				Slug = "project",
				HasContent = false
			};
			projectLayout.Add(
				new RegionLayout
				{
					Name = "Region 1",
					Slug = "region-1",
					HasContent = true
				});

			// Create a new project with the given layout.
			var project = new Project();
			project.ApplyLayout(projectLayout);

			// Create the format.
			var format = new MarkdownBufferFormat();

			// Parse the buffer lines.
			var context = new BufferLoadContext(
				project,
				persistence);

			format.LoadProject(context);

			// Return the project.
			return project;
		}

		#endregion
	}
}
