﻿// <copyright file="StoreNestedSequencedRegionsTests.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using System.Collections.Generic;

using AuthorIntrusion.Buffers;
using AuthorIntrusion.IO;

using MfGames.HierarchicalPaths;

using Xunit;

namespace AuthorIntrusion.Tests.IO.MarkdownBufferFormatTests
{
	/// <summary>
	/// Tests writing out a series of files.
	/// </summary>
	public class StoreNestedSequencedRegionsTests : MemoryPersistenceTestsBase
	{
		#region Fields

		/// <summary>
		/// Contains the output context from storing.
		/// </summary>
		private BufferStoreContext outputContext;

		/// <summary>
		/// Contains the resulting persistence results after the store.
		/// </summary>
		private MemoryPersistence outputPersistence;

		#endregion

		#region Public Methods and Operators

		/// <summary>
		/// Verifies the contents of the chapter-01.
		/// </summary>
		[Fact]
		public void VerifyChapter1()
		{
			Setup();

			List<string> lines =
				outputPersistence.GetDataLines("/chapter-01");

			AssertLines(
				lines,
				"---",
				"title: Chapter 1",
				"---",
				string.Empty,
				"1. [Scenes](chapter-01/scene-001)",
				"2. [Scenes](chapter-01/scene-002)");
		}

		/// <summary>
		/// Verifies the contents of the chapter-01/scene-001.
		/// </summary>
		[Fact]
		public void VerifyChapter1Scene1()
		{
			Setup();

			List<string> lines =
				outputPersistence.GetDataLines("/chapter-01/scene-001");

			AssertLines(
				lines,
				"Text in chapter 1, scene 1.");
		}

		/// <summary>
		/// Verifies the contents of the chapter-01/scene-002.
		/// </summary>
		[Fact]
		public void VerifyChapter1Scene2()
		{
			Setup();

			List<string> lines =
				outputPersistence.GetDataLines("/chapter-01/scene-002");

			AssertLines(
				lines,
				"Text in chapter 1, scene 2.");
		}

		/// <summary>
		/// Verifies the contents of the chapter-02.
		/// </summary>
		[Fact]
		public void VerifyChapter2()
		{
			Setup();

			List<string> lines =
				outputPersistence.GetDataLines("/chapter-02");

			AssertLines(
				lines,
				"---",
				"title: Chapter 2",
				"---",
				string.Empty,
				"1. [Scenes](chapter-02/scene-001)",
				"2. [Scenes](chapter-02/scene-002)");
		}

		/// <summary>
		/// Verifies the contents of the chapter-02/scene-001.
		/// </summary>
		[Fact]
		public void VerifyChapter2Scene1()
		{
			Setup();

			List<string> lines =
				outputPersistence.GetDataLines("/chapter-02/scene-001");

			AssertLines(
				lines,
				"Text in chapter 2, scene 1.");
		}

		/// <summary>
		/// Verifies the contents of the chapter-02/scene-002.
		/// </summary>
		[Fact]
		public void VerifyChapter2Scene2()
		{
			Setup();

			List<string> lines =
				outputPersistence.GetDataLines("/chapter-02/scene-002");

			AssertLines(
				lines,
				"Text in chapter 2, scene 2.");
		}

		/// <summary>
		/// Verifies the resulting output files.
		/// </summary>
		[Fact]
		public void VerifyOutputFiles()
		{
			Setup();

			Assert.Equal(
				7,
				outputPersistence.DataCount);
		}

		/// <summary>
		/// Verifies the contents of the project file.
		/// </summary>
		[Fact]
		public void VerifyProjectContents()
		{
			Setup();

			List<string> lines = outputPersistence.GetDataLines("/");

			AssertLines(
				lines,
				"1. [Chapter 1](chapter-01)",
				"2. [Chapter 2](chapter-02)");
		}

		#endregion

		#region Methods

		/// <summary>
		/// Tests reading a single nested Internal region.
		/// </summary>
		private void Setup()
		{
			// Create the test input.
			var persistence = new MemoryPersistence();
			persistence.SetData(
				new HierarchicalPath("/"),
				"* [Chapter 1](chapter-01)",
				"* [Chapter 2](chapter-02)");
			persistence.SetData(
				new HierarchicalPath("/chapter-01"),
				"---",
				"title: Chapter 1",
				"---",
				"* [Scene 1](chapter-01/scene-001)",
				"* [Scene 2](chapter-01/scene-002)");
			persistence.SetData(
				new HierarchicalPath("/chapter-02"),
				"---",
				"title: Chapter 2",
				"---",
				"* [Scene 1](chapter-02/scene-001)",
				"* [Scene 2](chapter-02/scene-002)");
			persistence.SetData(
				new HierarchicalPath("/chapter-01/scene-001"),
				"Text in chapter 1, scene 1.");
			persistence.SetData(
				new HierarchicalPath("/chapter-01/scene-002"),
				"Text in chapter 1, scene 2.");
			persistence.SetData(
				new HierarchicalPath("/chapter-02/scene-001"),
				"Text in chapter 2, scene 1.");
			persistence.SetData(
				new HierarchicalPath("/chapter-02/scene-002"),
				"Text in chapter 2, scene 2.");

			// Set up the layout.
			var projectLayout = new RegionLayout
			{
				Name = "Project",
				Slug = "project",
				HasContent = false
			};
			var chapterLayout = new RegionLayout
			{
				Name = "Chapters",
				Slug = "chapter-$(ContainerIndex:00)",
				HasContent = false,
				IsExternal = true,
				IsSequenced = true
			};
			var sceneLayout = new RegionLayout
			{
				Name = "Scenes",
				Slug = "$(ParentSlug)/scene-$(ContainerIndex:000)",
				HasContent = true,
				IsExternal = true,
				IsSequenced = true
			};
			projectLayout.Add(chapterLayout);
			chapterLayout.Add(sceneLayout);

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

			// Using the same project layout, we create a new persistence and
			// write out the results.
			outputPersistence = new MemoryPersistence();
			outputContext = new BufferStoreContext(
				project,
				outputPersistence);

			format.StoreProject(outputContext);
		}

		#endregion
	}
}
