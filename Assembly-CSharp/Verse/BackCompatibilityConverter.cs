using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020007B3 RID: 1971
	public abstract class BackCompatibilityConverter
	{
		// Token: 0x060031C0 RID: 12736
		public abstract bool AppliesToVersion(int majorVer, int minorVer);

		// Token: 0x060031C1 RID: 12737
		public abstract string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null);

		// Token: 0x060031C2 RID: 12738
		public abstract Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node);

		// Token: 0x060031C3 RID: 12739 RVA: 0x00020556 File Offset: 0x0001E756
		public virtual int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			return index;
		}

		// Token: 0x060031C4 RID: 12740
		public abstract void PostExposeData(object obj);

		// Token: 0x060031C5 RID: 12741 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostCouldntLoadDef(string defName)
		{
		}

		// Token: 0x060031C6 RID: 12742 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PreLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x060031C7 RID: 12743 RVA: 0x00006A05 File Offset: 0x00004C05
		public virtual void PostLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x060031C8 RID: 12744 RVA: 0x000271D5 File Offset: 0x000253D5
		public bool AppliesToLoadedGameVersion(bool allowInactiveScribe = false)
		{
			return !ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() && (allowInactiveScribe || Scribe.mode != LoadSaveMode.Inactive) && this.AppliesToVersion(VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion), VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion));
		}
	}
}
