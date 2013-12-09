using System;
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

            Loaded += MainWindow_Loaded;
            Closing+= MainWindow_Closing;
        }

        void MainWindow_Loaded( object sender, RoutedEventArgs e )
        {
            scratch.OnBroadcast += scratch_OnBroadcast;
            scratch.OnSensorUpdate += scratch_OnSensorUpdate;

            ListReceive.Items.Add( "hoge" );
        }

        void scratch_OnBroadcast( object sender, string value )
        {
            ListReceive.Items.Add( value );
        }

        void scratch_OnSensorUpdate( object sender, Dictionary<string, string> value )
        {
            string v = "";
            foreach ( var val in value ) {
                v += string.Format( "{0},{1} ", val.Key, val.Value );
            }

            ListReceive.Items.Add( v );
        }

        void MainWindow_Closing( object sender, System.ComponentModel.CancelEventArgs e )
        {
            scratch.Close();
        }

        private void Button_Click( object sender, RoutedEventArgs e )
        {
            try {
                count++;
                scratch.AddSensorValue( "hoge", count.ToString() );
                scratch.SensorUpdate();
            }
            catch ( Exception ex ) {
                MessageBox.Show( ex.Message );
            }
        }

        private void Button_Click_1( object sender, RoutedEventArgs e )
        {
            try {
                scratch.Broadcast( "bar" );
            }
            catch ( Exception ex ) {
                MessageBox.Show( ex.Message );
            }
        }

        private void Button_Click_2( object sender, RoutedEventArgs e )
        {
            try {
                scratch.Connect();
            }
            catch ( Exception ex ) {
                MessageBox.Show( ex.Message );
            }
        }
    }
}
