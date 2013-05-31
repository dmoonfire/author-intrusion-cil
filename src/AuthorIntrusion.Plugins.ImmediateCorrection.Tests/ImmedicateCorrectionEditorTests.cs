﻿using System;
using AuthorIntrusion.Common;
using AuthorIntrusion.Common.Blocks;
using AuthorIntrusion.Common.Commands;
using AuthorIntrusion.Common.Plugins;
using NUnit.Framework;

namespace AuthorIntrusion.Plugins.ImmediateCorrection.Tests
{
	[TestFixture]
    public class ImmedicateCorrectionEditorTests
    {
		[Test]
		public void ActivatePlugin()
		{
			// Act
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks, out commands, out controller);

			// Assert
			Project project = blocks.Project;

			Assert.AreEqual(1, project.Plugins.Controllers.Count);
		}

		[Test]
		public void SimpleQuoteSubstitution()
		{
			// Arrange
			BlockOwnerCollection blocks;
			BlockCommandSupervisor commands;
			ImmediateCorrectionController controller;
			SetupCorrectionPlugin(out blocks,out commands,out controller);

			// Act
			controller.AddSubstitution("teh", "the", SubstitutionOptions.WholeWord);

			commands.InsertText(blocks[0], 0, "teh ");

			// Assert
			Assert.AreEqual("the ", blocks[0].Text);
		}

		/// <summary>
		/// Configures the environment to load the plugin manager and verify we
		/// have access to the ImmediateCorrectionPlugin.
		/// </summary>
		private void SetupCorrectionPlugin(
			out BlockOwnerCollection blocks,
			out BlockCommandSupervisor commands,
			out ImmediateCorrectionController controller)
		{
			// Start by making sure the plugin manager is configured.
			var resolver = new EnvironmentResolver();

			resolver.LoadPluginManager();

			// Create a project and pull out the useful properties we'll use to
			// make changes.
			var project = new Project();

			blocks = project.Blocks;
			commands = project.Commands;

			// Load in the immediate correction editor.
			if (!project.Plugins.Add("Immediate Correction"))
			{
				// We couldn't load it for some reason.
				throw new ApplicationException("Cannot load immediate correction plugin");
			}

			// Pull out the controller for the correction and cast it (since we know
			// what type it is).
			ProjectPluginController pluginController = project.Plugins.Controllers[0];
			controller = (ImmediateCorrectionController) pluginController.Controller;
		}
    }
}
