#region Namespaces

using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using MfGames.Author.Contract.Languages;

#endregion

namespace MfGames.Author.Installers
{
	/// <summary>
	/// Installs the various language-specific elements into the Windsor container.
	/// </summary>
	public class LanguageInstaller
	{
		/// <summary>
		/// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="store">The configuration store.</param>
		public void Install(
			IWindsorContainer container,
			IConfigurationStore store)
		{
			// Register the individual input components.
			container.Register(
				AllTypes.FromThisAssembly().BasedOn<IParagraphParser>().WithService.
					DefaultInterface());
		}
	}
}