#region Namespaces

using AuthorIntrusion.Contracts.Collections;
using AuthorIntrusion.Contracts.Enumerations;

#endregion

namespace AuthorIntrusion.Contracts.Languages
{
	/// <summary>
	/// Describes a class that can parse the unparsed contents of a paragraph
	/// and populate it a parsed sentences.
	/// </summary>
	public interface IContentParser : ILanguageSpecific
	{
		#region Parsing

		/// <summary>
		/// Parses the content of the content container and replaces the contents
		/// with parsed data.
		/// </summary>
		/// <param name="contents">The content container.</param>
		/// <returns>The status result from the parse.</returns>
		ParserStatus Parse(ContentList contents);

		#endregion
	}
}