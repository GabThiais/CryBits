﻿using System;
using System.Windows.Forms;

class Program
{
    // Usado para manter a aplicação aberta
    public static bool Working = true;

    [STAThread]
    static void Main()
    {
        // Inicia o dispositivo de rede
        Socket.Init();

        // Carrega as opções
        Read.Options();

        // Inicia a aplicação
        Login.Objects.Visible = true;
        Application.EnableVisualStyles();
        Loop.Main();
    }

    public static void Close()
    {
        int Wait_Timer = Environment.TickCount;

        // Desconecta da rede
        Socket.Disconnect();

        // Espera até que o jogador seja desconectado
        while (Socket.IsConnected() && Environment.TickCount <= Wait_Timer + 1000)
            Application.DoEvents();

        // Fecha a aplicação
        Application.Exit();
    }

    // Usado para criar um vinculo entre as plataformas na hora de serializar
    public class Binder : System.Runtime.Serialization.SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            assemblyName = assemblyName.Replace("Server", "Editors");
            return Type.GetType(string.Format("{0}, {1}", typeName, assemblyName));
        }
    }
}