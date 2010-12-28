#region Namespaces

using System;
using System.Collections.Generic;
using System.Xml;

using AuthorIntrusion.Contracts;
using AuthorIntrusion.Contracts.Constants;
using AuthorIntrusion.Contracts.Contents;
using AuthorIntrusion.Contracts.Interfaces;
using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.IO
{
	/// <summary>
	/// Defines an input reader that takes Docbook 5 XML and produces the
	/// internal structure. Docbook elements that are not understood are ignored
	/// and dropped.
	/// </summary>
	public class Docbook5InputReader : XmlInputReaderBase
	{
		#region Identification

		/// <summary>
		/// Gets the file masks that are commonly associated with this input
		/// file format.
		/// </summary>
		/// <value>The file mask.</value>
		public override string[] FileExtensions
		{
			get { return new[] { ".xml" }; }
		}

		/// <summary>
		/// Gets the name of the input file.
		/// </summary>
		/// <value>The name.</value>
		public override string Name
		{
			get { return "Docbook 5"; }
		}

		/// <summary>
		/// Determines whether this instance can read XML files with the given root
		/// element.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns>
		/// 	<c>true</c> if this instance [can read element] the specified reader; otherwise, <c>false</c>.
		/// </returns>
		protected override bool CanReadElement(XmlReader reader)
		{
			if (reader.NamespaceURI != Namespaces.Docbook5 || reader["version"] != "5.0")
			{
				return false;
			}

			return true;
		}

		#endregion

		#region Reading

		/// <summary>
		/// Reads the specified input stream and returns a structure elements.
		/// If there is any problems with reading the input, this should throw
		/// an exception and never return a null root structure.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <returns></returns>
		protected override Structure Read(XmlReader reader)
		{
			// This implements a very simple Docbook 5 XML reader that ignores
			// all the elements outside of the scope of this application and 
			// creates a simplified structure.
			var context = new List<Element>();
			Structure rootStructure = null;

			while (reader.Read())
			{
				switch (reader.NodeType)
				{
					case XmlNodeType.Element:
						ReadElement(reader, context);

						if (context.Count == 1)
						{
							rootStructure = context[0] as Structure;
						}
						break;

					case XmlNodeType.EndElement:
						ReadEndElement(reader, context);
						break;

					case XmlNodeType.Text:
						ReadText(reader, context);
						break;
				}
			}

			// If we still have a null root structure, something is wrong.
			if (rootStructure == null)
			{
				throw new Exception("Cannot identify the root level element");
			}

			if (rootStructure == null)
			{
				throw new Exception("Root element is not a structure");
			}

			// There is nothing wrong with the parse, so return the root.
			return rootStructure;
		}

		/// <summary>
		/// Reads the element from the XML reader and parses it.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="context">The context.</param>
		private static void ReadElement(
			XmlReader reader,
			List<Element> context)
		{
			// If we aren't a DocBook element, just ignore it.
			if (reader.NamespaceURI != Namespaces.Docbook5)
			{
				return;
			}

			// Get the last item in the context.
			Structure parent = null;

			if (context.Count > 0)
			{
				parent = context[context.Count - 1] as Structure;
			}

			// Switch based on the local tag.
			Element element;

			switch (reader.LocalName)
			{
				case "book":
					element = new StructureContainerStructure();
					break;

				case "chapter":
					var chapter = new StructureContainerStructure();
					element = chapter;

					if (parent != null && parent is IStructureContainer)
					{
						((IStructureContainer) parent).Structures.Add(chapter);
					}
					break;

				case "article":
					element = new StructureContainerStructure();
					break;

				case "section":
					var section = new StructureContainerStructure();
					element = section;

					if (parent != null && parent is IStructureContainer)
					{
						((IStructureContainer) parent).Structures.Add(section);
					}
					break;

				case "para":
				case "simpara":
					var paragraph = new ContentContainerStructure();
					element = paragraph;

					if (parent != null && parent is IStructureContainer)
					{
						((IStructureContainer) parent).Structures.Add(paragraph);
					}

					break;

				case "quote":
					if (parent != null && parent is IContentContainer)
					{
						((IContentContainer) parent).Contents.Add("\"");
					}

					return;

				default:
					// Unknown type, so just skip it.
					return;
			}

			// Add the structure to the context.
			context.Add(element);
		}

		/// <summary>
		/// Reads the element from the XML reader and parses it.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="context">The context.</param>
		private static void ReadEndElement(
			XmlReader reader,
			List<Element> context)
		{
			// If we aren't a DocBook element, just ignore it.
			if (reader.NamespaceURI != Namespaces.Docbook5)
			{
				return;
			}

			// Switch based on the local tag.
			switch (reader.LocalName)
			{
				case "quote":
					IContentContainer parent = (IContentContainer) context[context.Count - 1]; 
					parent.Contents.Add("\"");
					break;

				case "book":
				case "chapter":
				case "article":
				case "section":
				case "para":
				case "simpara":
					// Remove the last item which should be this element.
					context.RemoveAt(context.Count - 1);
					break;
			}
		}

		/// <summary>
		/// Reads the text from the given XML string.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="context">The context.</param>
		private static void ReadText(
			XmlReader reader,
			List<Element> context)
		{
			// Figure out where to put this text content.
			if (context.Count == 0)
			{
				throw new Exception("Cannot figure out context to add contents");
			}

			// Make sure the last item in the context can contain contents.
			var container = context[context.Count - 1] as IContentContainer;

			if (container == null)
			{
				// Ignore it since the item can't handle it. This is used for things like
				// titles.
				return;
			}

			// Wrap the text into an unparsed string and add it to the container.
			container.Contents.Add(reader.Value);
		}

		#endregion
	}
}