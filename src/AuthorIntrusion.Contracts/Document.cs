#region Copyright and License

// Copyright (c) 2011, Moonfire Games
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

#endregion

#region Namespaces

using System;
using System.Diagnostics;

using AuthorIntrusion.Contracts.Matters;
using AuthorIntrusion.Contracts.Structures;

#endregion

namespace AuthorIntrusion.Contracts
{
	/// <summary>
	/// Represents an entire Author Intrusion document, including the single
	/// root node for the entire document.
	/// </summary>
	public class Document : Element, IMattersContainer
	{
		#region Fields

		private readonly DocumentMatterCollection documentMatters;
		private readonly MatterCollection matters;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Document"/> class.
		/// </summary>
		public Document()
		{
			matters = new MatterCollection(this);
			matters.ParagraphChanged += OnParagraphChanged;
			documentMatters = new DocumentMatterCollection(matters);
		}

		#endregion

		#region Matters

		/// <summary>
		/// Gets the zero-based depth of the object in the container.
		/// </summary>
		public int Depth
		{
			[DebuggerStepThrough]
			get { return 0; }
		}

		/// <summary>
		/// Gets the flattened list of document matters.
		/// </summary>
		public DocumentMatterCollection DocumentMatters
		{
			[DebuggerStepThrough]
			get { return documentMatters; }
		}

		/// <summary>
		/// Gets an ordered list of matters inside the document.
		/// </summary>
		public MatterCollection Matters
		{
			[DebuggerStepThrough]
			get { return matters; }
		}

		/// <summary>
		/// Gets the parent container object.
		/// </summary>
		public IMattersContainer ParentContainer
		{
			[DebuggerStepThrough]
			get { return null; }
		}

		/// <summary>
		/// Gets the index of this instance in its parent container.
		/// </summary>
		/// <value>
		/// The index of the item in the parent.
		/// </value>
		public int ParentIndex
		{
			[DebuggerStepThrough]
			get { return -1; }
		}

		/// <summary>
		/// Generates a thumbprint of the document. This is mainly for
		/// unit-testing, but could be used to get an overall view of an entire
		/// document.
		/// </summary>
		/// <returns></returns>
		public string GetThumbprint()
		{
			var thumbnailer = new DocumentThumbnailer();

			thumbnailer.Visit(this);

			return thumbnailer.Thumbnail;
		}

		#endregion

		#region Editing

		/// <summary>
		/// Called when a contained paragraph changes.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="AuthorIntrusion.Contracts.Matters.ParagraphChangedEventArgs"/> instance containing the event data.</param>
		private void OnParagraphChanged(
			object sender,
			ParagraphChangedEventArgs e)
		{
			RaiseParagraphChanged(e);
		}

		/// <summary>
		/// Occurs when a contained paragraph changes.
		/// </summary>
		public event EventHandler<ParagraphChangedEventArgs> ParagraphChanged;

		/// <summary>
		/// Raises the paragraph changed event.
		/// </summary>
		/// <param name="e">The <see cref="AuthorIntrusion.Contracts.Matters.ParagraphChangedEventArgs"/> instance containing the event data.</param>
		protected void RaiseParagraphChanged(ParagraphChangedEventArgs e)
		{
			if (ParagraphChanged != null)
			{
				ParagraphChanged(this, e);
			}
		}

		#endregion
	}
}