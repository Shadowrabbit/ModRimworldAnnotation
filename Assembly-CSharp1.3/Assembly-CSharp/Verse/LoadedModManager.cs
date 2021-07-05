using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000233 RID: 563
	public static class LoadedModManager
	{
		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x0005AB13 File Offset: 0x00058D13
		public static List<ModContentPack> RunningModsListForReading
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x0005AB13 File Offset: 0x00058D13
		public static IEnumerable<ModContentPack> RunningMods
		{
			get
			{
				return LoadedModManager.runningMods;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x0005AB1A File Offset: 0x00058D1A
		public static List<Def> PatchedDefsForReading
		{
			get
			{
				return LoadedModManager.patchedDefs;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000FFE RID: 4094 RVA: 0x0005AB21 File Offset: 0x00058D21
		public static IEnumerable<Mod> ModHandles
		{
			get
			{
				return LoadedModManager.runningModClasses.Values;
			}
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x0005AB30 File Offset: 0x00058D30
		public static void LoadAllActiveMods()
		{
			DeepProfiler.Start("XmlInheritance.Clear()");
			try
			{
				XmlInheritance.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("InitializeMods()");
			try
			{
				LoadedModManager.InitializeMods();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("LoadModContent()");
			try
			{
				LoadedModManager.LoadModContent();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("CreateModClasses()");
			try
			{
				LoadedModManager.CreateModClasses();
			}
			finally
			{
				DeepProfiler.End();
			}
			List<LoadableXmlAsset> xmls = null;
			DeepProfiler.Start("LoadModXML()");
			try
			{
				xmls = LoadedModManager.LoadModXML();
			}
			finally
			{
				DeepProfiler.End();
			}
			Dictionary<XmlNode, LoadableXmlAsset> assetlookup = new Dictionary<XmlNode, LoadableXmlAsset>();
			XmlDocument xmlDocument = null;
			DeepProfiler.Start("CombineIntoUnifiedXML()");
			try
			{
				xmlDocument = LoadedModManager.CombineIntoUnifiedXML(xmls, assetlookup);
			}
			finally
			{
				DeepProfiler.End();
			}
			TKeySystem.Clear();
			DeepProfiler.Start("TKeySystem.Parse()");
			try
			{
				TKeySystem.Parse(xmlDocument);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ErrorCheckPatches()");
			try
			{
				LoadedModManager.ErrorCheckPatches();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ApplyPatches()");
			try
			{
				LoadedModManager.ApplyPatches(xmlDocument, assetlookup);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ParseAndProcessXML()");
			try
			{
				LoadedModManager.ParseAndProcessXML(xmlDocument, assetlookup);
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("ClearCachedPatches()");
			try
			{
				LoadedModManager.ClearCachedPatches();
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("XmlInheritance.Clear()");
			try
			{
				XmlInheritance.Clear();
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x0005AD00 File Offset: 0x00058F00
		public static void InitializeMods()
		{
			int num = 0;
			foreach (ModMetaData modMetaData in ModsConfig.ActiveModsInLoadOrder.ToList<ModMetaData>())
			{
				DeepProfiler.Start("Initializing " + modMetaData);
				try
				{
					if (!modMetaData.RootDir.Exists)
					{
						ModsConfig.SetActive(modMetaData.PackageId, false);
						Log.Warning(string.Concat(new object[]
						{
							"Failed to find active mod ",
							modMetaData.Name,
							"(",
							modMetaData.PackageIdPlayerFacing,
							") at ",
							modMetaData.RootDir
						}));
					}
					else
					{
						ModContentPack item = new ModContentPack(modMetaData.RootDir, modMetaData.PackageId, modMetaData.PackageIdPlayerFacing, num, modMetaData.Name, modMetaData.Official);
						num++;
						LoadedModManager.runningMods.Add(item);
					}
				}
				catch (Exception arg)
				{
					Log.Error("Error initializing mod: " + arg);
					ModsConfig.SetActive(modMetaData.PackageId, false);
				}
				finally
				{
					DeepProfiler.End();
				}
			}
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x0005AE3C File Offset: 0x0005903C
		public static void LoadModContent()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DeepProfiler.Start("LoadModContent");
			});
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				DeepProfiler.Start("Loading " + modContentPack + " content");
				try
				{
					modContentPack.ReloadContent();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not reload mod content for mod ",
						modContentPack.PackageIdPlayerFacing,
						": ",
						ex
					}));
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				DeepProfiler.End();
				for (int j = 0; j < LoadedModManager.runningMods.Count; j++)
				{
					ModContentPack modContentPack2 = LoadedModManager.runningMods[j];
					if (!modContentPack2.AnyContentLoaded())
					{
						Log.Error("Mod " + modContentPack2.Name + " did not load any content. Following load folders were used:\n" + modContentPack2.foldersToLoadDescendingOrder.ToLineList("  - "));
					}
				}
			});
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x0005AF24 File Offset: 0x00059124
		public static void CreateModClasses()
		{
			using (IEnumerator<Type> enumerator = typeof(Mod).InstantiableDescendantsAndSelf().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type type = enumerator.Current;
					DeepProfiler.Start("Loading " + type + " mod class");
					try
					{
						if (!LoadedModManager.runningModClasses.ContainsKey(type))
						{
							ModContentPack modContentPack = (from modpack in LoadedModManager.runningMods
							where modpack.assemblies.loadedAssemblies.Contains(type.Assembly)
							select modpack).FirstOrDefault<ModContentPack>();
							LoadedModManager.runningModClasses[type] = (Mod)Activator.CreateInstance(type, new object[]
							{
								modContentPack
							});
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Error while instantiating a mod of type ",
							type,
							": ",
							ex
						}));
					}
					finally
					{
						DeepProfiler.End();
					}
				}
			}
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x0005B048 File Offset: 0x00059248
		public static List<LoadableXmlAsset> LoadModXML()
		{
			List<LoadableXmlAsset> list = new List<LoadableXmlAsset>();
			for (int i = 0; i < LoadedModManager.runningMods.Count; i++)
			{
				ModContentPack modContentPack = LoadedModManager.runningMods[i];
				DeepProfiler.Start("Loading " + modContentPack);
				try
				{
					list.AddRange(modContentPack.LoadDefs());
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not load defs for mod ",
						modContentPack.PackageIdPlayerFacing,
						": ",
						ex
					}));
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			return list;
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x0005B0F0 File Offset: 0x000592F0
		public static void ErrorCheckPatches()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				foreach (PatchOperation patchOperation in modContentPack.Patches)
				{
					try
					{
						foreach (string text in patchOperation.ConfigErrors())
						{
							Log.Error(string.Concat(new object[]
							{
								"Config error in ",
								modContentPack.Name,
								" patch ",
								patchOperation,
								": ",
								text
							}));
						}
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Exception in ConfigErrors() of ",
							modContentPack.Name,
							" patch ",
							patchOperation,
							": ",
							ex
						}));
					}
				}
			}
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x0005B240 File Offset: 0x00059440
		public static void ApplyPatches(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			foreach (PatchOperation patchOperation in LoadedModManager.runningMods.SelectMany((ModContentPack rm) => rm.Patches))
			{
				try
				{
					patchOperation.Apply(xmlDoc);
				}
				catch (Exception arg)
				{
					Log.Error("Error in patch.Apply(): " + arg);
				}
			}
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x0005B2D4 File Offset: 0x000594D4
		public static XmlDocument CombineIntoUnifiedXML(List<LoadableXmlAsset> xmls, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.AppendChild(xmlDocument.CreateElement("Defs"));
			foreach (LoadableXmlAsset loadableXmlAsset in xmls)
			{
				if (loadableXmlAsset.xmlDoc == null || loadableXmlAsset.xmlDoc.DocumentElement == null)
				{
					Log.Error(string.Format("{0}: unknown parse failure", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name));
				}
				else
				{
					if (loadableXmlAsset.xmlDoc.DocumentElement.Name != "Defs")
					{
						Log.Error(string.Format("{0}: root element named {1}; should be named Defs", loadableXmlAsset.fullFolderPath + "/" + loadableXmlAsset.name, loadableXmlAsset.xmlDoc.DocumentElement.Name));
					}
					foreach (object obj in loadableXmlAsset.xmlDoc.DocumentElement.ChildNodes)
					{
						XmlNode node = (XmlNode)obj;
						XmlNode xmlNode = xmlDocument.ImportNode(node, true);
						assetlookup[xmlNode] = loadableXmlAsset;
						xmlDocument.DocumentElement.AppendChild(xmlNode);
					}
				}
			}
			return xmlDocument;
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x0005B454 File Offset: 0x00059654
		public static void ParseAndProcessXML(XmlDocument xmlDoc, Dictionary<XmlNode, LoadableXmlAsset> assetlookup)
		{
			XmlNodeList childNodes = xmlDoc.DocumentElement.ChildNodes;
			List<XmlNode> list = new List<XmlNode>();
			foreach (object obj in childNodes)
			{
				list.Add(obj as XmlNode);
			}
			DeepProfiler.Start("Loading asset nodes " + list.Count);
			try
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].NodeType == XmlNodeType.Element)
					{
						LoadableXmlAsset loadableXmlAsset = null;
						DeepProfiler.Start("assetlookup.TryGetValue");
						try
						{
							assetlookup.TryGetValue(list[i], out loadableXmlAsset);
						}
						finally
						{
							DeepProfiler.End();
						}
						DeepProfiler.Start("XmlInheritance.TryRegister");
						try
						{
							XmlInheritance.TryRegister(list[i], (loadableXmlAsset != null) ? loadableXmlAsset.mod : null);
						}
						finally
						{
							DeepProfiler.End();
						}
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
			DeepProfiler.Start("XmlInheritance.Resolve()");
			try
			{
				XmlInheritance.Resolve();
			}
			finally
			{
				DeepProfiler.End();
			}
			LoadedModManager.runningMods.FirstOrDefault<ModContentPack>();
			DeepProfiler.Start("Loading defs for " + list.Count + " nodes");
			try
			{
				foreach (XmlNode xmlNode in list)
				{
					XmlAttributeCollection attributes = xmlNode.Attributes;
					string text;
					if (attributes == null)
					{
						text = null;
					}
					else
					{
						XmlAttribute xmlAttribute = attributes["MayRequire"];
						text = ((xmlAttribute != null) ? xmlAttribute.Value.ToLower() : null);
					}
					string text2 = text;
					if (text2 == null || ModsConfig.IsActive(text2))
					{
						LoadableXmlAsset loadableXmlAsset2 = assetlookup.TryGetValue(xmlNode, null);
						Def def = DirectXmlLoader.DefFromNode(xmlNode, loadableXmlAsset2);
						if (def != null)
						{
							ModContentPack modContentPack = (loadableXmlAsset2 != null) ? loadableXmlAsset2.mod : null;
							if (modContentPack != null)
							{
								modContentPack.AddDef(def, loadableXmlAsset2.name);
							}
							else
							{
								LoadedModManager.patchedDefs.Add(def);
							}
						}
					}
				}
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x0005B698 File Offset: 0x00059898
		public static void ClearCachedPatches()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				foreach (PatchOperation patchOperation in modContentPack.Patches)
				{
					try
					{
						patchOperation.Complete(modContentPack.Name);
					}
					catch (Exception arg)
					{
						Log.Error("Error in patch.Complete(): " + arg);
					}
				}
				modContentPack.ClearPatchesCache();
			}
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x0005B750 File Offset: 0x00059950
		public static void ClearDestroy()
		{
			foreach (ModContentPack modContentPack in LoadedModManager.runningMods)
			{
				try
				{
					modContentPack.ClearDestroy();
				}
				catch (Exception arg)
				{
					Log.Error("Error in mod.ClearDestroy(): " + arg);
				}
			}
			LoadedModManager.runningMods.Clear();
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x0005B7CC File Offset: 0x000599CC
		public static T GetMod<T>() where T : Mod
		{
			return LoadedModManager.GetMod(typeof(T)) as T;
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x0005B7E8 File Offset: 0x000599E8
		public static Mod GetMod(Type type)
		{
			if (LoadedModManager.runningModClasses.ContainsKey(type))
			{
				return LoadedModManager.runningModClasses[type];
			}
			return (from kvp in LoadedModManager.runningModClasses
			where type.IsAssignableFrom(kvp.Key)
			select kvp).FirstOrDefault<KeyValuePair<Type, Mod>>().Value;
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x0005B848 File Offset: 0x00059A48
		private static string GetSettingsFilename(string modIdentifier, string modHandleName)
		{
			return Path.Combine(GenFilePaths.ConfigFolderPath, GenText.SanitizeFilename(string.Format("Mod_{0}_{1}.xml", modIdentifier, modHandleName)));
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x0005B868 File Offset: 0x00059A68
		public static T ReadModSettings<T>(string modIdentifier, string modHandleName) where T : ModSettings, new()
		{
			string settingsFilename = LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName);
			T t = default(T);
			try
			{
				if (File.Exists(settingsFilename))
				{
					Scribe.loader.InitLoading(settingsFilename);
					try
					{
						Scribe_Deep.Look<T>(ref t, "ModSettings", Array.Empty<object>());
					}
					finally
					{
						Scribe.loader.FinalizeLoading();
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Caught exception while loading mod settings data for {0}. Generating fresh settings. The exception was: {1}", modIdentifier, ex.ToString()));
				t = default(T);
			}
			if (t == null)
			{
				t = Activator.CreateInstance<T>();
			}
			return t;
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0005B904 File Offset: 0x00059B04
		public static void WriteModSettings(string modIdentifier, string modHandleName, ModSettings settings)
		{
			Scribe.saver.InitSaving(LoadedModManager.GetSettingsFilename(modIdentifier, modHandleName), "SettingsBlock");
			try
			{
				Scribe_Deep.Look<ModSettings>(ref settings, "ModSettings", Array.Empty<object>());
			}
			finally
			{
				Scribe.saver.FinalizeSaving();
			}
		}

		// Token: 0x04000C7C RID: 3196
		private static List<ModContentPack> runningMods = new List<ModContentPack>();

		// Token: 0x04000C7D RID: 3197
		private static Dictionary<Type, Mod> runningModClasses = new Dictionary<Type, Mod>();

		// Token: 0x04000C7E RID: 3198
		private static List<Def> patchedDefs = new List<Def>();
	}
}
