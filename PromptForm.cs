using System;
using System.Windows.Forms;

namespace DriveMounter
{
    public partial class PromptForm : Form
    {
        private PromptForm(string text, string caption)
        {
            InitializeComponent();

            lblPrompt.Text = text;
            Text = caption;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        public static string Show(string text, string caption)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (caption is null)
            {
                throw new ArgumentNullException(nameof(caption));
            }

            var form = new PromptForm(text, caption);
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                return form.txtUserInput.Text;
            }
            else
            {
                return null;
            }
        }
    }
}
