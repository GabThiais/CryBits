﻿using System;

class Lists
{
    // Armazenamento de dados
    public static Structures.Options Options = new Structures.Options();
    public static Structures.Server_Data Server_Data = new Structures.Server_Data();
    public static Structures.Player[] Player;
    public static Structures.Class[] Class;
    public static Structures.Characters[] Characters;
    public static Structures.Maps Map;
    public static Structures.Temp_Map Temp_Map;
    public static Structures.Weather[] Weather;
    public static Structures.NPCs[] NPC;
    public static Structures.Items[] Item;

    // Estrutura dos itens em gerais
    public class Structures
    {
        public struct Options
        {
            public string GameName;
            public bool SaveUsername;
            public bool Sounds;
            public bool Musics;
            public string Username;
        }

        public struct Server_Data
        {
            public byte Max_Characters;
        }

        public class Player
        {
            // Apenas na parte do cliente
            public short X2;
            public short Y2;
            public byte Animation;
            public bool Attacking;
            public int Attack_Timer;
            public int Hurt;
            public short[] Max_Vital;
            public int Collect_Timer;
            // Geral
            public string Name;
            public byte Class;
            public bool Genre;
            public short Level;
            public short Experience;
            public short ExpNeeded;
            public short Points;
            public short[] Vital;
            public short[] Attribute;
            public short Map;
            public byte X;
            public byte Y;
            public Game.Directions Direction;
            public Game.Movements Movement;
            public short[] Equipment;
        }

        public class Characters
        {
            public string Name;
            public byte Class;
            public bool Genre;
            public short Level;
        }

        public class Class
        {
            public string Name;
            public short Texture_Male;
            public short Texture_Female;
        }

        [Serializable]
        public struct Maps
        {
            public short Revision;
            public string Name;
            public byte Width;
            public byte Height;
            public byte Moral;
            public byte Panorama;
            public byte Music;
            public int Color;
            public Map_Weather Weather;
            public Map_Fog Fog;
            public short[] Link;
            public Map_Tile[,] Tile;
            public Map_Light[] Light;
            public short[] NPC;
        }

        public struct Temp_Map
        {
            public Map_NPCs[] NPC;
            public Map_Items[] Item;
        }

        [Serializable]
        public struct Map_Weather
        {
            public byte Type;
            public byte Intensity;
        }

        [Serializable]
        public struct Map_Fog
        {
            public byte Texture;
            public sbyte Speed_X;
            public sbyte Speed_Y;
            public byte Alpha;
        }

        [Serializable]
        public struct Map_Tile
        {
            public byte Attribute;
            public bool[] Block;
            public Map_Tile_Data[,] Data;
        }

        [Serializable]
        public struct Map_Tile_Data
        {
            public byte X;
            public byte Y;
            public byte Tile;
            public bool Automatic;
            public System.Drawing.Point[] Mini;
        }

        [Serializable]
        public struct Map_Light
        {
            public byte X;
            public byte Y;
            public byte Width;
            public byte Height;
        }

        public struct Weather
        {
            public bool Visible;
            public int x;
            public int y;
            public int Speed;
            public int Start;
            public bool Back;
        }

        public struct NPCs
        {
            public string Name;
            public short Texture;
            public byte Type;
            public short[] Vital;
        }

        public struct Map_NPCs
        {
            // Apenas na parte do cliente
            public short X2;
            public short Y2;
            public byte Animation;
            public bool Attacking;
            public int Attack_Timer;
            public int Hurt;
            // Geral
            public short Index;
            public byte X;
            public byte Y;
            public Game.Directions Direction;
            public Game.Movements Movement;
            public short[] Vital;
        }

        public struct Map_Items
        {
            public short Index;
            public byte X;
            public byte Y;
        }

        public struct Items
        {
            // Geral
            public string Name;
            public string Description;
            public short Texture;
            public byte Type;
            // Requerimentos
            public short Req_Level;
            public byte Req_Class;
            // Poção
            public short Potion_Experience;
            public short[] Potion_Vital;
            // Equipamento
            public byte Equip_Type;
            public short[] Equip_Attribute;
            public short Weapon_Damage;
        }

        public struct Inventory
        {
            public short Item_Num;
            public short Amount;
        }

        public struct Hotbar
        {
            public byte Type;
            public byte Slot;
        }
    }
}