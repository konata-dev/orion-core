﻿using System.IO;

namespace Orion.Net.Packets
{
	/// <summary>
	/// Base class from which all packet types should inherit.
	/// </summary>
	public class TerrariaPacket
	{
		/// <summary>
		/// Packet length.
		/// </summary>
		private short _length;

		/// <summary>
		/// Packet ID.
		/// </summary>
		public byte id;

		/// <summary>
		/// Whether or not this packet has been handled.
		/// </summary>
		public bool handled;

		/// <summary>
		/// Whether or not this packet has had its data modified.
		/// </summary>
		public bool hasNewData;

		/// <summary>
		/// Obtains packet data from a BinaryReader.
		/// Used when receiving packets.
		/// </summary>
		/// <param name="reader">The reader object with the data to be read.</param>
		internal TerrariaPacket(BinaryReader reader)
		{
			_length = reader.ReadInt16();
			id = reader.ReadByte();
		}

		/// <summary>
		/// Creates a packet based on it's ID.
		/// Used when sending packets.
		/// </summary>
		/// <param name="id">The packet id.</param>
		internal TerrariaPacket(byte id)
		{
			this.id = id;
		}

		/// <summary>
		/// Creates a packet based on it's ID.
		/// Used when sending packets.
		/// </summary>
		/// <param name="id">The packet id.</param>
		internal TerrariaPacket(PacketTypes id)
			:this((byte)id)
		{
			
		}

		/// <summary>
		/// Override this method to change the data in a packet before it is sent or received.
		/// </summary>
		/// <param name="e">The SendDataEventArgs to change.</param>
		internal virtual void SetNewData(ref TerrariaApi.Server.SendDataEventArgs e)
		{

		}
	}
}
