using System;
using System.Reflection;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C42 RID: 7234
	public static class DefOfHelper
	{
		// Token: 0x06009F43 RID: 40771 RVA: 0x002EA67C File Offset: 0x002E887C
		public static void RebindAllDefOfs(bool earlyTryMode)
		{
			DefOfHelper.earlyTry = earlyTryMode;
			DefOfHelper.bindingNow = true;
			try
			{
				foreach (Type type in GenTypes.AllTypesWithAttribute<DefOf>())
				{
					DefOfHelper.BindDefsFor(type);
				}
			}
			finally
			{
				DefOfHelper.bindingNow = false;
			}
		}

		// Token: 0x06009F44 RID: 40772 RVA: 0x002EA6E4 File Offset: 0x002E88E4
		private static void BindDefsFor(Type type)
		{
			foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Static | BindingFlags.Public))
			{
				Type fieldType = fieldInfo.FieldType;
				if (!typeof(Def).IsAssignableFrom(fieldType))
				{
					Log.Error(fieldType + " is not a Def.", false);
				}
				else
				{
					MayRequireAttribute mayRequireAttribute = fieldInfo.TryGetAttribute<MayRequireAttribute>();
					bool flag = (mayRequireAttribute == null || ModsConfig.IsActive(mayRequireAttribute.modId)) && !DefOfHelper.earlyTry;
					string text = fieldInfo.Name;
					DefAliasAttribute defAliasAttribute = fieldInfo.TryGetAttribute<DefAliasAttribute>();
					if (defAliasAttribute != null)
					{
						text = defAliasAttribute.defName;
					}
					if (fieldType == typeof(SoundDef))
					{
						SoundDef soundDef = SoundDef.Named(text);
						if (soundDef.isUndefined && flag)
						{
							Log.Error("Could not find SoundDef named " + text, false);
						}
						fieldInfo.SetValue(null, soundDef);
					}
					else
					{
						Def def = GenDefDatabase.GetDef(fieldType, text, flag);
						fieldInfo.SetValue(null, def);
					}
				}
			}
		}

		// Token: 0x06009F45 RID: 40773 RVA: 0x002EA7E0 File Offset: 0x002E89E0
		public static void EnsureInitializedInCtor(Type defOf)
		{
			if (!DefOfHelper.bindingNow)
			{
				string text;
				if (DirectXmlToObject.currentlyInstantiatingObjectOfType.Any<Type>())
				{
					text = "DirectXmlToObject is currently instantiating an object of type " + DirectXmlToObject.currentlyInstantiatingObjectOfType.Peek();
				}
				else if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					text = "curParent=" + Scribe.loader.curParent.ToStringSafe<IExposable>() + " curPathRelToParent=" + Scribe.loader.curPathRelToParent;
				}
				else
				{
					text = "";
				}
				Log.Warning("Tried to use an uninitialized DefOf of type " + defOf.Name + ". DefOfs are initialized right after all defs all loaded. Uninitialized DefOfs will return only nulls. (hint: don't use DefOfs as default field values in Defs, try to resolve them in ResolveReferences() instead)" + (text.NullOrEmpty() ? "" : (" Debug info: " + text)), false);
			}
			if (typeof(Def).IsAssignableFrom(defOf))
			{
				Log.Warning("Possible typo: " + defOf.Name + ". Using def type name not preceded by \"Of\"", false);
			}
		}

		// Token: 0x04006574 RID: 25972
		private static bool bindingNow;

		// Token: 0x04006575 RID: 25973
		private static bool earlyTry = true;
	}
}
