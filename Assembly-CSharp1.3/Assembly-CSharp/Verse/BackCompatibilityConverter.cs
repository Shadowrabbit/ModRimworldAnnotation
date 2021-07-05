using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000460 RID: 1120
	public abstract class BackCompatibilityConverter
	{
		// Token: 0x0600220A RID: 8714
		public abstract bool AppliesToVersion(int majorVer, int minorVer);

		// Token: 0x0600220B RID: 8715
		public abstract string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null);

		// Token: 0x0600220C RID: 8716
		public abstract Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node);

		// Token: 0x0600220D RID: 8717 RVA: 0x000D491C File Offset: 0x000D2B1C
		public virtual int GetBackCompatibleBodyPartIndex(BodyDef body, int index)
		{
			return index;
		}

		// Token: 0x0600220E RID: 8718
		public abstract void PostExposeData(object obj);

		// Token: 0x0600220F RID: 8719 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostCouldntLoadDef(string defName)
		{
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PreLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x0000313F File Offset: 0x0000133F
		public virtual void PostLoadSavegame(string loadingVersion)
		{
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x000D491F File Offset: 0x000D2B1F
		public bool AppliesToLoadedGameVersion(bool allowInactiveScribe = false)
		{
			return !ScribeMetaHeaderUtility.loadedGameVersion.NullOrEmpty() && (allowInactiveScribe || Scribe.mode != LoadSaveMode.Inactive) && this.AppliesToVersion(VersionControl.MajorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion), VersionControl.MinorFromVersionString(ScribeMetaHeaderUtility.loadedGameVersion));
		}
	}
}
