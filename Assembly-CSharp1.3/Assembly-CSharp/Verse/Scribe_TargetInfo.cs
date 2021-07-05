using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000335 RID: 821
	public static class Scribe_TargetInfo
	{
		// Token: 0x06001747 RID: 5959 RVA: 0x0008ADAD File Offset: 0x00088FAD
		public static void Look(ref LocalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, LocalTargetInfo.Invalid, false);
		}

		// Token: 0x06001748 RID: 5960 RVA: 0x0008ADBD File Offset: 0x00088FBD
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, LocalTargetInfo.Invalid, true);
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x0008ADCD File Offset: 0x00088FCD
		public static void Look(ref LocalTargetInfo value, string label, LocalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x0008ADDC File Offset: 0x00088FDC
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label, LocalTargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.LocalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveLocalTargetInfo(value, label);
				}
			}
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x0008AE70 File Offset: 0x00089070
		public static void Look(ref TargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, TargetInfo.Invalid, false);
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x0008AE80 File Offset: 0x00089080
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, TargetInfo.Invalid, true);
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x0008AE90 File Offset: 0x00089090
		public static void Look(ref TargetInfo value, string label, TargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0008AE9C File Offset: 0x0008909C
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label, TargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.TargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveTargetInfo(value, label);
				}
			}
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x0008AF79 File Offset: 0x00089179
		public static void Look(ref GlobalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, GlobalTargetInfo.Invalid, false);
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x0008AF89 File Offset: 0x00089189
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, GlobalTargetInfo.Invalid, true);
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x0008AF99 File Offset: 0x00089199
		public static void Look(ref GlobalTargetInfo value, string label, GlobalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x0008AFA8 File Offset: 0x000891A8
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label, GlobalTargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (value.WorldObject != null && Scribe_References.CheckSaveReferenceToDestroyedWorldObject(value.WorldObject, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && !value.HasWorldObject && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.GlobalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveGlobalTargetInfo(value, label);
				}
			}
		}
	}
}
