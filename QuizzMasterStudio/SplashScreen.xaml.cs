#region License
/*   QuizzMasterStudio Copyright © 2012 Robert Long
 *  
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *   The full license is also included in the root folder.
 */
#endregion

#region Contact

/* Robert Long
 * Email: rbtLong@live.com
 */

#endregion

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
using System.Windows.Shapes;
using System.Threading;

namespace QuizzMasterStudio
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        SynchronizationContext synch;
        Thread thd;

        public SplashScreen()
        {
            InitializeComponent();
            synch = SynchronizationContext.Current;
            this.Loaded += new RoutedEventHandler(SplashScreen_Loaded);
            this.Closing += new System.ComponentModel.CancelEventHandler(SplashScreen_Closing);

            thd = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(1650);
                synch.Post((o) => this.DialogResult = true, null);
            }));
        }

        void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            thd.Start();
        }

        void SplashScreen_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(thd != null && thd.IsAlive)
                thd.Abort();

            this.DialogResult = true;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
