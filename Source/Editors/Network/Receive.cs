﻿using Lidgren.Network;
using System.Drawing;
using System.Windows.Forms;

partial class Receive
{
    // Pacotes do servidor
    public enum Packets
    {
        Alert,
        Connect,
        Server_Data,
        Classes,
        Tiles,
        Maps,
        Map,
        NPCs,
        Items
    }

    public static void Handle(NetIncomingMessage Data)
    {
        // Manuseia os dados recebidos
        switch ((Packets)Data.ReadByte())
        {
            case Packets.Alert: Alert(Data); break;
            case Packets.Connect: Connect(); break;
            case Packets.Server_Data: Server_Data(Data); break;
            case Packets.Classes: Classes(Data); break;
            case Packets.Maps: Maps(Data); break;
            case Packets.Map: Map(Data); break;
            case Packets.NPCs: NPCs(Data); break;
            case Packets.Items: Items(Data); break;
            case Packets.Tiles: Tiles(Data); break;
        }
    }

    private static void Alert(NetIncomingMessage Data)
    {
        // Mostra a mensagem
        MessageBox.Show(Data.ReadString());
    }

    private static void Connect()
    {
        // Abre a janela de edição
        Login.Objects.Visible = false;
        Selection.Objects.Visible = true;
    }

    public static void Server_Data(NetIncomingMessage Data)
    {
        // Lê os dados
        Lists.Server_Data.Game_Name = Data.ReadString();
        Lists.Server_Data.Welcome = Data.ReadString();
        Lists.Server_Data.Port = Data.ReadInt16();
        Lists.Server_Data.Max_Players = Data.ReadByte();
        Lists.Server_Data.Max_Characters = Data.ReadByte();

        // Abre o editor
        if (Data.ReadBoolean()) Editor_Data.Open();
    }

    public static void Classes(NetIncomingMessage Data)
    {
        // Quantidade de classes
        Lists.Class = new Lists.Structures.Class[Data.ReadByte() + 1];

        for (short i = 1; i < Lists.Class.Length; i++)
        {
            // Redimensiona os valores necessários 
            Lists.Class[i].Vital = new short[(byte)Globals.Vitals.Count];
            Lists.Class[i].Attribute = new short[(byte)Globals.Attributes.Count];

            // Lê os dados
            Lists.Class[i].Name = Data.ReadString();
            Lists.Class[i].Texture_Male = Data.ReadInt16();
            Lists.Class[i].Texture_Female = Data.ReadInt16();
            Lists.Class[i].Spawn_Map = Data.ReadInt16();
            Lists.Class[i].Spawn_Direction = Data.ReadByte();
            Lists.Class[i].Spawn_X = Data.ReadByte();
            Lists.Class[i].Spawn_Y = Data.ReadByte();
            for (byte v = 0; v < (byte)Globals.Vitals.Count; v++) Lists.Class[i].Vital[v] = Data.ReadInt16();
            for (byte a = 0; a < (byte)Globals.Attributes.Count; a++) Lists.Class[i].Attribute[a] = Data.ReadInt16();
        }

        // Abre o editor
        if (Data.ReadBoolean()) Editor_Classes.Open();
    }

    public static void Maps(NetIncomingMessage Data)
    {
        // Quantidade de mapas
        Lists.Map = new Lists.Structures.Map[Data.ReadInt16()];
    }

