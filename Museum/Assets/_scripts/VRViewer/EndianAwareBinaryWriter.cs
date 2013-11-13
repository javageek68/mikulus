using System;
using System.IO;

namespace CustomClasses.IO
{
	public class EndianAwareBinaryWriter : BinaryWriter
	{
		protected EndianAwareBitConverter bitConverter;
		
		public EndianAwareBinaryWriter (Stream output, bool littleEndian)
			: base (output)
		{
			bitConverter = new EndianAwareBitConverter (littleEndian);
		}
		
		public EndianAwareBinaryWriter (Stream output, System.Text.Encoding encoding, bool littleEndian)
			: base (output, encoding)
		{
			bitConverter = new EndianAwareBitConverter (littleEndian);
		}
		
		public bool TargetIsLittleEndian
		{
			get { return bitConverter.TargetIsLittleEndian; }
			set { bitConverter.TargetIsLittleEndian = value; }
		}
		
		public override void Write (char ch)
		{
			byte[] buffer = bitConverter.GetBytes (ch);
			Write (buffer, 0, 2);
		}

		public override void Write (char[] chars, int startIndex, int count /*chars*/)
		{
			byte[] buffer = bitConverter.GetBytes  (chars, startIndex, count);
			Write (buffer, 0, buffer.Length);
		}

		public override void Write (short value)
		{
			byte[] buffer = bitConverter.GetBytes (value);
			Write (buffer, 0, 2);
		}

		public override void Write (int value)
		{
			byte[] buffer = bitConverter.GetBytes (value);
			Write (buffer, 0, 4);
		}
		
		public override void Write (long value)
		{
			byte[] buffer = bitConverter.GetBytes (value);
			Write (buffer, 0, 8);
		}
		
		public override void Write (ushort value)
		{
			byte[] buffer = bitConverter.GetBytes (value);
			Write (buffer, 0, 2);
		}
		
		public override void Write (uint value)
		{
			byte[] buffer = bitConverter.GetBytes (value);
			Write (buffer, 0, 4);
		}
		
		public override void Write (ulong value)
		{
			byte[] buffer = bitConverter.GetBytes (value);
			Write (buffer, 0, 8);
		}
		
		public override void Write (float value)
		{
			byte[] buffer = bitConverter.GetBytes (value);
			Write (buffer, 0, 4);
		}

		public override void Write (double value)
		{
			byte[] buffer = bitConverter.GetBytes (value);
			Write (buffer, 0, 8);
		}
		
	}
}
