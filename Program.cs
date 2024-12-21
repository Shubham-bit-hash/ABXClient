


using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        const string host = "localhost";
        const int port = 3000;

        try
        {
            // Connect to the ABX server
            using TcpClient client = new TcpClient(host, port);
            Console.WriteLine("Connected to ABX server.");

            using NetworkStream stream = client.GetStream();

            // Request all packets
            byte[] requestPayload = new byte[2];
            requestPayload[0] = 1; // callType = 1 (Stream All Packets)
            requestPayload[1] = 0; // resendSeq (not used for callType 1)
            stream.Write(requestPayload, 0, requestPayload.Length);

            // Read response packets
            List<Packet> packets = new List<Packet>();
            while (client.Connected && stream.CanRead)
            {
                byte[] buffer = new byte[17]; // Packet size is 17 bytes
                int bytesRead = 0;

                // Ensure we read all 17 bytes
                while (bytesRead < buffer.Length)
                {
                    int bytes = stream.Read(buffer, bytesRead, buffer.Length - bytesRead);
                    if (bytes == 0)
                    {
                        break;
                    }
                    bytesRead += bytes;
                }

                if (bytesRead == buffer.Length)
                {
                    packets.Add(Packet.FromBytes(buffer));
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine($"Received {packets.Count} packets.");

            // Identify missing sequences
            List<int> missingSequences = Packet.GetMissingSequences(packets);
            Console.WriteLine($"Missing sequences: {string.Join(", ", missingSequences)}");

            // Request missing packets
            foreach (int seq in missingSequences)
            {
                requestPayload[0] = 2; // callType = 2 (Resend Packet)
                requestPayload[1] = (byte)seq;
                stream.Write(requestPayload, 0, requestPayload.Length);

                byte[] buffer = new byte[17];
                int bytesRead = 0;

                // Ensure we read all 17 bytes for the missing packet
                while (bytesRead < buffer.Length)
                {
                    int bytes = stream.Read(buffer, bytesRead, buffer.Length - bytesRead);
                    if (bytes == 0)
                    {
                        break;
                    }
                    bytesRead += bytes;
                }

                if (bytesRead == buffer.Length)
                {
                    packets.Add(Packet.FromBytes(buffer));
                }
            }

            // Serialize packets to JSON
            string jsonOutput = JsonSerializer.Serialize(packets, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("output.json", jsonOutput);
            Console.WriteLine("Packets saved to output.json.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

// Define the Packet class
public class Packet
{
    public string Symbol { get; set; }
    public char BuySellIndicator { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
    public int PacketSequence { get; set; }

    public static Packet FromBytes(byte[] data)
    {
        if (data.Length != 17)
            throw new ArgumentException("Invalid packet size.");

        return new Packet
        {
            Symbol = Encoding.ASCII.GetString(data, 0, 4),
            BuySellIndicator = (char)data[4],
            Quantity = BitConverter.ToInt32(data[5..9].Reverse().ToArray(), 0),
            Price = BitConverter.ToInt32(data[9..13].Reverse().ToArray(), 0),
            PacketSequence = BitConverter.ToInt32(data[13..17].Reverse().ToArray(), 0)
        };
    }

    public static List<int> GetMissingSequences(List<Packet> packets)
    {
        packets.Sort((x, y) => x.PacketSequence.CompareTo(y.PacketSequence));
        List<int> missingSequences = new List<int>();

        for (int i = packets[0].PacketSequence; i <= packets[^1].PacketSequence; i++)
        {
            if (!packets.Exists(p => p.PacketSequence == i))
            {
                missingSequences.Add(i);
            }
        }

        return missingSequences;
    }
}







