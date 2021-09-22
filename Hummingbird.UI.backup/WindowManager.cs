// <copyright file="WindowManager.cs" company="Huaxing YUAN">
// Copyright (c) 2013-2018 Huaxing YUAN, All Rights Reserved
// </copyright>
// <author>Huaxing YUAN</author>
// <date>2016-10-12  21:06</date>
// <summary>Hummingbird.UI, A Modern UI Framework based on WPF and MVVM, originally developed for Hummingbird Test Studio</summary>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Hummingbird.UI
{
    [Serializable]
    internal class WindowManager
    {
        internal static WindowManager Instance { get; private set; } = new WindowManager();
        Dictionary<string, WindowPosition> Windows { get; set; }

        internal void RestoreWindowPosition(BasicWindow window) {
            string key = window.GetType().FullName;
            if (WindowManager.Instance.Windows.ContainsKey(key))
            {
                var v = WindowManager.Instance.Windows[window.GetType().FullName];
                if (v.Left > SystemParameters.VirtualScreenLeft && v.Left < SystemParameters.VirtualScreenWidth + SystemParameters.VirtualScreenLeft)
                {
                    window.Left = v.Left + 8;
                }
                if (v.Top > SystemParameters.VirtualScreenTop && v.Top < SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight)
                {
                    window.Top = v.Top + 30;
                }
                if (v.WindowState == WindowState.Maximized)
                {
                    window.Width = 1280;
                    window.Height = 800;
                    window.WindowState = WindowState.Maximized;
                }else
                {
                    window.Width = v.Width;
                    window.Height = v.Height;


                }
            }
        }
        internal void SaveWindowPosition(BasicWindow window)
        {
            Instance.Windows[window.GetType().FullName] = new WindowPosition()
            {
                Left = window.Left,
                Top = window.Top,
                Width = window.ActualWidth,
                Height = window.ActualHeight,
                WindowState = window.WindowState,
            };
        }

        private WindowManager()
        {
            try
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Hummingbird";
                string path = dir + "\\WindowManager";
                StreamReader sr = new StreamReader(path, Encoding.UTF8);
                string content = sr.ReadToEnd();
                sr.Close();

                byte[] bArray = System.Convert.FromBase64String(content);
                Stream bStream = new MemoryStream(bArray);
                BinaryFormatter bformatter = new BinaryFormatter();
                this.Windows = ((WindowManager)(bformatter.Deserialize(bStream))).Windows;
                bStream.Close();

            }
            catch
            {
                this.Windows = new Dictionary<string, WindowPosition>();
            }
        }

        ~WindowManager()
        {
            try
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Hummingbird";
                string path = dir + "\\WindowManager";

                MemoryStream bStream = new MemoryStream();
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(bStream, this);
                byte[] bArray = bStream.ToArray();
                String base64String = System.Convert.ToBase64String(bArray, 0, bArray.Length);
                bStream.Close();
                System.IO.StreamWriter sr = new System.IO.StreamWriter(path, false, Encoding.UTF8);
                sr.Write(base64String);
                sr.Close();

            }
            catch {
                //nothing to do here
            }
        }
    }
}
