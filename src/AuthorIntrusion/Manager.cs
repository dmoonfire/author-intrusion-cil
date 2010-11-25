#region Namespaces

using System;

using AuthorIntrusion.Contracts.IO;
using AuthorIntrusion.Contracts.Languages;
using AuthorIntrusion.IO;
using AuthorIntrusion.Languages;

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

#endregion

namespace AuthorIntrusion
{
	/// <summary>
	/// Manages the entire author process, including extensions.
	/// </summary>
	public class Manager : IDisposable
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Manager"/> class.
		/// </summary>
		public Manager()
		{
			// Set up Windsor container along with the extensions.
			container = new WindsorContainer();
			container.Install(
				FromAssembly.This(),
				FromAssembly.InDirectory(new AssemblyFilter("Extensions", "*.dll")));
			container.Kernel.Resolver.AddSubResolver(
				new CollectionResolver(container.Kernel, true));

			container.Register(
				Component.For<IInputManager>().ImplementedBy<InputManager>(),
				Component.For<IOutputManager>().ImplementedBy<OutputManager>(),
				Component.For<ILanguageManager>().ImplementedBy<LanguageManager>());
		}

		#endregion

		#region Destructors

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (container != null)
			{
				container.Dispose();
				container = null;
			}
		}

		#endregion

		#region IOC

		private WindsorContainer container;

		#endregion

		#region Managers

		/// <summary>
		/// Gets the input manager.
		/// </summary>
		/// <value>The input manager.</value>
		public IInputManager InputManager
		{
			get { return container.Resolve<IInputManager>(); }
		}

		/// <summary>
		/// Contains the language manager.
		/// </summary>
		/// <value>The language manager.</value>
		public ILanguageManager LanguageManager
		{
			get { return container.Resolve<ILanguageManager>(); }
		}

		/// <summary>
		/// Gets the output manager.
		/// </summary>
		/// <value>The output manager.</value>
		public IOutputManager OutputManager
		{
			get { return container.Resolve<IOutputManager>(); }
		}

		#endregion
	}
}