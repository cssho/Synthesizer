using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
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
using Synthesizer.Model;
using Synthesizer.Module;

namespace Synthesizer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainViewModel();
            DataContext = vm;
            Observable.FromEvent<KeyEventHandler, KeyEventArgs>(h => (sender, e) => h(e),
                    h => KeyDown += h,
                    h => KeyDown -= h)
                .Where(KeyInput.IsTargetKey)
                .Select(x => new KeyInput { IsDown = true, TargetKey = x.Key })
                .Merge(Observable.FromEvent<KeyEventHandler, KeyEventArgs>(h => (sender, e) => h(e),
                        h => KeyUp += h,
                        h => KeyUp -= h)
                    .Where(KeyInput.IsTargetKey)
                    .Select(x => new KeyInput { IsDown = false, TargetKey = x.Key }))
                .Subscribe(vm.Pipeline);
            QSlider.Maximum = 100;
            QSlider.Minimum = double.Epsilon;
        }
    }
}
