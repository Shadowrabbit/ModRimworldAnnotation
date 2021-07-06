﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200023F RID: 575
	public class ModLoadFolders
	{
		// Token: 0x060010A3 RID: 4259 RVA: 0x0005E07C File Offset: 0x0005C27C
		public List<LoadFolder> FoldersForVersion(string version)
		{
			if (this.foldersForVersion.ContainsKey(version))
			{
				return this.foldersForVersion[version];
			}
			return null;
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0005E09A File Offset: 0x0005C29A
		public List<string> DefinedVersions()
		{
			return this.foldersForVersion.Keys.ToList<string>();
		}

		// Token: 0x060010A5 RID: 4261 RVA: 0x0005E0AC File Offset: 0x0005C2AC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			foreach (object obj in xmlRoot.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!(xmlNode is XmlComment))
				{
					string text = xmlNode.Name.ToLower();
					if (text.StartsWith("v"))
					{
						text = text.Substring(1);
					}
					if (!this.foldersForVersion.ContainsKey(text))
					{
						this.foldersForVersion.Add(text, new List<LoadFolder>());
					}
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (!(xmlNode2 is XmlComment))
						{
							XmlAttributeCollection attributes = xmlNode2.Attributes;
							XmlAttribute xmlAttribute = (attributes != null) ? attributes["IfModActive"] : null;
							List<string> requiredPackageIds = null;
							if (xmlAttribute != null)
							{
								requiredPackageIds = (from s in xmlAttribute.Value.Split(new char[]
								{
									','
								})
								select s.Trim()).ToList<string>();
							}
							XmlAttributeCollection attributes2 = xmlNode2.Attributes;
							XmlAttribute xmlAttribute2 = (attributes2 != null) ? attributes2["IfModNotActive"] : null;
							List<string> disallowedPackageIds = null;
							if (xmlAttribute2 != null)
							{
								disallowedPackageIds = (from s in xmlAttribute2.Value.Split(new char[]
								{
									','
								})
								select s.Trim()).ToList<string>();
							}
							if (xmlNode2.InnerText == "/" || xmlNode2.InnerText == "\\")
							{
								this.foldersForVersion[text].Add(new LoadFolder("", requiredPackageIds, disallowedPackageIds));
							}
							else
							{
								this.foldersForVersion[text].Add(new LoadFolder(xmlNode2.InnerText, requiredPackageIds, disallowedPackageIds));
							}
						}
					}
				}
			}
		}

		// Token: 0x060010A6 RID: 4262 RVA: 0x0005E2F0 File Offset: 0x0005C4F0
		public List<string> GetIssueList(ModMetaData mod)
		{
			List<string> list = new List<string>();
			if (this.foldersForVersion.Count > 0)
			{
				string text = null;
				foreach (string text2 in this.foldersForVersion.Keys)
				{
					if (this.foldersForVersion[text2].Count == 0)
					{
						list.Add("ModLoadFolderListEmpty".Translate(text2));
					}
					foreach (LoadFolder loadFolder in from f in this.foldersForVersion[text2]
					group f by f into g
					where g.Count<LoadFolder>() > 1
					select g.Key)
					{
						list.Add("ModLoadFolderRepeatingFolder".Translate(text2, loadFolder.folderName));
					}
					if (!VersionControl.IsWellFormattedVersionString(text2) && !text2.Equals("default", StringComparison.InvariantCultureIgnoreCase))
					{
						list.Add("ModLoadFolderMalformedVersion".Translate(text2));
					}
					if (text2.Equals("default"))
					{
						list.Add("ModLoadFolderDefaultDeprecated".Translate());
					}
					Version v;
					Version v2;
					if (text != null && VersionControl.TryParseVersionString(text2, out v) && VersionControl.TryParseVersionString(text, out v2) && v < v2)
					{
						list.Add("ModLoadFolderOutOfOrder".Translate(text2, text));
					}
					for (int i = 0; i < this.foldersForVersion[text2].Count; i++)
					{
						LoadFolder loadFolder2 = this.foldersForVersion[text2][i];
						if (!Directory.Exists(Path.Combine(mod.RootDir.FullName, loadFolder2.folderName)))
						{
							list.Add("ModLoadFolderDoesntExist".Translate(loadFolder2.folderName, text2));
						}
					}
					Version item;
					if (VersionControl.TryParseVersionString(text2, out item) && !mod.SupportedVersionsReadOnly.Contains(item))
					{
						list.Add("ModLoadFolderDefinesUnsupportedGameVersion".Translate(text2));
					}
					text = text2;
				}
			}
			return list;
		}

		// Token: 0x04000CC4 RID: 3268
		private Dictionary<string, List<LoadFolder>> foldersForVersion = new Dictionary<string, List<LoadFolder>>();

		// Token: 0x04000CC5 RID: 3269
		public const string defaultVersionName = "default";
	}
}