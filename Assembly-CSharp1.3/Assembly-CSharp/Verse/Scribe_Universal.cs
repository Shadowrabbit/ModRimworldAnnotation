using System;
using System.Reflection;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000336 RID: 822
	public static class Scribe_Universal
	{
		// Token: 0x06001753 RID: 5971 RVA: 0x0008B0A8 File Offset: 0x000892A8
		public static void Look<TAny>(ref TAny obj, string label, LookMode lookMode, ref Type type)
		{
			if (lookMode == LookMode.Undefined)
			{
				Log.Error("Look mode can't be Undefined in Scribe_Universal.Look() if \"ref\" isn't used.");
				return;
			}
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				type = ((obj != null) ? obj.GetType() : typeof(TAny));
			}
			Scribe_Values.Look<Type>(ref type, label + "_type", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && type == null)
			{
				type = typeof(TAny);
			}
			object obj2 = obj;
			Scribe_Universal.LookInt(ref obj2, label, lookMode, type);
			obj = ((obj2 is TAny) ? ((TAny)((object)obj2)) : default(TAny));
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x0008B15C File Offset: 0x0008935C
		public static void Look<TAny>(ref TAny obj, string label, ref LookMode lookMode, ref Type type, bool forceDeepSave = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				type = ((obj != null) ? obj.GetType() : typeof(TAny));
				if (!Scribe_Universal.TryResolveLookMode(type, out lookMode, true, forceDeepSave))
				{
					Log.Error("Scribe_Universal could not resolve look mode for " + obj.ToStringSafe<TAny>());
					return;
				}
			}
			Scribe_Values.Look<LookMode>(ref lookMode, label + "_lookMode", LookMode.Undefined, false);
			Scribe_Values.Look<Type>(ref type, label + "_type", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (type == null)
				{
					type = typeof(TAny);
				}
				if (lookMode == LookMode.Undefined && !Scribe_Universal.TryResolveLookMode(type, out lookMode, true, forceDeepSave))
				{
					Log.Error("Scribe_Universal could not resolve look mode for " + obj.ToStringSafe<TAny>());
					return;
				}
			}
			object obj2 = obj;
			Scribe_Universal.LookInt(ref obj2, label, lookMode, type);
			obj = ((obj2 is TAny) ? ((TAny)((object)obj2)) : default(TAny));
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x0008B268 File Offset: 0x00089468
		private static void LookInt(ref object obj, string label, LookMode lookMode, Type type)
		{
			if (type == typeof(object))
			{
				type = typeof(string);
				lookMode = LookMode.Value;
			}
			string name;
			if (lookMode != LookMode.Reference)
			{
				if (lookMode == LookMode.Def)
				{
					name = "LookInternalDef";
				}
				else
				{
					name = "LookInternalMisc";
				}
			}
			else
			{
				name = "LookInternalReference";
			}
			object[] array = new object[]
			{
				obj,
				label,
				lookMode
			};
			typeof(Scribe_Universal).GetMethod(name, BindingFlags.Static | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
			{
				type
			}).Invoke(null, array);
			obj = array[0];
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x0008B2FC File Offset: 0x000894FC
		private static void LookInternalMisc<T>(ref T obj, string label, LookMode lookMode)
		{
			switch (lookMode)
			{
			case LookMode.Value:
				Scribe_Values.Look<T>(ref obj, label, default(T), false);
				return;
			case LookMode.Deep:
				Scribe_Deep.Look<T>(ref obj, label, Array.Empty<object>());
				return;
			case LookMode.Reference:
			case LookMode.Def:
				break;
			case LookMode.LocalTargetInfo:
			{
				LocalTargetInfo localTargetInfo = (LocalTargetInfo)((object)obj);
				Scribe_TargetInfo.Look(ref localTargetInfo, label);
				obj = (T)((object)localTargetInfo);
				return;
			}
			case LookMode.TargetInfo:
			{
				TargetInfo targetInfo = (TargetInfo)((object)obj);
				Scribe_TargetInfo.Look(ref targetInfo, label);
				obj = (T)((object)targetInfo);
				return;
			}
			case LookMode.GlobalTargetInfo:
			{
				GlobalTargetInfo globalTargetInfo = (GlobalTargetInfo)((object)obj);
				Scribe_TargetInfo.Look(ref globalTargetInfo, label);
				obj = (T)((object)globalTargetInfo);
				return;
			}
			case LookMode.BodyPart:
			{
				BodyPartRecord bodyPartRecord = (BodyPartRecord)((object)obj);
				Scribe_BodyParts.Look(ref bodyPartRecord, label, null);
				obj = (T)((object)bodyPartRecord);
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x0008B3FA File Offset: 0x000895FA
		private static void LookInternalDef<T>(ref T obj, string label, LookMode lookMode) where T : Def, new()
		{
			Scribe_Defs.Look<T>(ref obj, label);
		}

		// Token: 0x06001758 RID: 5976 RVA: 0x0008B403 File Offset: 0x00089603
		private static void LookInternalReference<T>(ref T obj, string label, LookMode lookMode) where T : ILoadReferenceable
		{
			Scribe_References.Look<T>(ref obj, label, false);
		}

		// Token: 0x06001759 RID: 5977 RVA: 0x0008B410 File Offset: 0x00089610
		public static bool TryResolveLookMode(Type type, out LookMode lookMode, bool desperate = false, bool preferDeepIfDesperateAndAmbiguous = false)
		{
			if (type == null)
			{
				if (desperate)
				{
					lookMode = LookMode.Value;
					return true;
				}
				lookMode = LookMode.Undefined;
				return false;
			}
			else
			{
				if (type == typeof(object) && desperate)
				{
					lookMode = LookMode.Value;
					return true;
				}
				if (ParseHelper.HandlesType(type))
				{
					lookMode = LookMode.Value;
					return true;
				}
				if (type == typeof(LocalTargetInfo))
				{
					lookMode = LookMode.LocalTargetInfo;
					return true;
				}
				if (type == typeof(TargetInfo))
				{
					lookMode = LookMode.TargetInfo;
					return true;
				}
				if (type == typeof(GlobalTargetInfo))
				{
					lookMode = LookMode.GlobalTargetInfo;
					return true;
				}
				if (typeof(Def).IsAssignableFrom(type))
				{
					lookMode = LookMode.Def;
					return true;
				}
				if (type == typeof(BodyPartRecord))
				{
					lookMode = LookMode.BodyPart;
					return true;
				}
				if (typeof(IExposable).IsAssignableFrom(type) && !typeof(ILoadReferenceable).IsAssignableFrom(type))
				{
					lookMode = LookMode.Deep;
					return true;
				}
				if (desperate && typeof(ILoadReferenceable).IsAssignableFrom(type))
				{
					if (preferDeepIfDesperateAndAmbiguous)
					{
						lookMode = LookMode.Deep;
					}
					else
					{
						lookMode = LookMode.Reference;
					}
					return true;
				}
				lookMode = LookMode.Undefined;
				return false;
			}
		}

		// Token: 0x04001025 RID: 4133
		private const string LookInternalMiscName = "LookInternalMisc";

		// Token: 0x04001026 RID: 4134
		private const string LookInternalDefName = "LookInternalDef";

		// Token: 0x04001027 RID: 4135
		private const string LookInternalReferenceName = "LookInternalReference";
	}
}
