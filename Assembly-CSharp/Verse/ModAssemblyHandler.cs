using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Verse
{
	// Token: 0x02000338 RID: 824
	public class ModAssemblyHandler
	{
		// Token: 0x060014F5 RID: 5365 RVA: 0x00014FAC File Offset: 0x000131AC
		public ModAssemblyHandler(ModContentPack mod)
		{
			this.mod = mod;
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x000D0ED8 File Offset: 0x000CF0D8
		public void ReloadAll()
		{
			if (!ModAssemblyHandler.globalResolverIsSet)
			{
				ResolveEventHandler @object = (object obj, ResolveEventArgs args) => Assembly.GetExecutingAssembly();
				AppDomain.CurrentDomain.AssemblyResolve += @object.Invoke;
				ModAssemblyHandler.globalResolverIsSet = true;
			}
			foreach (FileInfo fileInfo in from f in ModContentPack.GetAllFilesForModPreserveOrder(this.mod, "Assemblies/", (string e) => e.ToLower() == ".dll", null)
			select f.Item2)
			{
				Assembly assembly = null;
				try
				{
					byte[] rawAssembly = File.ReadAllBytes(fileInfo.FullName);
					FileInfo fileInfo2 = new FileInfo(Path.Combine(fileInfo.DirectoryName, Path.GetFileNameWithoutExtension(fileInfo.FullName)) + ".pdb");
					if (fileInfo2.Exists)
					{
						byte[] rawSymbolStore = File.ReadAllBytes(fileInfo2.FullName);
						assembly = AppDomain.CurrentDomain.Load(rawAssembly, rawSymbolStore);
					}
					else
					{
						assembly = AppDomain.CurrentDomain.Load(rawAssembly);
					}
				}
				catch (Exception ex)
				{
					Log.Error("Exception loading " + fileInfo.Name + ": " + ex.ToString(), false);
					break;
				}
				if (!(assembly == null) && this.AssemblyIsUsable(assembly))
				{
					this.loadedAssemblies.Add(assembly);
				}
			}
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x000D1074 File Offset: 0x000CF274
		private bool AssemblyIsUsable(Assembly asm)
		{
			try
			{
				asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"ReflectionTypeLoadException getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex
				}));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Loader exceptions:");
				if (ex.LoaderExceptions != null)
				{
					foreach (Exception ex2 in ex.LoaderExceptions)
					{
						stringBuilder.AppendLine("   => " + ex2.ToString());
					}
				}
				Log.Error(stringBuilder.ToString(), false);
				return false;
			}
			catch (Exception ex3)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception getting types in assembly ",
					asm.GetName().Name,
					": ",
					ex3
				}), false);
				return false;
			}
			return true;
		}

		// Token: 0x04001045 RID: 4165
		private ModContentPack mod;

		// Token: 0x04001046 RID: 4166
		public List<Assembly> loadedAssemblies = new List<Assembly>();

		// Token: 0x04001047 RID: 4167
		private static bool globalResolverIsSet;
	}
}
