﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScratchNet.Sample
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        Scratch scratch = new Scratch();
        int count;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            count++;
            scratch.AddSensorValue( "hoge", count.ToString() );
            scratch.SensorUpdate();
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            scratch.Broadcast( "foo" );
            scratch.Broadcast( "bar" );
        }
    }
}
