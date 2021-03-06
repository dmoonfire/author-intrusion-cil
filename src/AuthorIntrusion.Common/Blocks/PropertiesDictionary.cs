﻿// Copyright 2012-2013 Moonfire Games
// Released under the MIT license
// http://mfgames.com/author-intrusion/license

using System;
using System.Collections.Generic;
using MfGames.Conversion;
using MfGames.HierarchicalPaths;

namespace AuthorIntrusion.Common.Blocks
{
	/// <summary>
	/// Implements a dictionary of properties to be assigned to a block or
	/// project.
	/// </summary>
	public class PropertiesDictionary: Dictionary<HierarchicalPath, string>
	{
		#region Methods

		/// <summary>
		/// Either adds a value to an existing key or adds a new one.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="amount">The amount.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public void AdditionOrAdd(
			HierarchicalPath path,
			int amount)
		{
			if (ContainsKey(path))
			{
				int value = Convert.ToInt32(this[path]);
				this[path] = Convert.ToString(value + amount);
			}
			else
			{
				this[path] = Convert.ToString(amount);
			}
		}

		public TResult Get<TResult>(HierarchicalPath path)
		{
			string value = this[path];
			TResult result = ExtendableConvert.Instance.Convert<string, TResult>(value);
			return result;
		}

		public TResult Get<TResult>(
			string path,
			HierarchicalPath rootPath = null)
		{
			var key = new HierarchicalPath(path, rootPath);
			var results = Get<TResult>(key);
			return results;
		}

		/// <summary>
		/// Gets the value at the path, or the default if the item is not a stored
		/// property.
		/// </summary>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="path">The path.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>Either the converted value or the default value.</returns>
		public TResult GetOrDefault<TResult>(
			HierarchicalPath path,
			TResult defaultValue)
		{
			return ContainsKey(path)
				? Get<TResult>(path)
				: defaultValue;
		}

		#endregion
	}
}