    public static void Map(NetIncomingMessage Data)
    {
        // Dados básicos
        short i = Data.ReadInt16();
        Lists.Map[i].Revision = Data.ReadInt16();
        Lists.Map[i].Name = Data.ReadString();
        Lists.Map[i].Width = Data.ReadByte();
        Lists.Map[i].Height = Data.ReadByte();
        Lists.Map[i].Moral = Data.ReadByte();
        Lists.Map[i].Panorama = Data.ReadByte();
        Lists.Map[i].Music = Data.ReadByte();
        Lists.Map[i].Color = Data.ReadInt32();
        Lists.Map[i].Weather.Type = Data.ReadByte();
        Lists.Map[i].Weather.Intensity = Data.ReadByte();
        Lists.Map[i].Fog.Texture = Data.ReadByte();
        Lists.Map[i].Fog.Speed_X = Data.ReadSByte();
        Lists.Map[i].Fog.Speed_Y = Data.ReadSByte();
        Lists.Map[i].Fog.Alpha = Data.ReadByte();
        Lists.Map[i].Light_Global = Data.ReadByte();
        Lists.Map[i].Lighting = Data.ReadByte();

        // Ligações
        Lists.Map[i].Link = new short[(short)Globals.Directions.Count];
        for (short n = 0; n < (short)Globals.Directions.Count; n++)
            Lists.Map[i].Link[n] = Data.ReadInt16();

        // Quantidade de camadas
        byte Num_Layers = Data.ReadByte();
        Lists.Map[i].Layer = new System.Collections.Generic.List<Lists.Structures.Map_Layer>();

        // Camadas
        for (byte n = 0; n <= Num_Layers; n++)
        {
            // Dados básicos
            Lists.Map[i].Layer.Add(new Lists.Structures.Map_Layer());
            Lists.Map[i].Layer[n].Name = Data.ReadString();
            Lists.Map[i].Layer[n].Type = Data.ReadByte();

            // Redimensiona os azulejos
            Lists.Map[i].Layer[n].Tile = new Lists.Structures.Map_Tile_Data[Lists.Map[i].Width + 1, Lists.Map[i].Height + 1];

            // Azulejos
            for (byte x = 0; x <= Lists.Map[i].Width; x++)
                for (byte y = 0; y <= Lists.Map[i].Height; y++)
                {
                    Lists.Map[i].Layer[n].Tile[x, y].X = Data.ReadByte();
                    Lists.Map[i].Layer[n].Tile[x, y].Y = Data.ReadByte();
                    Lists.Map[i].Layer[n].Tile[x, y].Tile = Data.ReadByte();
                    Lists.Map[i].Layer[n].Tile[x, y].Auto = Data.ReadBoolean();
                    Lists.Map[i].Layer[n].Tile[x, y].Mini = new Point[4];
                }
        }

        // Dados específicos dos azulejos
        Lists.Map[i].Tile = new Lists.Structures.Map_Tile[Lists.Map[i].Width + 1, Lists.Map[i].Height + 1];
        for (byte x = 0; x <= Lists.Map[i].Width; x++)
            for (byte y = 0; y <= Lists.Map[i].Height; y++)
            {
                Lists.Map[i].Tile[x, y].Attribute = Data.ReadByte();
                Lists.Map[i].Tile[x, y].Data_1 = Data.ReadInt16();
                Lists.Map[i].Tile[x, y].Data_2 = Data.ReadInt16();
                Lists.Map[i].Tile[x, y].Data_3 = Data.ReadInt16();
                Lists.Map[i].Tile[x, y].Data_4 = Data.ReadInt16();
                Lists.Map[i].Tile[x, y].Zone = Data.ReadByte();
                Lists.Map[i].Tile[x, y].Block = new bool[(byte)Globals.Directions.Count];

                for (byte n = 0; n < (byte)Globals.Directions.Count; n++)
                    Lists.Map[i].Tile[x, y].Block[n] = Data.ReadBoolean();
            }

        // Luzes
        byte Num_Lights = Data.ReadByte();
        Lists.Map[i].Light = new System.Collections.Generic.List<Lists.Structures.Map_Light>();
        if (Num_Lights > 0)
            for (byte n = 0; n < Num_Lights; n++)
                Lists.Map[i].Light.Add(new Lists.Structures.Map_Light(new Rectangle(Data.ReadByte(), Data.ReadByte(), Data.ReadByte(), Data.ReadByte())));

        // NPCs
        byte Num_NPCs = Data.ReadByte();
        Lists.Map[i].NPC = new System.Collections.Generic.List<Lists.Structures.Map_NPC>();
        Lists.Structures.Map_NPC NPC = new Lists.Structures.Map_NPC();
        if (Num_NPCs > 0)
            for (byte n = 0; n < Num_NPCs; n++)
            {
                NPC.Index = Data.ReadInt16();
                NPC.Zone = Data.ReadByte();
                NPC.Spawn = Data.ReadBoolean();
                NPC.X = Data.ReadByte();
                NPC.Y = Data.ReadByte();
                Lists.Map[i].NPC.Add(NPC);
            }

        // Abre o editor
        if (Data.ReadBoolean()) Editor_Maps.Open();
    }

