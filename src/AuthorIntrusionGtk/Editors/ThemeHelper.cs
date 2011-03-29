using System;

using AuthorIntrusion.Contracts.Enumerations;

using Cairo;

using MfGames.GtkExt;
using MfGames.GtkExt.TextEditor.Models.Styles;

namespace AuthorIntrusionGtk.Editors
{
	/// <summary>
	/// Class to configure the structure of the text editor theme.
	/// </summary>
	public static class ThemeHelper
	{
		/// <summary>
		/// Creates the theme structure for the text editor and gives it
		/// sane defaults.
		/// </summary>
		/// <returns></returns>
		public static void SetupTheme(Theme theme)
		{
			// Pull out the common styles we'll use.
			var textStyle = theme.TextLineStyle;

			// Create the section style container.
			var sectionStyle = new LineBlockStyle(textStyle);

			sectionStyle.StyleUsage = StyleUsage.Application;

			theme.LineStyles["Section"] = sectionStyle;

			// Go through all the structure types and create a theme for them.
			foreach (StructureType structureType in Enum.GetValues(typeof(StructureType)))
			{
				// Create the basic style.
				var lineStyle = new LineBlockStyle(
					structureType == StructureType.Paragraph ? textStyle : sectionStyle);

				lineStyle.StyleUsage = StyleUsage.Application;

				theme.LineStyles[structureType.ToString()] = lineStyle;

				// Create a style if the line is empty. This defaults to a
				// grayed out line.
				var emptyLineStyle = new LineBlockStyle(lineStyle);

				emptyLineStyle.StyleUsage = StyleUsage.Application;
				emptyLineStyle.ForegroundColor = new Color(0.8, 0.8, 0.8);

				theme.LineStyles["Blank " + structureType] = emptyLineStyle;

				// Perform type-specific initialization.
				switch (structureType)
				{
					case StructureType.Book:
						lineStyle.FontDescription =
							FontDescriptionCache.GetFontDescription("Courier New Bold 18");
						break;

					case StructureType.Chapter:
					case StructureType.Article:
						lineStyle.FontDescription =
							FontDescriptionCache.GetFontDescription("Courier New Bold 14");
						break;
				}
			}

			// Update the styles to make them a little more distinct as a default.
			theme.TextLineStyle.FontDescription =
				FontDescriptionCache.GetFontDescription("Courier New 12");

			sectionStyle.FontDescription =
				FontDescriptionCache.GetFontDescription("Courier New Bold 12");

			// Return the resulting theme.
			return;
		}
	}
}