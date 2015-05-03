﻿// <copyright file="InitialState.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using NUnit.Framework;

namespace MfGames.TextTokens.Tests
{
	/// <summary>
	/// Verifies the state of an empty MemoryBuffer.
	/// </summary>
	[TestFixture]
	public class InitialState : MemoryBufferTests
	{
		#region Public Methods and Operators

		/// <summary>
		/// Verifies the number of lines in the buffer.
		/// </summary>
		[Test]
		public void HasCorrectLineCount()
		{
			Setup();
			Assert.AreEqual(
				0,
				State.Lines.Count,
				"Number of lines was unexpected.");
		}

		#endregion
	}
}
