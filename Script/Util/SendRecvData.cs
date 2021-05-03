using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SendData
{
	public BinaryWriter m_binWriter = new BinaryWriter(new MemoryStream());

	public Stream Stream { get { return m_binWriter.BaseStream; } }

	public void Write(double value) { m_binWriter.Write(value); }
	public void Write(ulong value) { m_binWriter.Write(value); }
	public void Write(uint value) { m_binWriter.Write(value); }
	public void Write(ushort value) { m_binWriter.Write(value); }
	public void Write(string value) { m_binWriter.Write(value); }
	public void Write(float value) { m_binWriter.Write(value); }
	public void Write(sbyte value) { m_binWriter.Write(value); }
	public void Write(long value) { m_binWriter.Write(value); }
	public void Write(int value) { m_binWriter.Write(value); }
	public void Write(char[] chars) { m_binWriter.Write(chars); }
	public void Write(decimal value) { m_binWriter.Write(value); }
	public void Write(char[] chars, int index, int count) { m_binWriter.Write(chars, index, count); }
	public void Write(char ch) { m_binWriter.Write(ch); }
	public void Write(byte[] buffer, int index, int count) { m_binWriter.Write(buffer, index, count); }
	public void Write(byte[] buffer) { m_binWriter.Write(buffer); }
	public void Write(byte value) { m_binWriter.Write(value); }
	public void Write(bool value) { m_binWriter.Write(value); }
	public void Write(short value) { m_binWriter.Write(value); }
}

public class RecvData
{
	private BinaryReader m_binReader;

	public RecvData(BinaryReader binReader)
	{
		m_binReader = binReader;
	}

	public int PeekChar() { return m_binReader.PeekChar(); }
	public int Read() { return m_binReader.Read(); }
	public int Read(byte[] buffer, int index, int count) { return m_binReader.Read(buffer, index, count); }
	public int Read(char[] buffer, int index, int count) { return m_binReader.Read(buffer, index, count); }
	public bool ReadBoolean() { return m_binReader.ReadBoolean(); }
	public byte ReadByte() { return m_binReader.ReadByte(); }
	public byte[] ReadBytes(int count) { return m_binReader.ReadBytes(count); }
	public char ReadChar() { return m_binReader.ReadChar(); }
	public char[] ReadChars(int count) { return m_binReader.ReadChars(count); }
	public decimal ReadDecimal() { return m_binReader.ReadDecimal(); }
	public double ReadDouble() { return m_binReader.ReadDouble(); }
	public short ReadInt16() { return m_binReader.ReadInt16(); }
	public int ReadInt32() { return m_binReader.ReadInt32(); }
	public long ReadInt64() { return m_binReader.ReadInt64(); }
	public sbyte ReadSByte() { return m_binReader.ReadSByte(); }
	public float ReadSingle() { return m_binReader.ReadSingle(); }
	public string ReadString() { return m_binReader.ReadString(); }
	public ushort ReadUInt16() { return m_binReader.ReadUInt16(); }
	public uint ReadUInt32() { return m_binReader.ReadUInt32(); }
	public ulong ReadUInt64() { return m_binReader.ReadUInt64(); }
	public void Recycle() { m_binReader.BaseStream.Seek(6, SeekOrigin.Begin); }
}