using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD2 RID: 3026
	public struct SignalArgs
	{
		// Token: 0x17000C6D RID: 3181
		// (get) Token: 0x06004706 RID: 18182 RVA: 0x00177C15 File Offset: 0x00175E15
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		// Token: 0x17000C6E RID: 3182
		// (get) Token: 0x06004707 RID: 18183 RVA: 0x00177C1D File Offset: 0x00175E1D
		public IEnumerable<NamedArgument> Args
		{
			get
			{
				if (this.count == 0)
				{
					yield break;
				}
				if (this.args != null)
				{
					int num;
					for (int i = 0; i < this.args.Length; i = num + 1)
					{
						yield return this.args[i];
						num = i;
					}
				}
				else
				{
					yield return this.arg1;
					if (this.count >= 2)
					{
						yield return this.arg2;
					}
					if (this.count >= 3)
					{
						yield return this.arg3;
					}
					if (this.count >= 4)
					{
						yield return this.arg4;
					}
				}
				yield break;
			}
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x00177C34 File Offset: 0x00175E34
		public SignalArgs(SignalArgs args)
		{
			this.count = args.count;
			this.arg1 = args.arg1;
			this.arg2 = args.arg2;
			this.arg3 = args.arg3;
			this.arg4 = args.arg4;
			this.args = args.args;
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x00177C89 File Offset: 0x00175E89
		public SignalArgs(NamedArgument arg1)
		{
			this.count = 1;
			this.arg1 = arg1;
			this.arg2 = default(NamedArgument);
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x00177CC4 File Offset: 0x00175EC4
		public SignalArgs(NamedArgument arg1, NamedArgument arg2)
		{
			this.count = 2;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x00177CFA File Offset: 0x00175EFA
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.count = 3;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x00177D2B File Offset: 0x00175F2B
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.count = 4;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = arg4;
			this.args = null;
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x00177D58 File Offset: 0x00175F58
		public SignalArgs(params NamedArgument[] args)
		{
			this.count = args.Length;
			if (args.Length > 4)
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
				this.args = new NamedArgument[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					this.args[i] = args[i];
				}
				return;
			}
			if (args.Length == 1)
			{
				this.arg1 = args[0];
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 2)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 3)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 4)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = args[3];
			}
			else
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.args = null;
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x00177EFC File Offset: 0x001760FC
		public bool TryGetArg(int index, out NamedArgument arg)
		{
			if (index < 0 || index >= this.count)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				arg = this.args[index];
			}
			else if (index == 0)
			{
				arg = this.arg1;
			}
			else if (index == 1)
			{
				arg = this.arg2;
			}
			else if (index == 2)
			{
				arg = this.arg3;
			}
			else
			{
				arg = this.arg4;
			}
			return true;
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x00177F80 File Offset: 0x00176180
		public bool TryGetArg(string name, out NamedArgument arg)
		{
			if (this.count == 0)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				for (int i = 0; i < this.args.Length; i++)
				{
					if (this.args[i].label == name)
					{
						arg = this.args[i];
						return true;
					}
				}
			}
			else
			{
				if (this.count >= 1 && this.arg1.label == name)
				{
					arg = this.arg1;
					return true;
				}
				if (this.count >= 2 && this.arg2.label == name)
				{
					arg = this.arg2;
					return true;
				}
				if (this.count >= 3 && this.arg3.label == name)
				{
					arg = this.arg3;
					return true;
				}
				if (this.count >= 4 && this.arg4.label == name)
				{
					arg = this.arg4;
					return true;
				}
			}
			arg = default(NamedArgument);
			return false;
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x0017809C File Offset: 0x0017629C
		public bool TryGetArg<T>(string name, out T arg)
		{
			NamedArgument namedArgument;
			if (!this.TryGetArg(name, out namedArgument) || !(namedArgument.arg is T))
			{
				arg = default(T);
				return false;
			}
			arg = (T)((object)namedArgument.arg);
			return true;
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x001780DC File Offset: 0x001762DC
		public NamedArgument GetArg(int index)
		{
			NamedArgument result;
			if (this.TryGetArg(index, out result))
			{
				return result;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x00178100 File Offset: 0x00176300
		public NamedArgument GetArg(string name)
		{
			NamedArgument result;
			if (this.TryGetArg(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name);
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x0017812C File Offset: 0x0017632C
		public T GetArg<T>(string name)
		{
			T result;
			if (this.TryGetArg<T>(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name + " of type " + typeof(T).Name);
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x0017816C File Offset: 0x0017636C
		public TaggedString GetFormattedText(TaggedString text)
		{
			if (this.count == 0)
			{
				return text.Formatted(Array.Empty<NamedArgument>());
			}
			if (this.args != null)
			{
				return text.Formatted(this.args);
			}
			if (this.count == 1)
			{
				return text.Formatted(this.arg1);
			}
			if (this.count == 2)
			{
				return text.Formatted(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return text.Formatted(this.arg1, this.arg2, this.arg3);
			}
			return text.Formatted(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x00178214 File Offset: 0x00176414
		public TaggedString GetTranslatedText(string textKey)
		{
			if (this.count == 0)
			{
				return textKey.Translate();
			}
			if (this.args != null)
			{
				return textKey.Translate(this.args);
			}
			if (this.count == 1)
			{
				return textKey.Translate(this.arg1);
			}
			if (this.count == 2)
			{
				return textKey.Translate(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return textKey.Translate(this.arg1, this.arg2, this.arg3);
			}
			return textKey.Translate(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x001782B8 File Offset: 0x001764B8
		public void Add(NamedArgument arg)
		{
			if (this.args != null)
			{
				NamedArgument[] array = new NamedArgument[this.args.Length + 1];
				for (int i = 0; i < this.args.Length; i++)
				{
					array[i] = this.args[i];
				}
				array[array.Length - 1] = arg;
				this.args = array;
				this.count = this.args.Length;
				return;
			}
			if (this.count == 0)
			{
				this.arg1 = arg;
			}
			else if (this.count == 1)
			{
				this.arg2 = arg;
			}
			else if (this.count == 2)
			{
				this.arg3 = arg;
			}
			else if (this.count == 3)
			{
				this.arg4 = arg;
			}
			else
			{
				this.args = new NamedArgument[5];
				this.args[0] = this.arg1;
				this.args[1] = this.arg2;
				this.args[2] = this.arg3;
				this.args[3] = this.arg4;
				this.args[4] = arg;
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.count++;
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x00178417 File Offset: 0x00176617
		public void Add(NamedArgument arg1, NamedArgument arg2)
		{
			this.Add(arg1);
			this.Add(arg2);
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x00178427 File Offset: 0x00176627
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x0017843E File Offset: 0x0017663E
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
			this.Add(arg4);
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x00178460 File Offset: 0x00176660
		public void Add(params NamedArgument[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				this.Add(args[i]);
			}
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x00178488 File Offset: 0x00176688
		public void Add(SignalArgs args)
		{
			if (args.count == 0)
			{
				return;
			}
			if (args.args != null)
			{
				for (int i = 0; i < args.args.Length; i++)
				{
					this.Add(args.args[i]);
				}
				return;
			}
			if (args.count >= 1)
			{
				this.Add(args.arg1);
			}
			if (args.count >= 2)
			{
				this.Add(args.arg2);
			}
			if (args.count >= 3)
			{
				this.Add(args.arg3);
			}
			if (args.count >= 4)
			{
				this.Add(args.arg4);
			}
		}

		// Token: 0x04002B84 RID: 11140
		private int count;

		// Token: 0x04002B85 RID: 11141
		private NamedArgument arg1;

		// Token: 0x04002B86 RID: 11142
		private NamedArgument arg2;

		// Token: 0x04002B87 RID: 11143
		private NamedArgument arg3;

		// Token: 0x04002B88 RID: 11144
		private NamedArgument arg4;

		// Token: 0x04002B89 RID: 11145
		private NamedArgument[] args;
	}
}