    public static void NPCs(NetIncomingMessage Data)
    {
        // Quantidade de nocs
        Lists.NPC = new Lists.Structures.NPC[Data.ReadInt16() + 1];

        for (short i = 1; i < Lists.NPC.Length; i++)
        {
            // Redimensiona os valores necessários 
            Lists.NPC[i].Vital = new short[(byte)Globals.Vitals.Count];
            Lists.NPC[i].Attribute = new short[(byte)Globals.Attributes.Count];
            Lists.NPC[i].Drop = new Lists.Structures.NPC_Drop[Globals.Max_NPC_Drop];

            // Lê os dados
            Lists.NPC[i].Name = Data.ReadString();
            Lists.NPC[i].Texture = Data.ReadInt16();
            Lists.NPC[i].Behaviour = Data.ReadByte();
            for (byte n = 0; n < (byte)Globals.Vitals.Count; n++) Lists.NPC[i].Vital[n] = Data.ReadInt16();
            Lists.NPC[i].SpawnTime = Data.ReadByte();
            Lists.NPC[i].Sight = Data.ReadByte();
            Lists.NPC[i].Experience = Data.ReadByte();
            for (byte n = 0; n < (byte)Globals.Attributes.Count; n++) Lists.NPC[i].Attribute[n] = Data.ReadInt16();
            for (byte n = 0; n < Globals.Max_NPC_Drop; n++)
            {
                Lists.NPC[i].Drop[n].Item_Num = Data.ReadInt16();
                Lists.NPC[i].Drop[n].Amount = Data.ReadInt16();
                Lists.NPC[i].Drop[n].Chance = Data.ReadByte();
            }
        }

        // Abre o editor
        if (Data.ReadBoolean()) Editor_NPCs.Open();
    }

    public static void Items(NetIncomingMessage Data)
    {
        // Quantidade de itens
        Lists.Item = new Lists.Structures.Item[Data.ReadInt16() + 1];

        for (short i = 1; i < Lists.Item.Length; i++)
        {
            // Redimensiona os valores necessários 
            Lists.Item[i].Potion_Vital = new short[(byte)Globals.Vitals.Count];
            Lists.Item[i].Equip_Attribute = new short[(byte)Globals.Attributes.Count];

            // Lê os dados
            Lists.Item[i].Name = Data.ReadString();
            Lists.Item[i].Description = Data.ReadString();
            Lists.Item[i].Texture = Data.ReadInt16();
            Lists.Item[i].Type = Data.ReadByte();
            Lists.Item[i].Price = Data.ReadInt16();
            Lists.Item[i].Stackable = Data.ReadBoolean();
            Lists.Item[i].Bind = Data.ReadBoolean();
            Lists.Item[i].Req_Level = Data.ReadInt16();
            Lists.Item[i].Req_Class = Data.ReadByte();
            Lists.Item[i].Potion_Experience = Data.ReadInt16();
            for (byte v = 0; v < (byte)Globals.Vitals.Count; v++) Lists.Item[i].Potion_Vital[v] = Data.ReadInt16();
            Lists.Item[i].Equip_Type = Data.ReadByte();
            for (byte a = 0; a < (byte)Globals.Attributes.Count; a++) Lists.Item[i].Equip_Attribute[a] = Data.ReadInt16();
            Lists.Item[i].Weapon_Damage = Data.ReadInt16();
        }

        // Abre o editor
        if (Data.ReadBoolean()) Editor_Items.Open();
    }

    public static void Tiles(NetIncomingMessage Data)
    {
        Lists.Tile = new Lists.Structures.Tile[Data.ReadByte()];

        for (byte i = 1; i < Lists.Tile.Length; i++)
        {
            // Dados básicos
            byte Width = Data.ReadByte();
            byte Height = Data.ReadByte();

            // Dados de cada azulejo
            Clear.Tile(i);
            for (byte x = 0; x <= Width; x++)
                for (byte y = 0; y <= Height; y++)
                {
                    // Faz a leitura correta caso alguma textura do azulejo tiver sido redimensionada
                    if (x > Lists.Tile[i].Width || y > Lists.Tile[i].Height)
                    {
                        Data.ReadByte();
                        for (byte d = 0; d < (byte)Globals.Directions.Count; d++) Data.ReadBoolean();
                        continue;
                    }

                    // Atributos
                    Lists.Tile[i].Data[x, y] = new Lists.Structures.Tile_Data();
                    Lists.Tile[i].Data[x, y].Attribute = Data.ReadByte();
                    Lists.Tile[i].Data[x, y].Block = new bool[(byte)Globals.Directions.Count];

                    // Bloqueio direcional
                    for (byte d = 0; d < (byte)Globals.Directions.Count; d++) Lists.Tile[i].Data[x, y].Block[d] = Data.ReadBoolean();
                }
        }

        // Abre o editor
        if (Data.ReadBoolean())
            Editor_Tiles.Open();
    }
}