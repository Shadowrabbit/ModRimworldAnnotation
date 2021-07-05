using System;
using System.Xml;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200163E RID: 5694
	public class PrefixCapturedVar
	{
		// Token: 0x06008520 RID: 34080 RVA: 0x002FD194 File Offset: 0x002FB394
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured PrefixCapturedVar: " + xmlRoot.OuterXml);
				return;
			}
			this.name = xmlRoot.Name;
			this.value = new SlateRef<object>(DirectXmlToObject.InnerTextWithReplacedNewlinesOrXML(xmlRoot));
			TKeySystem.MarkTreatAsList(xmlRoot.ParentNode);
		}

		// Token: 0x040052EC RID: 21228
		[NoTranslate]
		[TranslationHandle]
		public string name;

		// Token: 0x040052ED RID: 21229
		public SlateRef<object> value;
	}
}
