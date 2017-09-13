﻿using DMSkin.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DMSKIN.WPF.DEMO2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : DMSkinWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSkin_Click(object sender, RoutedEventArgs e)
        {
            DMFullScreen = true;
            WindowState = WindowState.Maximized;
        }
    }
}
