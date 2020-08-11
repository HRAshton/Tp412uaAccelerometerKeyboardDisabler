using System;
using System.Windows.Forms;

namespace Tp412uaAccelerometerKeyboardDisabler
{
    internal class ContextMenus
    {
        public ContextMenuStrip Create()
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;

            item = new ToolStripMenuItem
            {
                Text = "E&xit and turn on input"
            };
            item.Click += new System.EventHandler(Exit_Click);
            menu.Items.Add(item);

            return menu;
        }

        private void Exit_Click(object sender, EventArgs e) => Application.Exit();
    }
}
