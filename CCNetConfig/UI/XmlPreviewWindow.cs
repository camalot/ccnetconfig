using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core;
using System.Xml;

namespace CCNetConfig.UI
{
    /// <summary>
    /// Previews the output XML for the configuration.
    /// </summary>
    public partial class XmlPreviewWindow : Form
    {
        /// <summary>
        /// Initialises a new instance of <see cref="XmlPreviewWindow"/>.
        /// </summary>
        public XmlPreviewWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Generates the preview.
        /// </summary>
        /// <param name="value"></param>
        public void GeneratePreview(CruiseControl value)
        {
            // Generate the output XML
            var builder = new StringBuilder();
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };
            var writer = XmlWriter.Create(builder, settings);
            value.Serialize().OwnerDocument.Save(writer);

            // Display it
            xmlPreview.IsReadOnly = false;
            xmlPreview.Text = builder.ToString();
            xmlPreview.IsReadOnly = true;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
