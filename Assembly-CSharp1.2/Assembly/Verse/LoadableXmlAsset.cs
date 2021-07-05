using System;
using System.IO;
using System.Xml;

namespace Verse
{
	// Token: 0x02000495 RID: 1173
	public class LoadableXmlAsset
	{
		// Token: 0x1700058C RID: 1420
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x000F5A6C File Offset: 0x000F3C6C
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar.ToString() + this.name;
			}
		}

		// Token: 0x06001D6F RID: 7535 RVA: 0x000F5A98 File Offset: 0x000F3C98
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
				}), false);
				this.xmlDoc = null;
			}
		}

		// Token: 0x06001D70 RID: 7536 RVA: 0x0001A66B File Offset: 0x0001886B
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x04001507 RID: 5383
		private static XmlReader reader;

		// Token: 0x04001508 RID: 5384
		public string name;

		// Token: 0x04001509 RID: 5385
		public string fullFolderPath;

		// Token: 0x0400150A RID: 5386
		public XmlDocument xmlDoc;

		// Token: 0x0400150B RID: 5387
		public ModContentPack mod;
	}
}
