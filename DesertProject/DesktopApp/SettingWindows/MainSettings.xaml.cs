﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace DesktopApp
{
    /// <summary>
    /// Interaction logic for MainSetings 
    /// </summary>
    public partial class MainSettings : Window
    {
        public MainSettings()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            if (!File.Exists("MainSettings.xml"))
            {
                Height.Text = "15";
                Width.Text = "20";
                PatchesOfGrassCount.Text = "4";
                ObstaclesCount.Text = "5";
                QuicksandSinkholesCount.Text = "14";
                WaterSourcesCount.Text = "4";
            }
            else
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                FileStream fs = new FileStream("MainSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                Height.Text = xmldoc.ChildNodes[1].ChildNodes[0].InnerText;
                Width.Text = xmldoc.ChildNodes[1].ChildNodes[1].InnerText;
                PatchesOfGrassCount.Text = xmldoc.ChildNodes[1].ChildNodes[2].InnerText;
                ObstaclesCount.Text = xmldoc.ChildNodes[1].ChildNodes[3].InnerText;
                QuicksandSinkholesCount.Text = xmldoc.ChildNodes[1].ChildNodes[4].InnerText;
                WaterSourcesCount.Text = xmldoc.ChildNodes[1].ChildNodes[5].InnerText;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(Height.Text, out var x);
            int.TryParse(Width.Text, out var y);
            int.TryParse(ObstaclesCount.Text, out var obstaclesCount);
            int.TryParse(PatchesOfGrassCount.Text, out var patchesOfGrassCount);
            int.TryParse(QuicksandSinkholesCount.Text, out var quicksandSinkholesCount);
            int.TryParse(WaterSourcesCount.Text, out var waterSourcesCount);
            if (x != 0 && y != 0 && obstaclesCount != 0 && patchesOfGrassCount != 0 && quicksandSinkholesCount != 0 &&
                waterSourcesCount != 0)
                try
                {
                    using (XmlWriter writer = XmlWriter.Create("MainSettings.xml"))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("MainSetting");

                        writer.WriteElementString("Height", Height.Text);
                        writer.WriteElementString("Width", Width.Text);
                        writer.WriteElementString("PatchesOfGrassCount", PatchesOfGrassCount.Text);
                        writer.WriteElementString("ObstaclesCount", ObstaclesCount.Text);
                        writer.WriteElementString("QuicksandSinkholesCount", QuicksandSinkholesCount.Text);
                        writer.WriteElementString("WaterSourcesCount", WaterSourcesCount.Text);

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                    MainWindow.Restart();
                    Close();
                }
                catch{/*ignored*/}
            else
            {
                MessageBox.Show("Incorrect insert !!!");
            }
        }
    }
}
