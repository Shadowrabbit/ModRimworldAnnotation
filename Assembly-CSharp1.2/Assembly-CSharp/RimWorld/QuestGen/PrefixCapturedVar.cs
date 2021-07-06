using System;
using System.Xml;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001EF8 RID: 7928
	public class PrefixCapturedVar
	{
		// Token: 0x0600A9E3 RID: 43491 RVA: 0x00319EEC File Offset: 0x003180EC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured PrefixCapturedVar: " + xmlRoot.OuterXml, false);
				return;
			}
			this.name = xmlRoot.Name;
			this.value = new SlateRef<object>(DirectXmlToObject.InnerTextWithReplacedNewlinesOrXML(xmlRoot));
			TKeySystem.MarkTreatAsList(xmlRoot.ParentNode);
		}

		// Token: 0x04007336 RID: 29494
		[NoTranslate]
		[TranslationHandle]
		public string name;

		// Token: 0x04007337 RID: 29495
		public SlateRef<object> value;
	}
}
