﻿using System;
using System.Collections.Generic;
using DesktopApp.Base_classes;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using System.Linq;
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
                    MainSettingsModel.Height = height;
                    MainSettingsModel.Width = width;
                    MainSettingsModel.ObstaclesCount = obstaclesCount;
                    MainSettingsModel.PatchesOfGrassCount = patchesOfGrassCount;
                    MainSettingsModel.QuicksandSinkholesCount = quicksandSinkholesCount;
                    MainSettingsModel.WaterSourcesCount = waterSourcesCount;
                }
            }
            Rows = MainSettingsModel.Height;
            Columns = MainSettingsModel.Width;
            if (File.Exists("PlayerSettings.xml"))
            {
                var xmldoc = new XmlDataDocument();
                var fs = new FileStream("PlayerSettings.xml", FileMode.Open, FileAccess.Read);
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
            var waters = MainSettingsModel.WaterSourcesCount;
            var grasses = MainSettingsModel.PatchesOfGrassCount;
            var rocks = MainSettingsModel.ObstaclesCount;
            var quicksands = MainSettingsModel.QuicksandSinkholesCount;

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
            try
            {
                IterateCoyotes();
                IterateMice();
            }
            finally
            {
                timer.Enabled = true;
            }
        }

        private void GiveBirthToAChild(int indexOfParent, ElementType et)
        {
            var adjacentSpots = GetAdjacentSpots(indexOfParent);
            var sands = adjacentSpots.Where(x => Items[x].ElementType == ElementType.Sand).ToList();
            if (sands.Count == 0) return;
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                var randomIndex = sands[_rnd.Next(0, sands.Count)];
                switch (et)
                {
                    case ElementType.Coyote:
                        Items[randomIndex] = new Coyote();
                        _coyoteIndexes.Add(randomIndex);
                        break;
                    case ElementType.PocketMouse:
                        Items[randomIndex] = new PocketMouse();
                        _miceIndexes.Add(randomIndex);
                        break;
                }
            });
        }

        private void ShowCoyoteIndexes()
        {
            var str = string.Join(", ", _coyoteIndexes.Select(x => x.ToString()));
            MessageBox.Show(str);
        }

        private void IterateCoyotes()
        {
            for (var i = 0; i < _coyoteIndexes.Count; i++)
            {
                //ShowCoyoteIndexes();
                var adjacentSpots = GetAdjacentSpots(_coyoteIndexes[i]);
                if (adjacentSpots.Count == 0) continue;
                var randomStep = _rnd.Next(0, adjacentSpots.Count);
                var currentCoyote = Items[_coyoteIndexes[i]] as Coyote;
                if (currentCoyote is null) continue;
                //lock (_coyoteIndexes)
                //{
                    Application.Current.Dispatcher.Invoke((Action)delegate
                   {
                       if (currentCoyote.Age != 0 && currentCoyote.Age % 3 == 0)
                       {
                           GiveBirthToAChild(_coyoteIndexes[i], ElementType.Coyote);
                       }
                       else
                       {
                           switch (Items[adjacentSpots[randomStep]].ElementType)
                           {
                               case ElementType.Coyote:
                               case ElementType.Rock:
                                   currentCoyote.Dehydration += 10;
                                   currentCoyote.Starvation += 10;
                                   return;
                               case ElementType.Quicksand:
                                   Items[_coyoteIndexes[i]] = new Element();
                                   _coyoteIndexes.RemoveAt(i);
                                   i--;
                                   return;
                               case ElementType.Water:
                                   currentCoyote.Dehydration -= 10;
                                   currentCoyote.Starvation += 10;
                                   break;
                               case ElementType.PocketMouse:
                                   Items[adjacentSpots[randomStep]] = currentCoyote;
                                   _miceIndexes.Remove(adjacentSpots[randomStep]);
                                   Items[_coyoteIndexes[i]] = new Element();
                                   _coyoteIndexes[i] = adjacentSpots[randomStep];
                                   currentCoyote.Starvation -= 10;
                                   currentCoyote.Dehydration += 10;
                                   break;
                               default:
                                   Items[adjacentSpots[randomStep]] = currentCoyote;
                                   Items[_coyoteIndexes[i]] = new Element();
                                   _coyoteIndexes[i] = adjacentSpots[randomStep];
                                   currentCoyote.Dehydration += 10;
                                   currentCoyote.Starvation += 10;
                                   break;
                           }
                       }

                       currentCoyote.Age++;

                       if (currentCoyote.Dehydration >= 1000 || currentCoyote.Starvation >= 1000)
                       {
                           //lock (this)
                           //{
                               Application.Current.Dispatcher.Invoke((Action)delegate
                                              {
                                                          Items[_coyoteIndexes[i]] = new Element();
                                                          _coyoteIndexes.RemoveAt(i);
                                                          i--;
                                                      });
                           //}
                       }
                   });
                //}
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
                var currentMouse = Items[_miceIndexes[i]] as PocketMouse;
                if(currentMouse is null) continue;
                Application.Current.Dispatcher.Invoke((Action)delegate
                {
                    if (currentMouse.Age != 0 && currentMouse.Age % 3 == 0)
                    {
                        GiveBirthToAChild(_miceIndexes[i], ElementType.PocketMouse);
                    }
                    else
                    {
                        switch (Items[adjacentSpots[randomStep]].ElementType)
                        {
                            case ElementType.Coyote:
                            case ElementType.Rock:
                            case ElementType.PocketMouse:
                                currentMouse.Dehydration += 10;
                                currentMouse.Starvation += 10;
                                return;
                            case ElementType.Quicksand:
                                Items[_miceIndexes[i]] = new Element();
                                _miceIndexes.RemoveAt(i);
                                i--;
                                return;
                            case ElementType.Water:
                                currentMouse.Dehydration -= 10;
                                currentMouse.Starvation += 10;
                                break;
                            case ElementType.Grass:
                                Items[adjacentSpots[randomStep]] = currentMouse;
                                Items[_miceIndexes[i]] = new Element();
                                _miceIndexes[i] = adjacentSpots[randomStep];
                                currentMouse.Starvation -= 10;
                                currentMouse.Dehydration += 10;
                                break;
                            default:
                                Items[adjacentSpots[randomStep]] = currentMouse;
                                Items[_miceIndexes[i]] = new Element();
                                _miceIndexes[i] = adjacentSpots[randomStep];
                                currentMouse.Dehydration += 10;
                                currentMouse.Starvation += 10;
                                break;
                        }
                    }

                    currentMouse.Age++;

                    if (currentMouse.Dehydration >= 1000 || currentMouse.Starvation >= 1000)
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