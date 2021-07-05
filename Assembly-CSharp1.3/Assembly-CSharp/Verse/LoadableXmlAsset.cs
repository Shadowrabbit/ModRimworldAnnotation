using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000319 RID: 793
	public class LoadableXmlAsset
	{
		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x060016B6 RID: 5814 RVA: 0x00085658 File Offset: 0x00083858
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar.ToString() + this.name;
			}
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x00085684 File Offset: 0x00083884
		public LoadableXmlAsset(string name, string fullFolderPath, string contents)
		{
			this.name = name;
			this.fullFolderPath = fullFolderPath;
			try
			{
				XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
				xmlReaderSettings.IgnoreComments = true;
				xmlReaderSettings.IgnoreWhitespace = true;
				xmlReaderSettings.CheckCharacters = false;
				using (StringReader stringReader = new StringReader(contents))
				{
					using (XmlReader xmlReader = XmlReader.Create(stringReader, xmlReaderSettings))
					{
						this.xmlDoc = new XmlDocument();
						this.xmlDoc.Load(xmlReader);
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Exception reading ",
					name,
					" as XML: ",
					ex
				}));
				this.xmlDoc = null;
			}
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x0008575C File Offset: 0x0008395C
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04000FCA RID: 4042
		private static XmlReader reader;

		// Token: 0x04000FCB RID: 4043
		public string name;

		// Token: 0x04000FCC RID: 4044
		public string fullFolderPath;

		// Token: 0x04000FCD RID: 4045
		public XmlDocument xmlDoc;

		// Token: 0x04000FCE RID: 4046
		public ModContentPack mod;
	}
}
