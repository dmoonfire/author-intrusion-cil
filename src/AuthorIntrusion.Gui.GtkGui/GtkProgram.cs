﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.IO;
using Gtk;

namespace AuthorIntrusion.Gui.GtkGui
{
	/// <summary>
	/// Sets up the Gtk framework and starts up the application.
	/// </summary>
	internal static class GtkProgram
	{
		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			// Initial the Gtk GUI framework.
			Application.Init("Author Intrusion", ref args);

			// We use the Inversion of Control (IoC) container to resolve all the
			// elements of the window. This lets everything wire up together without
			// having a lot of maintenance or singletons.
			var resolver = new EnvironmentResolver();

			// Set up the environment.
			resolver.LoadPluginManager();

			// Create the main window, show its contents, and start the Gtk loop.
			try
			{
				var projectManager = new ProjectManager();
				var mainWindow = new MainWindow(projectManager);

				mainWindow.ShowAll();

				// If we have some arguments, try to load the file as project.
				if (args.Length > 0)
				{
					var projectFile = new FileInfo(args[0]);
					mainWindow.OpenProject(projectFile);
				}

				// Start running the application.
				Application.Run();
			}
			catch (Exception exception)
			{
				Console.WriteLine("There was an exception");

				Exception e = exception;

				while (e != null)
				{
					Console.WriteLine("=== EXCEPTION");
					Console.WriteLine(e);
					e = e.InnerException;
				}

				throw;
			}
		}

		#endregion
	}
}
