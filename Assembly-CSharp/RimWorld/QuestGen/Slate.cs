using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001FFF RID: 8191
	public class Slate
	{
		// Token: 0x17001986 RID: 6534
		// (get) Token: 0x0600AD7C RID: 44412 RVA: 0x00070ECC File Offset: 0x0006F0CC
		public string CurrentPrefix
		{
			get
			{
				return this.prefix;
			}
		}

		// Token: 0x0600AD7D RID: 44413 RVA: 0x003281DC File Offset: 0x003263DC
		public T Get<T>(string name, T defaultValue = default(T), bool isAbsoluteName = false)
		{
			T result;
			if (this.TryGet<T>(name, out result, isAbsoluteName))
			{
				return result;
			}
			return defaultValue;
		}

		// Token: 0x0600AD7E RID: 44414 RVA: 0x003281F8 File Offset: 0x003263F8
		public bool TryGet<T>(string name, out T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				var = default(T);
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			object obj;
			if (!this.vars.TryGetValue(name, out obj))
			{
				var = default(T);
				return false;
			}
			if (obj == null)
			{
				var = default(T);
				return true;
			}
			if (obj is T)
			{
				var = (T)((object)obj);
				return true;
			}
			if (ConvertHelper.CanConvert<T>(obj))
			{
				var = ConvertHelper.Convert<T>(obj);
				return true;
			}
			Log.Error(string.Concat(new string[]
			{
				"Could not convert slate variable \"",
				name,
				"\" (",
				obj.GetType().Name,
				") to ",
				typeof(T).Name
			}), false);
			var = default(T);
			return false;
		}

		// Token: 0x0600AD7F RID: 44415 RVA: 0x003282F8 File Offset: 0x003264F8
		public void Set<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				Log.Error("Tried to set a variable with null name. var=" + var.ToStringSafe<T>(), false);
				return;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			ISlateRef slateRef = var as ISlateRef;
			if (slateRef != null)
			{
				object value;
				slateRef.TryGetConvertedValue<object>(this, out value);
				this.vars[name] = value;
				return;
			}
			this.vars[name] = var;
		}

		// Token: 0x0600AD80 RID: 44416 RVA: 0x00070ED4 File Offset: 0x0006F0D4
		public void SetIfNone<T>(string name, T var, bool isAbsoluteName = false)
		{
			if (this.Exists(name, isAbsoluteName))
			{
				return;
			}
			this.Set<T>(name, var, isAbsoluteName);
		}

		// Token: 0x0600AD81 RID: 44417 RVA: 0x00328388 File Offset: 0x00326588
		public bool Remove(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			return this.vars.Remove(name);
		}

		// Token: 0x0600AD82 RID: 44418 RVA: 0x003283D8 File Offset: 0x003265D8
		public bool Exists(string name, bool isAbsoluteName = false)
		{
			if (name.NullOrEmpty())
			{
				return false;
			}
			if (!isAbsoluteName && !this.prefix.NullOrEmpty())
			{
				name = this.prefix + "/" + name;
			}
			name = QuestGenUtility.NormalizeVarPath(name);
			if (this.allowNonPrefixedLookup)
			{
				name = this.TryResolveFirstAvailableName(name);
			}
			return this.vars.ContainsKey(name);
		}

		// Token: 0x0600AD83 RID: 44419 RVA: 0x00328438 File Offset: 0x00326638
		private string TryResolveFirstAvailableName(string nameWithPrefix)
		{
			if (nameWithPrefix == null)
			{
				return null;
			}
			nameWithPrefix = QuestGenUtility.NormalizeVarPath(nameWithPrefix);
			if (this.vars.ContainsKey(nameWithPrefix))
			{
				return nameWithPrefix;
			}
			int num = nameWithPrefix.LastIndexOf('/');
			if (num < 0)
			{
				return nameWithPrefix;
			}
			string text = nameWithPrefix.Substring(num + 1);
			string text2 = nameWithPrefix.Substring(0, num);
			string text3;
			for (;;)
			{
				text3 = text;
				if (!text2.NullOrEmpty())
				{
					text3 = text2 + "/" + text3;
				}
				if (this.vars.ContainsKey(text3))
				{
					break;
				}
				if (text2.NullOrEmpty())
				{
					return nameWithPrefix;
				}
				int num2 = text2.LastIndexOf('/');
				if (num2 >= 0)
				{
					text2 = text2.Substring(0, num2);
				}
				else
				{
					text2 = "";
				}
			}
			return text3;
		}

		// Token: 0x0600AD84 RID: 44420 RVA: 0x003284D8 File Offset: 0x003266D8
		public void PushPrefix(string newPrefix, bool allowNonPrefixedLookup = false)
		{
			if (newPrefix.NullOrEmpty())
			{
				Log.Error("Tried to push a null prefix.", false);
				newPrefix = "unnamed";
			}
			if (!this.prefix.NullOrEmpty())
			{
				this.prefix += "/";
			}
			this.prefix += newPrefix;
			this.prevAllowNonPrefixedLookupStack.Push(this.allowNonPrefixedLookup);
			if (allowNonPrefixedLookup)
			{
				this.allowNonPrefixedLookup = true;
			}
		}

		// Token: 0x0600AD85 RID: 44421 RVA: 0x00328550 File Offset: 0x00326750
		public void PopPrefix()
		{
			int num = this.prefix.LastIndexOf('/');
			if (num >= 0)
			{
				this.prefix = this.prefix.Substring(0, num);
			}
			else
			{
				this.prefix = "";
			}
			if (this.prevAllowNonPrefixedLookupStack.Count != 0)
			{
				this.allowNonPrefixedLookup = this.prevAllowNonPrefixedLookupStack.Pop();
			}
		}

		// Token: 0x0600AD86 RID: 44422 RVA: 0x003285B0 File Offset: 0x003267B0
		public Slate.VarRestoreInfo GetRestoreInfo(string name)
		{
			bool flag = this.allowNonPrefixedLookup;
			this.allowNonPrefixedLookup = false;
			Slate.VarRestoreInfo result;
			try
			{
				object value;
				bool exists = this.TryGet<object>(name, out value, false);
				result = new Slate.VarRestoreInfo(name, exists, value);
			}
			finally
			{
				this.allowNonPrefixedLookup = flag;
			}
			return result;
		}

		// Token: 0x0600AD87 RID: 44423 RVA: 0x00070EEA File Offset: 0x0006F0EA
		public void Restore(Slate.VarRestoreInfo varRestoreInfo)
		{
			if (varRestoreInfo.exists)
			{
				this.Set<object>(varRestoreInfo.name, varRestoreInfo.value, false);
				return;
			}
			this.Remove(varRestoreInfo.name, false);
		}

		// Token: 0x0600AD88 RID: 44424 RVA: 0x003285FC File Offset: 0x003267FC
		public void SetAll(Slate otherSlate)
		{
			this.vars.Clear();
			foreach (KeyValuePair<string, object> keyValuePair in otherSlate.vars)
			{
				this.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x0600AD89 RID: 44425 RVA: 0x00070F16 File Offset: 0x0006F116
		public void Reset()
		{
			this.vars.Clear();
		}

		// Token: 0x0600AD8A RID: 44426 RVA: 0x0032866C File Offset: 0x0032686C
		public Slate DeepCopy()
		{
			Slate slate = new Slate();
			slate.prefix = this.prefix;
			foreach (KeyValuePair<string, object> keyValuePair in this.vars)
			{
				slate.vars.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return slate;
		}

		// Token: 0x0600AD8B RID: 44427 RVA: 0x003286E4 File Offset: 0x003268E4
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<string, object> keyValuePair in from x in this.vars
			orderby x.Key
			select x)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				string str = (keyValuePair.Value is IEnumerable && !(keyValuePair.Value is string)) ? ((IEnumerable)keyValuePair.Value).ToStringSafeEnumerable() : keyValuePair.Value.ToStringSafe<object>();
				stringBuilder.Append(keyValuePair.Key + "=" + str);
			}
			if (stringBuilder.Length == 0)
			{
				stringBuilder.Append("(none)");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04007712 RID: 30482
		private Dictionary<string, object> vars = new Dictionary<string, object>();

		// Token: 0x04007713 RID: 30483
		private string prefix = "";

		// Token: 0x04007714 RID: 30484
		private bool allowNonPrefixedLookup;

		// Token: 0x04007715 RID: 30485
		private Stack<bool> prevAllowNonPrefixedLookupStack = new Stack<bool>();

		// Token: 0x04007716 RID: 30486
		public const char Separator = '/';

		// Token: 0x02002000 RID: 8192
		public struct VarRestoreInfo
		{
			// Token: 0x0600AD8D RID: 44429 RVA: 0x00070F4C File Offset: 0x0006F14C
			public VarRestoreInfo(string name, bool exists, object value)
			{
				this.name = name;
				this.exists = exists;
				this.value = value;
			}

			// Token: 0x04007717 RID: 30487
			public string name;

			// Token: 0x04007718 RID: 30488
			public bool exists;

			// Token: 0x04007719 RID: 30489
			public object value;
		}
	}
}
