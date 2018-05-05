﻿using System;
using System.Collections.Generic;
using DesktopApp.Base_classes;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using DesktopApp.Base_classes.Elements;
using DesktopApp.Players;
using System.Timers;

namespace DesktopApp
{
    public class MainViewModel : ViewModel<MainWindow>
    {
        public static System.Timers.Timer timer;

        private ObservableCollection<Element> _items;

        private Random _rnd = new Random();
        private List<int> _coyoteIndexes = new List<int>();
        private List<int> _miceIndexes = new List<int>();
        private byte _babyBornCount = 1;

        public ObservableCollection<Element> Items
        {
            get { return _items; }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    Notify();
                }
            }
        }

        private int _rows;
        private int _columns;

        public void Show()
        {
            View.Show();
        }

        public MainViewModel()
        {
            timer = new System.Timers.Timer(2000);
            timer.Elapsed += Iterate;
            if (File.Exists("MainSettings.xml"))
            { 
                XmlDataDocument xmldoc = new XmlDataDocument();
                FileStream fs = new FileStream("MainSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[0].InnerText, out var height);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[1].InnerText, out var width);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[2].InnerText, out var patchesOfGrassCount);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[3].InnerText, out var obstaclesCount);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[4].InnerText, out var quicksandSinkholesCount);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[5].InnerText, out var waterSourcesCount);

                if (height == 0 || width == 0 || patchesOfGrassCount == 0 || waterSourcesCount == 0 || quicksandSinkholesCount == 0 || obstaclesCount == 0)
                {
                    MessageBox.Show("Please insert correct numbers!!!");
                }
                else
                {
                    MainSettings.Height = height;
                    MainSettings.Width = width;
                    MainSettings.ObstaclesCount = obstaclesCount;
                    MainSettings.PatchesOfGrassCount = patchesOfGrassCount;
                    MainSettings.QuicksandSinkholesCount = quicksandSinkholesCount;
                    MainSettings.WaterSourcesCount = waterSourcesCount;
                }
            }
            Rows = MainSettings.Height;
            Columns = MainSettings.Width;
            if (File.Exists("PlayerSettings.xml"))
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                FileStream fs = new FileStream("PlayerSettings.xml", FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[0].InnerText, out var starvationCoyote);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[1].InnerText, out var dResultehydrationCoyote);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[2].InnerText, out var gestationCoyote);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[3].InnerText, out var lifetimeCoyote);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[4].InnerText, out var starvationPocket);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[5].InnerText, out var dehydrationPocket);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[6].InnerText, out var gestationPocket);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[7].InnerText, out var lifetimePocket);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[8].InnerText, out var countOnDesertCoyot);
                int.TryParse(xmldoc.ChildNodes[1].ChildNodes[9].InnerText, out var countOnDesertPocket);
                if (starvationCoyote == 0 || dResultehydrationCoyote == 0 || gestationCoyote == 0 ||
                    lifetimeCoyote == 0 || starvationCoyote == 0 || starvationPocket == 0 || dehydrationPocket == 0 ||
                    gestationPocket == 0 || lifetimePocket == 0)
                {
                    MessageBox.Show("Please insert correct numbers!!!");
                }
                else
                {
                    PlayerSettings.StarvationCoyote = starvationCoyote;
                    PlayerSettings.DehydrationCoyote = dResultehydrationCoyote;
                    PlayerSettings.GestationCoyote = gestationCoyote;
                    PlayerSettings.LifetimeCoyote = lifetimeCoyote;
                    PlayerSettings.StarvationPocket = starvationPocket;
                    PlayerSettings.DehydrationPocket = dehydrationPocket;
                    PlayerSettings.GestationPocket = gestationPocket;
                    PlayerSettings.LifetimePocket = lifetimePocket;
                    PlayerSettings.CountOnDesertCoyot = countOnDesertCoyot;
                    PlayerSettings.CountOnDesertPocket = countOnDesertPocket;
                }
            }

            Items = new ObservableCollection<Element>();
            for (var i = 0; i < Rows * Columns; i++)
            {
                Items.Add(new Element());
            }

            FillElements();
            SetPlayers();

            Notify(nameof(Items));
        }

        private void FillElements()
        {
            var waters = MainSettings.WaterSourcesCount;
            var grasses = MainSettings.PatchesOfGrassCount;
            var rocks = MainSettings.ObstaclesCount;
            var quicksands =MainSettings.QuicksandSinkholesCount;

            int index;
            for (int i = 0; i < waters; i++)
            {
                index = _rnd.Next(0, Rows * Columns);
                while (Items[index].Color != "peru")
                {
                    index = _rnd.Next(0, Rows * Columns);
                }
                Items[index] = new Water();
            }
            for (int i = 0; i < grasses; i++)
            {
                index = _rnd.Next(0, Rows * Columns);
                while (Items[index].Color != "peru")
                {
                    index = _rnd.Next(0, Rows * Columns);
                }
                Items[index] = new Grass();
            }
            for (int i = 0; i < rocks; i++)
            {
                index = _rnd.Next(0, Rows * Columns);
                while (Items[index].Color != "peru")
                {
                    index = _rnd.Next(0, Rows * Columns);
                }
                Items[index] = new Rock();
            }
            for (int i = 0; i < quicksands; i++)
            {
                index = _rnd.Next(0, Rows * Columns);
                while (Items[index].Color != "peru")
                {
                    index = _rnd.Next(0, Rows * Columns);
                }
                Items[index] = new Quicksand();
            }
        }

        private void SetPlayers()
        {
            var mice = PlayerSettings.CountOnDesertPocket;
            var coyotes = PlayerSettings.CountOnDesertCoyot;

            int index;
            for (int i = 0; i < mice; i++)
            {
                index = _rnd.Next(0, Rows * Columns);
                while (Items[index].Color != "peru" || Items[index].Name != "")
                {
                    index = _rnd.Next(0, Rows * Columns);
                }
                _miceIndexes.Add(index);
                Items[index] = new PocketMouse();
            }
            for (int i = 0; i < coyotes; i++)
            {
                index = _rnd.Next(0, Rows * Columns);
                while (Items[index].Color != "peru" || Items[index].Name != "")
                {
                    index = _rnd.Next(0, Rows * Columns);
                }
                _coyoteIndexes.Add(index);
                Items[index] = new Coyote();
            }
        }

        private void Iterate(object sender, ElapsedEventArgs e)
        {
            timer.Enabled = false;
            IterateCoyotes();
            IterateMice();
            timer.Enabled = true;
        }

        private void GiveBirthToAChild(int indexOfParent)
        {

        }

        private void IterateCoyotes()
        {
            for (var i = 0; i < _coyoteIndexes.Count; i++)
            {
                var adjacentSpots = GetAdjacentSpots(_coyoteIndexes[i]);
                if (adjacentSpots.Count == 0) continue;
                var randomStep = _rnd.Next(0, adjacentSpots.Count);
                var currentCoyote = Items[_coyoteIndexes[i]];
                Application.Current.Dispatcher.Invoke((Action)delegate
               {
                   switch (Items[adjacentSpots[randomStep]].ElementType)
                   {
                       case ElementType.Coyote:
                       case ElementType.Rock:
                           ((Coyote)currentCoyote).Dehydration += 10;
                           ((Coyote)currentCoyote).Starvation += 10;
                           return;
                       case ElementType.Quicksand:
                           Items[_coyoteIndexes[i]] = new Element();
                           _coyoteIndexes.RemoveAt(i);
                           i--;
                           return;
                       case ElementType.Water:
                           ((Coyote)currentCoyote).Dehydration -= 10;
                           ((Coyote)currentCoyote).Starvation += 10;
                           break;
                       case ElementType.PocketMouse:
                           Items[adjacentSpots[randomStep]] = currentCoyote;
                           Items[_coyoteIndexes[i]] = new Element();
                           _coyoteIndexes[i] = adjacentSpots[randomStep];
                           ((Coyote)currentCoyote).Starvation -= 10;
                           ((Coyote)currentCoyote).Dehydration += 10;
                           break;
                       default:
                           Items[adjacentSpots[randomStep]] = currentCoyote;
                           Items[_coyoteIndexes[i]] = new Element();
                           _coyoteIndexes[i] = adjacentSpots[randomStep];
                           ((Coyote)currentCoyote).Dehydration += 10;
                           ((Coyote)currentCoyote).Starvation += 10;
                           break;
                   }
                   if (((Coyote)currentCoyote).Dehydration >= 1000 || ((Coyote)currentCoyote).Starvation >= 1000)
                   {
                       Application.Current.Dispatcher.Invoke((Action)delegate
                       {
                           Items[_coyoteIndexes[i]] = new Element();
                           _coyoteIndexes.RemoveAt(i);
                           i--;
                       });
                   }
               });
            }
            Notify(nameof(Items));
        }

        private void IterateMice()
        {
            for (var i = 0; i < _miceIndexes.Count; i++)
            {
                var adjacentSpots = GetAdjacentSpots(_miceIndexes[i]);
                if (adjacentSpots.Count == 0) continue;
                var randomStep = _rnd.Next(0, adjacentSpots.Count);
                var currentMice = Items[_miceIndexes[i]];
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    switch (Items[adjacentSpots[randomStep]].ElementType)
                    {
                        case ElementType.Coyote:
                        case ElementType.Rock:
                        case ElementType.PocketMouse:
                            ((PocketMouse)currentMice).Dehydration += 10;
                            ((PocketMouse)currentMice).Starvation += 10;
                            return;
                        case ElementType.Quicksand:
                            Items[_miceIndexes[i]] = new Element();
                            _miceIndexes.RemoveAt(i);
                            i--;
                            return;
                        case ElementType.Water:
                            ((PocketMouse)currentMice).Dehydration -= 10;
                            ((PocketMouse)currentMice).Starvation += 10;
                            break;
                        case ElementType.Grass:
                            Items[adjacentSpots[randomStep]] = currentMice;
                            Items[_miceIndexes[i]] = new Element();
                            _miceIndexes[i] = adjacentSpots[randomStep];
                            ((PocketMouse)currentMice).Starvation -= 10;
                            ((PocketMouse)currentMice).Dehydration += 10;
                            break;
                        default:
                            Items[adjacentSpots[randomStep]] = currentMice;
                            Items[_miceIndexes[i]] = new Element();
                            _miceIndexes[i] = adjacentSpots[randomStep];
                            ((PocketMouse)currentMice).Dehydration += 10;
                            ((PocketMouse)currentMice).Starvation += 10;
                            break;
                    }
                    if (((PocketMouse)currentMice).Dehydration >= 1000 || ((PocketMouse)currentMice).Starvation >= 1000)
                    {
                        Application.Current.Dispatcher.Invoke((Action)delegate
                        {
                            Items[_miceIndexes[i]] = new Element();
                            _miceIndexes.RemoveAt(i);
                            i--;
                        });
                    }
                });
            }
            Notify(nameof(Items));
        }

        private List<int> GetAdjacentSpots(int index)
        {
            var list = new List<int>();
            var rowIndex = index / Columns;
            var columnIndex = index % Columns;
            if (rowIndex - 1 >= 0) list.Add(index - Columns);
            if (rowIndex + 1 <= Rows - 1) list.Add(index + Columns);
            if (columnIndex - 1 >= 0) list.Add(index - 1);
            if (columnIndex + 1 <= Columns - 1) list.Add(index + 1);
            if (rowIndex - 1 >= 0 && columnIndex - 1 >= 0) list.Add(index - Columns - 1);
            if (rowIndex - 1 >= 0 && columnIndex + 1 <= Columns - 1) list.Add(index - Columns + 1);
            if (rowIndex + 1 <= Rows - 1 && columnIndex - 1 >= 0) list.Add(index + Columns - 1);
            if (rowIndex + 1 <= Rows - 1 && columnIndex + 1 <= Columns - 1) list.Add(index + Columns + 1);

            return list;
        }

        public int Rows
        {
            get { return _rows; }
            set
            {
                if (_rows != value)
                {
                    _rows = value;
                    Notify();
                }
            }
        }

        public int Columns
        {
            get { return _columns; }
            set
            {
                if (_columns != value)
                {
                    _columns = value;
                    Notify();
                }
            }
        }
    }
}