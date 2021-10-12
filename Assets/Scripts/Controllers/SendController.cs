using UnityEngine;

public class SendController
{
    public static void Welcome(int toClient, string msg)
    {
        using (Packet packet = new Packet((int)ServerPackets.Welcome))
        {
            packet.Write(msg);
            packet.Write(toClient);

            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.SpawnPlayer))
        {
            packet.Write(player.Id);
            packet.Write(player.Username);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);

            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerPosition(int id, Vector3 position)
    {
        using (Packet packet = new Packet((int)ServerPackets.PlayerPosition))
        {
            packet.Write(id);
            packet.Write(position);

            SendUdpDataToAll(packet);
        }
    }

    public static void PlayerRotation(int id, Quaternion rotation, Vector3 eulerAngles)
    {
        using (Packet packet = new Packet((int)ServerPackets.PlayerRotation))
        {
            packet.Write(id);
            packet.Write(rotation);
            packet.Write(eulerAngles);

            SendUdpDataToAll(id, packet);
        }
    }

    #region TCP

    private static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();

        Server.Clients[toClient].Tcp.SendData(packet);
    }

    private static void SendTCPDataToAll(Packet packet)
    {
        packet.WriteLength();

        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.Clients[i].Tcp.SendData(packet);
        }
    }

    private static void SendTCPDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();

        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
                Server.Clients[i].Tcp.SendData(packet);
        }
    }

    #endregion

    #region UDP

    private static void SendUDPData(int toClient, Packet packet)
    {
        packet.WriteLength();

        Server.Clients[toClient].Udp.SendData(packet);
    }

    private static void SendUdpDataToAll(Packet packet)
    {
        packet.WriteLength();

        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.Clients[i].Udp.SendData(packet);
        }
    }

    private static void SendUdpDataToAll(int exceptClient, Packet packet)
    {
        packet.WriteLength();

        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
                Server.Clients[i].Udp.SendData(packet);
        }
    }

    #endregion
}
